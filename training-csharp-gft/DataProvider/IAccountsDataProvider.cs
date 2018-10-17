using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCsharpGft.Api;

namespace TrainingCsharpGft.DataProvider
{
    public interface IAccountsDataProvider
    {
        Dictionary<string, Account> GetAccounts();
    }
}
