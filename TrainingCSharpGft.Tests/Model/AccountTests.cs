using NUnit.Framework;
using System.Threading.Tasks;
using TrainingCsharpGft.Api;
using TrainingCsharpGft.Api.Exceptions;
using TrainingCsharpGft.Api.Model;

namespace TrainingCsharpGft.Tests.Model
{
    /*
     * These tests are intended only to affect a single Account object 
     * without using any intermediary elements
     */
    class AccountTests
    {
        Account testAccount;

        [SetUp]
        public void TestSetUp()
        {
            testAccount = new Account() { Name = "testAccount" };
        }

        [Test]
        public void AccountsBallanceShouldEqualZeroIfNotSpecified()
        {
            Assert.AreEqual(testAccount.Ballance, 0);
        }

        [TestCase(0)]
        [TestCase(5)]
        [TestCase(54675)]
        public void AccountShouldIncreaseItsBallanceOnAdd(double amount)
        {
            testAccount.Add(amount);

            Assert.AreEqual(testAccount.Ballance, amount);
        }

        [TestCase(0)]
        [TestCase(50)]
        [TestCase(500)]
        public void AccountShouldDecreaseItsBallanceOnSubtract(double amount)
        {
            testAccount.Add(500);
            testAccount.Subtract(amount);

            Assert.AreEqual(testAccount.Ballance, 500 - amount);
        }

        [TestCase(501)]
        [TestCase(5000)]
        public void AccountShouldNotDecreaseItsBallanceBelowZero_ValueShouldNotChange_InsufficientFundsExceptionShoulbBeThrown(
            double amount)
        {
            testAccount.Add(500);
            
            Assert.Throws<InsufficientFundsException>(() =>
            {
                testAccount.Subtract(amount);
            });
            Assert.AreEqual(testAccount.Ballance, 500);
        }

        [Test]
        [Repeat(1000)]
        [Explicit]
        public void AccountShouldBeThreadSafeOnMultipleThreadsCallingAdd()
        {
            Task t1 = Task.Factory.StartNew(() =>
            {
                for(int i = 0; i < 50; i++)
                    testAccount.Add(10); //Should finally increase testAccount ballance by 500
            });

            Task t2 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 50; i++)
                    testAccount.Add(5); //Should finally increase testAccount ballance by 250
            });

            Task.WaitAll(t1, t2);

            Assert.AreEqual(testAccount.Ballance, 750);
        }

        [Test]
        [Repeat(1000)]
        [Explicit]
        public void AccountShouldBeThreadSafeOnMultipleThreadsCallingSubtract()
        {
            testAccount.Add(1000);

            Task t1 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 50; i++)
                    testAccount.Subtract(10); //Should finally decrease testAccount ballance by 500
            });

            Task t2 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 50; i++)
                    testAccount.Subtract(5); //Should finally decrease testAccount ballance by 250
            });

            Task.WaitAll(t1, t2);

            Assert.AreEqual(testAccount.Ballance, 250);
        }

        [Test]
        [Repeat(1000)]
        [Explicit]
        public void AccountShouldBeThreadSafeOnMultipleThreadsCallingAddAndSubtractAtTheSameTime()
        {
            testAccount.Add(1000);

            Task t1 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 50; i++)
                    testAccount.Add(10); //Should finally increase testAccount ballance by 500
            });

            Task t2 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 50; i++)
                    testAccount.Subtract(5); //Should finally decrease testAccount ballance by 250
            });

            Task.WaitAll(t1, t2);

            Assert.AreEqual(testAccount.Ballance, 1250);
        }

    }
}
