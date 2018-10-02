using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrainingCsharpGft.Api.Model
{
    public class AccountsManager : IStore
    {
        private List<Account> accounts;

        public AccountsManager()
        {
            accounts = new List<Account>();
            //tests
            accounts.Add(new Account() { Name = "A1", Ballance = 1000 });
            accounts.Add(new Account() { Name = "A2", Ballance = 2000 });
            accounts.Add(new Account() { Name = "A3", Ballance = 3000 });
        }

        public void Put(Account account)
        {
            if (!accounts.Any((acc) => acc.Name == account.Name))
            {
                Thread.Sleep(2000);
                accounts.Add(account);
            }
            else
            {
                throw new Exception("Account with this name already exists!");
            }
        }

        public Account Get(string accountName)
        {
            return accounts.First(x => x.Name == accountName);
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            Thread.Sleep(3000);
            return accounts;
        }

        public void Delete(string accountName)
        {
            Thread.Sleep(2000);
            Account acc = accounts.First(x => x.Name == accountName);
            accounts.Remove(acc);
        }

    }
}
