using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        [Test]
        public void AmountManagerShouldTransferSpecifiedAmountBetweenAccountsOnTransfer()
        {
            amountManager.Transfer("Acc2", "Acc1", 300);

            Assert.AreEqual(1300, storage.Get("Acc1").Ballance);
            Assert.AreEqual(1700, storage.Get("Acc2").Ballance);
        }

        [Test]
        public void AmountManagerShouldTopUpSpecifiedAccountWithGivenAmountOnTopUp()
        {
            double ballance = storage.Get("Acc1").Ballance;

            amountManager.TopUp("Acc1", 50.5);

            Assert.AreEqual(ballance + 50.5, storage.Get("Acc1").Ballance);
        }

        [Test]
        public void BallanceOfAnyAccountShouldNotBecomeLessThanZeroAndApprpriateTypeOfExceptionShouldBeThrown()
        {
            Assert.Throws<Exception>(() => 
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
