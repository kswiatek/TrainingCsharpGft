using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace TrainingCsharpGft.Api.Model
{
    public class AccountsManager : IStore
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private const string accountsDataFilePath = "AccountsData.dat";
        private List<Account> accounts;

        public AccountsManager()
        {
            accounts = new List<Account>();
            LoadAccountsFromFile(accountsDataFilePath);
        }

        private void LoadAccountsFromFile(string filePath)
        {
            if(!File.Exists(filePath))
            {
                log.Info($"No accounts data have been found.");
                return;
            }
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    var list = (List<Account>)bf.Deserialize(stream);
                    foreach (var account in list)
                    {
                        accounts.Add(account);
                    }
                    if(list.Count == 1)
                        log.Info($"{list.Count} account has been successfully loaded.");
                    else if (list.Count > 1)
                        log.Info($"{list.Count} accounts have been successfully loaded.");
                    else
                        log.Info($"No accounts have been loaded.");
                }
            }
            catch(Exception ex)
            {
                log.Error($"There was a problem with loading accounts data from {filePath}:\n {ex.Message}");
            }
        }

        public void SaveAccountsToFile(string filePath = accountsDataFilePath)
        {
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(stream, accounts);
                    log.Info($"Accounts data has been successfully saved. Closing application.");
                }
            }
            catch(Exception ex)
            {
                log.Error($"There was a problem with saving accounts data to file: {filePath}:\n {ex.Message}");
            }
        }

        public void Put(Account account)
        {
            if (!accounts.Any((acc) => acc.Name == account.Name))
            {
                Thread.Sleep(2000);
                accounts.Add(account);
                log.Info($"Account {account.Name} successfully added.");
            }
            else
            {
                log.Error($"Attempted to create an account with name {account.Name} which already exists.");
                throw new Exception("Account with this name already exists!");
            }
        }

        public Account Get(string accountName)
        {
            log.Info($"Attempted to get {accountName} account data.");
            return accounts.First(x => x.Name == accountName);
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            log.Info($"Attempted to get all accounts data.");
            Thread.Sleep(3000);
            return accounts;
        }

        public void Delete(string accountName)
        {
            log.Info($"Attempted to delete account {accountName}.");
            Thread.Sleep(2000);
            Account acc = accounts.First(x => x.Name == accountName);
            accounts.Remove(acc);
            log.Info($"Account {accountName} successfully deleted.");
        }

    }
}
