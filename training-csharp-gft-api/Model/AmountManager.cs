using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrainingCsharpGft.Api.Model
{
    public class AmountManager
    {
        private IStore persistance;

        public AmountManager(IStore persistance)
        {
            this.persistance = persistance;
        }

        public void Transfer(string chargedAccountName, string toppedUpAccountName, double amount)
        {
            Thread.Sleep(1000);
            Account chargedAccount = persistance.Get(chargedAccountName);
            Account toppedUpAccount = persistance.Get(toppedUpAccountName);
            if (chargedAccount.Ballance >= amount)
            {
                chargedAccount.Ballance -= amount;
                toppedUpAccount.Ballance += amount;
            }
            else
            {
                throw new Exception("Insufficient funds");
            }
        }

        public void TopUp(string toppedUpAccountName, double amount)
        {
            Thread.Sleep(1000);
            Account ac = persistance.Get(toppedUpAccountName);
            ac.Ballance += amount;
        }
    }
}
