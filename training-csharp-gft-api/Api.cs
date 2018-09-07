using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TrainingCsharpGft.Api
{
    public class Accounts : IStore, INotifyPropertyChanged
    {
        public Dictionary<string, Account> accounts = new Dictionary<string, Account>();

        public event PropertyChangedEventHandler PropertyChanged;

        public Accounts() //Examplary accounts
        {
            accounts.Add("A1", new Account() { Name = "A1", Ballance = 1000 });
            accounts.Add("A2", new Account() { Name = "A2", Ballance = 2000 });
            accounts.Add("A3", new Account() { Name = "A3", Ballance = 3000 });
        }

        public Dictionary<string, Account> Get()
        {
            Dictionary<string, Account> accountsDictionary = new Dictionary<string, Account>(accounts);
            return accountsDictionary;
        }

        public void Put(Account account)
        {
            if(!accounts.Keys.Contains(account.Name))
            {
                accounts.Add(account.Name, account);
                OnPropertyChanged("accountsAdded");
            }
            else
            {
                throw new Exception("Account with this name already exists!");
            }
        }

        public void Delete(string accountName)
        {
            if(accounts.ContainsKey(accountName))
            {
                accounts.Remove(accountName);
                OnPropertyChanged("accountsRemoved");
            }
            else
            {
                throw new Exception("Account with this name does not exists!");
            }
        }

        public void Transfer(string chargedAccountName, string toppedUpAccountName, double amount)
        {
            if(accounts[chargedAccountName].Ballance >= amount)
            {
                accounts[chargedAccountName].Ballance -= amount;
                accounts[toppedUpAccountName].Ballance += amount;
            }
            else
            {
                throw new Exception("Insufficient funds");
            }
        }

        public void TopUp(string toppedUpAccountName, double amount)
        {
            accounts[toppedUpAccountName].Ballance += amount;
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}