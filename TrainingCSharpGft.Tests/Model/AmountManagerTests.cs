using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TrainingCsharpGft.Api;
using TrainingCsharpGft.Api.Model;

namespace TrainingCsharpGft.Tests.Model
{
    class AmountManagerTests
    {
        private IStore storage;
        private AmountManager amountManager;

        [SetUp]
        public void TestSetUp()
        {
            storage = new AccountsManager();
            amountManager = new AmountManager(storage);
        }

        [Test]
        public void AmountManagerShouldTransferSpecifiedAmountsBetweenAccountsOnTransfer()
        {
            var testAccount1 = new Account { Name = "test1", Ballance = 200 };
            var testAccount2 = new Account { Name = "test2", Ballance = 400 };

            storage.Put(testAccount1);
            storage.Put(testAccount2);
            amountManager.Transfer("test2", "test1", 300);

            Assert.AreEqual(testAccount1.Ballance, 500);
            Assert.AreEqual(testAccount2.Ballance, 100);
        }

        [Test]
        public void AmountManagerShoulTopUpSpecifiedAccountWithAmountOnTopUp()
        {
            var testAccount = new Account { Name = "test1", Ballance = 200 };

            storage.Put(testAccount);
            amountManager.TopUp("test1", 50.5);

            Assert.AreEqual(testAccount.Ballance, 250.5);
        }

    }
}
