using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrainingCsharpGft.Api;

namespace TrainingCsharpGft
{
    /// <summary>
    /// Interaction logic for CreateNewAccountWindow.xaml
    /// </summary>
    public partial class CreateNewAccountWindow : Window
    {
        public CreateNewAccountWindow()
        {
            InitializeComponent();
            btn_createNewAccount.IsEnabled = false;
        }

        private void btn_createNewAccount_Click(object sender, RoutedEventArgs e)
        {
            Account createdAccount = new Account() { Name = txt_accountName.Text, Ballance = double.Parse(txt_accountInitialAmount.Text)};
            try
            {
                ((MainWindow)Application.Current.MainWindow).api.Put(createdAccount);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        private void txt_accountInitialAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = ((MainWindow)Application.Current.MainWindow).IsAmountInvalid(e.Text);
        }

        private void RefreshCreateAccountBtnAvailability()
        {
            if (txt_accountName.Text.Length > 0 && txt_accountInitialAmount.Text.Length > 0 && double.Parse(txt_accountInitialAmount.Text) > 0)
                btn_createNewAccount.IsEnabled = true;
            else
                btn_createNewAccount.IsEnabled = false;
        }

        private void txt_accountName_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshCreateAccountBtnAvailability();
        }

        private void txt_accountInitialAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshCreateAccountBtnAvailability();
        }
    }
}
