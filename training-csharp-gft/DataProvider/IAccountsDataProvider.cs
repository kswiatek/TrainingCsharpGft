using System.Collections.Generic;
using TrainingCsharpGft.Api;

namespace TrainingCsharpGft.DataProvider
{
    public interface IAccountsDataProvider
    {
        Dictionary<string, Account> GetAccounts();
    }
}
