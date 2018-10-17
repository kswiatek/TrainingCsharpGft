using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCsharpGft.Api
{
    public class Account
    {
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
        }

        public void Subtract(double amount)
        {
            lock (obj)
            {
                if (amount <= ballance)
                    ballance -= amount;
                else
                    throw new Exception("Insufficient funds");
            }
        }

    }
}
