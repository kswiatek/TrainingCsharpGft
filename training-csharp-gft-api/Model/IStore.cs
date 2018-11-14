using System.Collections.Generic;

namespace TrainingCsharpGft.Api
{
    public interface IStore
    {
        void Put(Account account);
        Account Get(string accountName);
        IEnumerable<Account> GetAllAccounts();
        void Delete(string accountName);
    }
}
