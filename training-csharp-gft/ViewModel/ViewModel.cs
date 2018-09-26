using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TrainingCsharpGft.Commands;
using TrainingCsharpGft.Api;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using TrainingCsharpGft.Api.Model;

namespace TrainingCsharpGft.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public Command ExecuteTopUpAccountCommand { get; set; }
        public Command ExecuteDeleteYourAccountCommand { get; set; }
        public Command ExecuteTransferToSelectedAccountCommand { get; set; }
        public Command ExecuteOpenCreateNewAccountWindowCommand { get; set; }
        public Command ExecuteCreateNewAccountCommand { get; set; }

        public static AccountsManager accountsManager = new AccountsManager();
        public Accounts api = new Accounts(accountsManager);
        public AmountManager amountManager = new AmountManager(accountsManager);
        Dictionary<string, Account> accounts = null;
        Account selectedAccount = null;

        public ObservableCollection<string> cbo_accountToTransferItems { get; set; }
        public ObservableCollection<string> lb_accountsItems { get; set; }
        private string _lb_accountsSelectedItem;
        public string lb_accountsSelectedItem
        {
            get { return _lb_accountsSelectedItem; }
            set
            {
                if(value == null)
                {
                    selectedAccount = null;
                    _lb_accountsSelectedItem = null;
                    cbo_accountToTransferItems.Clear();
                    return;
                }
                _lb_accountsSelectedItem = value;
                selectedAccountNameLabel = value;
                selectedAccount = accounts[value];
                selectedAccountBallanceText = selectedAccount.Ballance.ToString();

                cbo_accountToTransferItems.Clear();
                foreach (var key in accounts.Keys)
                    if(key != value)
                        cbo_accountToTransferItems.Add(key);

                CheckCanExecuteMethods();
            }
        }

        private string _selectedAccountNameLabel = "select one...";
        public string selectedAccountNameLabel
        {
            get { return _selectedAccountNameLabel; }
            set
            {
                _selectedAccountNameLabel = value;
                OnPropertyChanged("selectedAccountNameLabel");
            }
        }

        private string _cbo_accountToTransferSelectedItem;
        public string cbo_accountToTransferSelectedItem
        {
            get { return _cbo_accountToTransferSelectedItem; }
            set
            {
                _cbo_accountToTransferSelectedItem = value;
                OnPropertyChanged("cbo_accountToTransferSelectedItem");
                ExecuteTransferToSelectedAccountCommand.RaiseCanExecuteChanged(null);
            }
        }
        

        private string _selectedAccountBallanceText = "";
        public string selectedAccountBallanceText
        {
            get
            {
                return _selectedAccountBallanceText;
            }
            set
            {
                _selectedAccountBallanceText = value;
                OnPropertyChanged("selectedAccountBallanceText");
            }
        }

        private double _topUpAmount;
        public double topUpAmount
        {
            get { return _topUpAmount; }
            set
            {
                _topUpAmount = value;
                ExecuteTopUpAccountCommand.RaiseCanExecuteChanged(null);
            }
        }

        private double _transferAmount;
        public double transferAmount
        {
            get { return _transferAmount; }
            set
            {
                _transferAmount = value;
                ExecuteTransferToSelectedAccountCommand.RaiseCanExecuteChanged(null);
            }
        }

        private void LoadSelectedAccountData()
        {
            try
            {
                selectedAccount = accounts[_lb_accountsSelectedItem];
                OnPropertyChanged("selectedAccountBallanceText");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        public ViewModel()
        {
            ExecuteTopUpAccountCommand = new Command(ExecuteTopUpAccount, canExecuteTopUpAccount);
            ExecuteDeleteYourAccountCommand = new Command(ExecuteDeleteYourAccount, canExecuteDeleteYourAccount);
            ExecuteTransferToSelectedAccountCommand = new Command(ExecuteTransferToSelectedAccount, canExecuteTransferToSelectedAccount);
            ExecuteCreateNewAccountCommand = new Command(ExecuteCreateNewAccount, canExecuteCreateNewAccount);
            ExecuteOpenCreateNewAccountWindowCommand = new Command(ExecuteOpenCreateNewAccountWindow, canExecuteOpenCreateNewAccountWindow);

            cbo_accountToTransferItems = new ObservableCollection<string>();
            lb_accountsItems = new ObservableCollection<string>();
            FetchAccountsFromApi();
            LoadAccountsToMainListBox();
        }

        private void CheckCanExecuteMethods()
        {
            ExecuteDeleteYourAccountCommand.RaiseCanExecuteChanged(null);
            ExecuteTopUpAccountCommand.RaiseCanExecuteChanged(null);
            ExecuteTransferToSelectedAccountCommand.RaiseCanExecuteChanged(null);
        }

        private void CleanLabels()
        {
            selectedAccount = null;
            selectedAccountBallanceText = "";
            selectedAccountNameLabel = "";
        }

        private void FetchAccountsFromApi()
        {
            try
            {
                accounts = api.GetAccounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        public void ReloadAccountsDataFromApi()
        {
            FetchAccountsFromApi();
            LoadSelectedAccountData();
        }

        private void ExecuteTransferToSelectedAccount(object parameter)
        {
            try
            {
                amountManager.Transfer(selectedAccount.Name, _cbo_accountToTransferSelectedItem, transferAmount);
                FetchAccountsFromApi();
                selectedAccountBallanceText = selectedAccount.Ballance.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        private bool canExecuteTransferToSelectedAccount(object parameter)
        {
            if (selectedAccount != null && selectedAccount.Name.Length > 0 && transferAmount > 0 &&
                cbo_accountToTransferSelectedItem != null && cbo_accountToTransferSelectedItem.Length > 0)
                return true;
            return false;
        }

        private void ExecuteTopUpAccount(object parameter)
        {
            try
            {
                amountManager.TopUp(selectedAccount.Name, topUpAmount);
                FetchAccountsFromApi();
                selectedAccountBallanceText = selectedAccount.Ballance.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        private bool canExecuteTopUpAccount(object parameter)
        {
            if (selectedAccount != null && selectedAccount.Name.Length > 0 && topUpAmount > 0)
                return true;
            return false;
        }

        private void ExecuteDeleteYourAccount(object parameter)
        {
            try
            {
                api.DeleteAccount(selectedAccount.Name);
                FetchAccountsFromApi();

                lb_accountsItems.Clear();
                LoadAccountsToMainListBox();

                CleanLabels();
                CheckCanExecuteMethods();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        private bool canExecuteDeleteYourAccount(object parameter)
        {
            if (selectedAccount != null && selectedAccount.Name.Length > 0)
                return true;
            return false;
        }

        private string createdAccountName;
        public string CreatedAccountName
        {
            get { return createdAccountName; }
            set
            {
                createdAccountName = value;
                ExecuteCreateNewAccountCommand.RaiseCanExecuteChanged(null);
            }
        }

        private double createdAccountInitialAmount;
        public double CreatedAccountInitialAmount
        {
            get { return createdAccountInitialAmount; }
            set
            {
                createdAccountInitialAmount = value;
                ExecuteCreateNewAccountCommand.RaiseCanExecuteChanged(null);
            }
        }

        private void ExecuteOpenCreateNewAccountWindow(object parameter)
        {
            CreateNewAccountWindow createNewAccountWindow = new CreateNewAccountWindow(this)
            {
                Owner = (MainWindow)Application.Current.MainWindow,
                Name = "CreateNewAccountWindow"
            };
            createNewAccountWindow.ShowDialog();

            FetchAccountsFromApi();
            lb_accountsItems.Clear();
            LoadAccountsToMainListBox();

            CleanLabels();
            CheckCanExecuteMethods();
        }

        private void LoadAccountsToMainListBox()
        {
            foreach (var key in accounts.Keys)
                lb_accountsItems.Add(key);
        }

        private bool canExecuteOpenCreateNewAccountWindow(object parameter)
        {
            return true;
        }

        private void ExecuteCreateNewAccount(object parameter)
        {
            try
            {
                api.AddNewAccount(new Account() { Name = createdAccountName, Ballance = createdAccountInitialAmount });

                Window currentWindow = null;
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Name == "CreateNewAccountWindow")
                    {
                        currentWindow = window;
                    }
                }
                currentWindow?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        private bool canExecuteCreateNewAccount(object parameter)
        {
            if (createdAccountName != null && createdAccountName.Length > 0 && createdAccountInitialAmount > 0)
                return true;
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}
