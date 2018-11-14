using System;
using System.Threading;
using TrainingCsharpGft.Api.Exceptions;

namespace TrainingCsharpGft.Api.Model
{
    public class AmountManager
    {
        private IStore persistance;
        object obj = new object();


        public AmountManager(IStore persistance)
        {
            this.persistance = persistance;
        }

        public void Transfer(string chargedAccountName, string toppedUpAccountName, double amount)
        {
            Account chargedAccount = persistance.Get(chargedAccountName);
            Account toppedUpAccount = persistance.Get(toppedUpAccountName);

            try
            {
                Thread.Sleep(2000);
                //Sleep before an actuall action prevents UI from updating transfer amount before action is done
                chargedAccount.Subtract(amount);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            try
            {
                Thread.Sleep(2000);
                toppedUpAccount.Add(amount);
            }
            catch(Exception ex)
            {
                chargedAccount.Add(amount);
                throw ex;
            }                 
        }

        public void TopUp(string toppedUpAccountName, double amount)
        {
            Thread.Sleep(1000);
            if(String.IsNullOrEmpty(toppedUpAccountName))
                throw new WrongValueException("Empty account name was given");
            if (amount <= 0)
                throw new WrongValueException("Given amount was 0 or lower");
            Account ac = persistance.Get(toppedUpAccountName);
            ac.Add(amount);
        }
    }
}
