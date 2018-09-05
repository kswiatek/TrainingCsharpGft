using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCsharpGft.Api
{
    public class Account
    {
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
            set { ballance = value; }
        }

    }
}
