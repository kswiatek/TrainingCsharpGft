using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TrainingCsharpGft.Api;
using TrainingCsharpGft.Api.Model;
using TrainingCsharpGft.Api.Exceptions;
using Moq;

namespace TrainingCsharpGft.Tests.Model
{
    class AmountManagerTests
    {
        private IStore storage;
        private List<Account> accounts = new List<Account>();
        private AmountManager amountManager;

        [SetUp]
        public void TestSetUp()
        {
            accounts.Clear();

            var mockStorage = new Mock<IStore>();
            mockStorage.Setup(x => x.Put(It.IsAny<Account>())).Callback<Account>(acc => accounts.Add(acc));
            mockStorage.Setup(x => x.Get(It.IsAny<string>())).Returns((string accName) => accounts.First(acc => acc.Name == accName));
            storage = mockStorage.Object;

            amountManager = new AmountManager(storage);
            var A1 = new Account { Name = "Acc1"};
            var A2 = new Account { Name = "Acc2"};
            var A3 = new Account { Name = "Acc3"};
            A1.Add(1000);
            A2.Add(2000);
            A3.Add(3000);
            storage.Put(A1);
            storage.Put(A2);
            storage.Put(A3);
        }

        [TestCase("Acc2", "Acc1", 300, "Acc2", 1700)]
        [TestCase("Acc2", "Acc1", 300, "Acc1", 1300)]
        [TestCase("Acc2", "Acc1", 1500, "Acc3", 3000)]
        [TestCase("Acc2", "Acc1", 0, "Acc1", 1000)]
        [TestCase("Acc2", "Acc1", 0, "Acc2", 2000)]
        [TestCase("Acc2", "Acc1", 0, "Acc3", 3000)]
        [TestCase("Acc3", "Acc1", 2900, "Acc3", 100)]
        public void AmountManagerShouldTransferSpecifiedAmountBetweenAccountsOnTransfer(string chargedAccountName, 
            string toppedUpAccountName, double amount, string checkingAccountName, double expectedBallance)
        {
            amountManager.Transfer(chargedAccountName, toppedUpAccountName, amount);

            Assert.AreEqual(expectedBallance, storage.Get(checkingAccountName).Ballance);
        }

        [TestCase("Acc1", 50.5)]
        [TestCase("Acc1", 0)]
        [TestCase("Acc1", 4500)]
        public void AmountManagerShouldTopUpSpecifiedAccountWithGivenAmountOnTopUp(string accountName, double amount)
        {
            double ballance = storage.Get(accountName).Ballance;

            try
            {
                amountManager.TopUp("Acc1", amount);
            }
            catch(Exception ex)
            {
                if (ex.GetType() != typeof(WrongValueException))
                    throw ex;
            }

            Assert.AreEqual(ballance + amount, storage.Get(accountName).Ballance);
        }

        [TestCase(-100)]
        [TestCase(-1)]
        [TestCase(-0.654)]
        public void AmountManagerShouldNotAcceptNegativeValuesOnTopUp_WrongValueExceptionShouldBeThrown(double value)
        {
            Assert.Throws<WrongValueException> (() =>
            {
                amountManager.TopUp("Acc1", value);
            });
        }

        [Test]
        public void BallanceOfAnyAccountShouldNotBecomeLessThanZero_InsufficientFundsExceptionShouldBeThrown()
        {
            Assert.Throws<InsufficientFundsException>(() => 
            {
                amountManager.Transfer("Acc2", "Acc1", 2010);
            });
            Assert.AreEqual(1000, storage.Get("Acc1").Ballance);
            Assert.AreEqual(2000, storage.Get("Acc2").Ballance);
        }

        [Test]
        [Repeat(50)]
        [Explicit]
        public void MultipleThreadsShouldNotBeAbleToTakeBiggerAmountFromAnyAccountThanItsBallance()
        {
            Exception ex = Assert.Throws<AggregateException>(() =>
            {
                Task t1 = Task.Factory.StartNew(() =>
                {
                    amountManager.Transfer("Acc1", "Acc2", 700);
                });

                Task t2 = Task.Factory.StartNew(() =>
                {
                    amountManager.Transfer("Acc1", "Acc2", 400);
                });

                Task.WaitAll(t1, t2);
            });

            Assert.Contains(storage.Get("Acc1").Ballance, new List<double>() { 300, 600 });
            Assert.Contains(storage.Get("Acc2").Ballance, new List<double>() { 2700, 2400 });
        }

        [Test]
        [Repeat(50)]
        [Explicit]
        public void MultipleThreadsShouldNotBeAbleToTakeBiggerAmountFromAnyAccountThanItsBallanceManually()
        {
            Account testAcc1 = storage.Get("Acc1");
            Account testAcc2 = storage.Get("Acc2");

            Exception ex = Assert.Throws<AggregateException>(() =>
            {
                Task t1 = Task.Factory.StartNew(() =>
                {
                    testAcc1.Subtract(700);
                    Thread.Sleep(1000);
                    testAcc2.Add(700);
                });

                Task t2 = Task.Factory.StartNew(() =>
                {
                    testAcc1.Subtract(400);
                    Thread.Sleep(1000);
                    testAcc2.Add(400);
                });
                Task.WaitAll(t1, t2);
            });

            Assert.Contains(storage.Get("Acc1").Ballance, new List<double>() { 300, 600 });
            Assert.Contains(storage.Get("Acc2").Ballance, new List<double>() { 2700, 2400 });
            //Only one of given operations should perform
        }

        [Test]
        [Repeat(50)]
        [Explicit]
        public void MultipleThreadsShouldNotInterfereWhilePerformingMultipleTransfers()
        {
            Task t1 = Task.Factory.StartNew(() =>
            {
                amountManager.Transfer("Acc2", "Acc3", 1000);
                Thread.Sleep(1000);
            });

            Task t2 = Task.Factory.StartNew(() =>
            {
                amountManager.Transfer("Acc1", "Acc2", 200);
            });

            Task t3 = Task.Factory.StartNew(() =>
            {
                amountManager.Transfer("Acc3", "Acc2", 300);
                Thread.Sleep(1000);
            });

            Task.WaitAll(t1, t2, t3);

            Assert.AreEqual(800, storage.Get("Acc1").Ballance);
            Assert.AreEqual(1500, storage.Get("Acc2").Ballance);
            Assert.AreEqual(3700, storage.Get("Acc3").Ballance);
        }

    }
}
