﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using TrainingCsharpGft.Api.Model;

namespace TrainingCsharpGft.Api
{
    public class Accounts : INotifyPropertyChanged
    {
        private IStore accountsManager;
        private AmountManager amountManager;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public Accounts(IStore accountsManager)
        {
            this.accountsManager = accountsManager;
            amountManager = new AmountManager(accountsManager);
        }

        public Dictionary<string, Account> GetAccounts()
        {
            return accountsManager.GetAllAccounts().ToDictionary(x => x.Name);
        }

        public Account GetAccount(string accountName)
        {
            return accountsManager.Get(accountName);
        }

        public void AddNewAccount(Account account)
        {
            accountsManager.Put(account);
        }

        public void DeleteAccount(string accountName)
        {
            accountsManager.Delete(accountName);
        }

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}