﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCsharpGft.Api
{
    interface IStore
    {
        void Put(Account account);
        IEnumerable<Account> Get();
        void Delete(string accountName);
    }
}