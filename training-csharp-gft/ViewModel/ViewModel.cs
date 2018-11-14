using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using TrainingCsharpGft.Commands;
using TrainingCsharpGft.Api;
using System.ComponentModel;
using System.Collections.ObjectModel;
using TrainingCsharpGft.Api.Model;

namespace TrainingCsharpGft.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            ExecuteTopUpAccountCommand = new Command(ExecuteTopUpAccount, canExecuteTopUpAccount);
            ExecuteDeleteYourAccountCommand = new Command(ExecuteDeleteYourAccount, canExecuteDeleteYourAccount);
            ExecuteTransferToSelectedAccountCommand = new Command(ExecuteTransferToSelectedAccount, canExecuteTransferToSelectedAccount);
            ExecuteCreateNewAccountCommand = new Command(ExecuteCreateNewAccount, canExecuteCreateNewAccount);
            ExecuteOpenCreateNewAccountWindowCommand = new Command(ExecuteOpenCreateNewAccountWindow, canExecuteOpenCreateNewAccountWindow);
            ExecuteSaveAccountsToFileOnAppExitCommand = new Command(ExecuteSaveAccountsToFileOnAppExit, canExecuteSaveAccountsToFileOnAppExit);

            cbo_accountToTransferItems = new ObservableCollection<string>();
            lb_accountsItems = new ObservableCollection<string>();

            FetchAccountsFromApi(true);
        }

        public Command ExecuteTopUpAccountCommand { get; set; }
        public Command ExecuteDeleteYourAccountCommand { get; set; }
        public Command ExecuteTransferToSelectedAccountCommand { get; set; }
        public Command ExecuteOpenCreateNewAccountWindowCommand { get; set; }
        public Command ExecuteCreateNewAccountCommand { get; set; }
        public Command ExecuteSaveAccountsToFileOnAppExitCommand { get; set; }

        public static AccountsManager accountsManager = new AccountsManager();
        public Accounts api = new Accounts(accountsManager);
        public AmountManager amountManager = new AmountManager(accountsManager);
        Dictionary<string, Account> accounts = null;
        Account selectedAccount = null;

        Queue<object> changingBallanceTasksQueue = new Queue<object>();

        public ObservableCollection<string> cbo_accountToTransferItems { get; set; }
        public ObservableCollection<string> lb_accountsItems { get; set; }

        private bool _lb_accountsEnabled = true;
        public bool lb_accountsEnabled
        {
            get { return _lb_accountsEnabled; }
            set
            {
                _lb_accountsEnabled = value;
                OnPropertyChanged("lb_accountsEnabled");
            }
        }

        private Visibility _lbl_updatingBallanceVisibility = Visibility.Hidden;
        public Visibility lbl_updatingBallanceVisibility
        {
            get { return _lbl_updatingBallanceVisibility; }
            set
            {
                _lbl_updatingBallanceVisibility = value;
                OnPropertyChanged("lbl_updatingBallanceVisibility");
            }
        }

        private Visibility _lbl_deletingAccountVisibility = Visibility.Hidden;
        public Visibility lbl_deletingAccountVisibility
        {
            get { return _lbl_deletingAccountVisibility; }
            set
            {
                _lbl_deletingAccountVisibility = value;
                OnPropertyChanged("lbl_deletingAccountVisibility");
            }
        }

        private bool _gb_ballanceChangingControlsAvailability = true;
        public bool gb_ballanceChangingControlsAvailability
        {
            get { return _gb_ballanceChangingControlsAvailability; }
            set
            {
                _gb_ballanceChangingControlsAvailability = value;
                OnPropertyChanged("gb_ballanceChangingControlsAvailability");
            }
        }

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
                OnPropertyChanged("transferAmount");
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

        public async void FetchAccountsFromApi(bool LoadAccountsToMainListBox)
        {
            if (LoadAccountsToMainListBox)
            {
                lb_accountsEnabled = false;
                lb_accountsItems.Add("updating accounts...");
            }
            try
            {                   
                var task = await Task.Run(() =>
                {
                    return api.GetAccounts();
                });
                
                accounts = task;
                if(LoadAccountsToMainListBox)
                {
                    lb_accountsItems.Clear();
                    foreach (var key in accounts.Keys)
                        lb_accountsItems.Add(key);
                    lb_accountsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
        }

        private async void ExecuteTransferToSelectedAccount(object parameter)
        {
            lbl_updatingBallanceVisibility = Visibility.Visible;
            try
            {
                await Task.Run(() =>
                {
                    changingBallanceTasksQueue.Enqueue(new object());
                    amountManager.Transfer(selectedAccount.Name, _cbo_accountToTransferSelectedItem, transferAmount);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
            finally
            {
                changingBallanceTasksQueue.Dequeue();
                if (changingBallanceTasksQueue.Count == 0)
                {
                    FetchAccountsFromApi(false);
                    selectedAccountBallanceText = selectedAccount.Ballance.ToString();
                    ExecuteTransferToSelectedAccountCommand.RaiseCanExecuteChanged(null);
                    lbl_updatingBallanceVisibility = Visibility.Hidden;
                }
            }
        }

        private bool canExecuteTransferToSelectedAccount(object parameter)
        {
            if (selectedAccount != null && transferAmount > selectedAccount.Ballance)
            {
                transferAmount = selectedAccount.Ballance;
                OnPropertyChanged("transferAmount");
            } 
            if (selectedAccount != null && selectedAccount.Name.Length > 0 && transferAmount > 0 &&
                transferAmount <= selectedAccount.Ballance && cbo_accountToTransferSelectedItem != null &&
                cbo_accountToTransferSelectedItem.Length > 0)
                return true;
            return false;
        }

        private async void ExecuteTopUpAccount(object parameter)
        {
           lbl_updatingBallanceVisibility = Visibility.Visible;
            try
            {
                await Task.Run(() => 
                {
                    changingBallanceTasksQueue.Enqueue(new object());
                    amountManager.TopUp(selectedAccount.Name, _topUpAmount);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not perform this operation: " + ex.Message);
            }
            finally
            {
                changingBallanceTasksQueue.Dequeue();
                if (changingBallanceTasksQueue.Count == 0)
                {
                    FetchAccountsFromApi(false);
                    selectedAccountBallanceText = selectedAccount.Ballance.ToString();
                    lbl_updatingBallanceVisibility = Visibility.Hidden;
                }
            }
        }

        private bool canExecuteTopUpAccount(object parameter)
        {
            if (selectedAccount != null && selectedAccount.Name.Length > 0 && _topUpAmount > 0)
                return true;
            return false;
        }

        private async void ExecuteDeleteYourAccount(object parameter)
        {
            gb_ballanceChangingControlsAvailability = false;
            lbl_deletingAccountVisibility = Visibility.Visible;
            try
            {
                await Task.Run(() =>
                {
                    api.DeleteAccount(selectedAccount.Name);
                });
                FetchAccountsFromApi(true);

                CleanLabels();
                CheckCanExecuteMethods();
                gb_ballanceChangingControlsAvailability = true;
                lbl_deletingAccountVisibility = Visibility.Hidden;
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

            FetchAccountsFromApi(true);

            CleanLabels();
            CheckCanExecuteMethods();
        }

        private bool canExecuteOpenCreateNewAccountWindow(object parameter)
        {
            return true;
        }

        private void ExecuteCreateNewAccount(object parameter)
        {
            try
            {
                Task.Run(() => 
                {
                    Account acc = new Account() { Name = createdAccountName };
                    acc.Add(createdAccountInitialAmount);
                    api.AddNewAccount(acc);
                });

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
            if (createdAccountName != null && createdAccountName.Length > 0 && createdAccountInitialAmount >= 0)
                return true;
            return false;
        }

        private void ExecuteSaveAccountsToFileOnAppExit(object parameter)
        {
            accountsManager.SaveAccountsToFile();
        }

        private bool canExecuteSaveAccountsToFileOnAppExit(object parameter)
        {
            return true;
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
