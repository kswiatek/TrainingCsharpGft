using NUnit.Framework;
using TrainingCsharpGft.Api;
using TrainingCsharpGft.Api.Model;

namespace TrainingCsharpGft.Tests.Model
{
    class AccountsManagerTests
    {
        private IStore storage;
        
        [SetUp]
        public void TestSetUp()
        {
            storage = new AccountsManager();
        }

        [TestCase("test1", 500)]
        [TestCase("test2")]
        [TestCase("test3", 2500)]
        public void AccountsManagerShouldPersistAccountOnPut(string accountName, double accountBallance = 0)
        {
            //Arrange
            var testAccount = new Account { Name = accountName };
            if (accountBallance > 0)
                testAccount.Add(accountBallance);
            //Act
            storage.Put(testAccount);
            //Assert
            Assert.AreEqual(testAccount, storage.Get(testAccount.Name));
        }

        [Test]
        public void AccountsManagerShouldReturnAllAccountsOnGetAllAccounts()
        {
            var testAccount1 = new Account { Name = "test1" };
            var testAccount2 = new Account { Name = "test2" };
            var testAccount3 = new Account { Name = "test3" };
            testAccount2.Add(200);

            storage.Put(testAccount1);
            storage.Put(testAccount2);
            storage.Put(testAccount3);

            CollectionAssert.Contains(storage.GetAllAccounts(), testAccount1);
            CollectionAssert.Contains(storage.GetAllAccounts(), testAccount2);
            CollectionAssert.Contains(storage.GetAllAccounts(), testAccount3);
        }

        [TestCase("test1")]
        [TestCase("test2")]
        [TestCase("test3")]
        public void AccountsManagerShouldDeleteSpecifiedAccountOnDelete(string accountName)
        {
            var testAccount1 = new Account { Name = "test1" };
            var testAccount2 = new Account { Name = "test2" };
            var testAccount3 = new Account { Name = "test3" };

            storage.Put(testAccount1);
            storage.Put(testAccount2);
            storage.Put(testAccount3);
            var accountToDelete = storage.Get(accountName);
            storage.Delete(accountName);

            CollectionAssert.DoesNotContain(storage.GetAllAccounts(), accountToDelete);
        }

    }
}
