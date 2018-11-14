using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using TrainingCsharpGft.Api.Exceptions;

namespace TrainingCsharpGft.Api
{
    [Serializable]
    public class Account
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private object obj = new object();
        private string name;
        private double ballance;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public double Ballance
        {
            get { return ballance; }
        }

        public void Add(double amount)
        {
            lock (obj)
            {
                ballance += amount;
            }
            log.Info($"Account {name} has increased its amount by {amount}. Its ballance is now {ballance}.");
        }

        public void Subtract(double amount)
        {
            lock (obj)
            {
                if (amount <= ballance)
                    ballance -= amount;
                else
                {
                    log.Error($"Insufficient funds - attempted to get {amount} " +
                        $"from account {name} but its ballance was {ballance}.");
                    throw new InsufficientFundsException();
                }
                log.Info($"Account {name} has decreased its amount by {amount}. Its ballance is now {ballance}.");
            }
        }

    }
}
