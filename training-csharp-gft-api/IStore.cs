using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCsharpGft.Api
{
    interface IStore
    {
        void Put(Account account);
        Dictionary<string, Account> Get();
        void Delete(string accountName);
    }
}
