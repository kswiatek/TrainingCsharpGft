using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCsharpGft.Api
{
    public class Accounts : IStore
    {
        public Dictionary<string, Account> accounts = new Dictionary<string, Account>();

        public Accounts() //Examplary accounts
        {
            accounts.Add("A1", new Account() { Name = "A1", Ballance = 1000 });
            accounts.Add("A2", new Account() { Name = "A2", Ballance = 2000 });
            accounts.Add("A3", new Account() { Name = "A3", Ballance = 3000 });
        }

        public IEnumerable<Account> Get()
        {
            List<Account> accountsList = new List<Account>();
            foreach(var a in accounts.Values)
            {
                accountsList.Add(a);
            }
            return accountsList;
        }

        public void Put(Account account)
        {
            if(!accounts.Keys.Contains(account.Name))
            {
                accounts.Add(account.Name, account);
            }
        }

        public void Delete(string accountName)
        {
            accounts.Remove(accountName);
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


    }
}