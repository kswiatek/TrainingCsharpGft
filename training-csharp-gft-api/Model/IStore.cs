using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCsharpGft.Api
{
    public interface IStore
    {
        void Put(Account account);
        Account Get(string accountId);
        IEnumerable<Account> GetAllAccounts();
        void Delete(string accountName);
    }
}
