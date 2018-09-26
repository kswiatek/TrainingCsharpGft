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
    class AccountsManagerTests
    {
        private IStore storage;

        [SetUp]
        public void TestSetUp()
        {
            storage = new AccountsManager();
        }

        [Test]
        public void AccountsManagerShouldPersistAccountOnPut()
        {
            //Arrange
            var testAccount = new Account { Name = "test1"};
            //Act
            storage.Put(testAccount);
            //Assert
            Assert.AreEqual(testAccount, storage.Get(testAccount.Name));
        }
    }
}
