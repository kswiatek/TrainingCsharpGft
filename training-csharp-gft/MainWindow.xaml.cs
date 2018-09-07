using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrainingCsharpGft.Api;

namespace TrainingCsharpGft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Accounts api = new Accounts();
        Dictionary<string, Account> accounts = null;
        Account selectedAccount = null;

        public MainWindow()
        {
            InitializeComponent();
            FetchAccountsFromApi();
            SetAvailabilityOfAccountControls(false);
            api.PropertyChanged += AccountsChangedHandler;
        }

        private void AccountsChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "accountsAdded")
            {
                try
                {
                    accounts = api.Get();
                    lb_accounts.ItemsSource = accounts.Keys;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Could not perform this operation: " + ex.Message);
                }
            }
            else
            {
                selectedAccount = null;
                FetchAccountsFromApi();
                lb_accounts.ItemsSource = accounts.Keys;
            }
        }

        private void FetchAccountsFromApi()
        {
            try
            {
                accounts = api.Get();
                lb_accounts.ItemsSource = accounts.Keys;
                btn_transferToSelectedAccount.IsEnabled = false;
                btn_topUp.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        private void SetAvailabilityOfAccountControls(bool enable)
        {
            txt_topUpAmount.IsEnabled = enable;
            txt_transferAmount.IsEnabled = enable;
            cbo_accountToTransfer.IsEnabled = enable;
            btn_deleteYourAccount.IsEnabled = enable;
        }

        private void RefreshTransferToSelectedAccountBtnAvailability()
        {
            if (txt_transferAmount.Text.Length > 0 && double.Parse(txt_transferAmount.Text) > 0 && cbo_accountToTransfer.SelectedIndex > -1)
                btn_transferToSelectedAccount.IsEnabled = true;
            else
                btn_transferToSelectedAccount.IsEnabled = false;
        }

        private void RefreshTopUpBtnAvailability()
        {
            if (txt_topUpAmount.Text.Length > 0 && double.Parse(txt_topUpAmount.Text) > 0)
                btn_topUp.IsEnabled = true;
            else
                btn_topUp.IsEnabled = false;
        }

        private void lb_accounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lb_accounts.SelectedIndex != -1)
            {
                string selectedAccountName = e.AddedItems[0].ToString();
                selectedAccount = accounts[selectedAccountName];
                LoadSelectedAccountData();
            }
            else
            {
                tb_selectedAccount.Text = "select one...";
                tb_ballance.Text = "";
                SetAvailabilityOfAccountControls(false);
            }
        }

        private void LoadSelectedAccountData()
        {
            tb_selectedAccount.Text = selectedAccount.Name;
            tb_ballance.Text = selectedAccount.Ballance.ToString();
            cbo_accountToTransfer.Items.Clear();
            foreach (var item in accounts)
            {
                if (selectedAccount.Name != item.Key)
                {
                    cbo_accountToTransfer.Items.Add(item.Key);
                }
            }
            txt_transferAmount.Clear();
            SetAvailabilityOfAccountControls(true);
        }

        private void txt_topUpAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsAmountInvalid(e.Text);
        }

        private void txt_transferAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsAmountInvalid(e.Text);
        }

        public bool IsAmountInvalid(string input)
        {
            return new Regex("[^0-9]+").IsMatch(input);
        }

        private void txt_transferAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshTransferToSelectedAccountBtnAvailability();
            if (txt_transferAmount.Text.Length > 0 && double.Parse(txt_transferAmount.Text) > selectedAccount.Ballance)
            {
                txt_transferAmount.Text = selectedAccount.Ballance.ToString();
            }
        }

        private void cbo_accountToTransfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshTransferToSelectedAccountBtnAvailability();
        }

        private void txt_topUpAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshTopUpBtnAvailability();
        }

        public void ReloadAccountsDataFromApi()
        {
            try
            {
                accounts = api.Get();
                LoadSelectedAccountData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        private void btn_addNewAccount_Click(object sender, RoutedEventArgs e)
        {
            CreateNewAccountWindow createNewAccountWindow = new CreateNewAccountWindow
            {
                Owner = this
            };
            createNewAccountWindow.ShowDialog();
        }

        private void btn_topUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                api.TopUp(selectedAccount.Name, double.Parse(txt_topUpAmount.Text));
                ReloadAccountsDataFromApi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        private void btn_transferToSelectedAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                api.Transfer(selectedAccount.Name, cbo_accountToTransfer.Text, double.Parse(txt_transferAmount.Text));
                ReloadAccountsDataFromApi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        private void btn_deleteYourAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                api.Delete(selectedAccount.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

    }
}
