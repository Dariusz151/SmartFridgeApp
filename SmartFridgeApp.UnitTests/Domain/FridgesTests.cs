using NUnit.Framework;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class FridgesTests
    {
        private Fridge _fridge;
        private User _user;
        private const string userName = "Dario";
        private const string userEmail = "dario@gmail.com";

        [SetUp]
        public void BaseSetUp() {
            string fridgeName = "lodówa";
            string fridgeAddress = "Solika 5";
            string fridgeDesc = "BEKO";
            _fridge = new Fridge(fridgeName, fridgeAddress, fridgeDesc);
        } 

        [Test]
        public void AddUserToFridgeShouldHaveOneUser()
        {
            _user = new User(userName, userEmail);
            _fridge.AddUser(_user);

            var countUsers = _fridge.GetFridgeUsers().Count;

            Assert.AreEqual(1, countUsers);
        }

        [Test]
        public void AddSameUserToFridgeShouldThrowDomainException()
        {
            _user = new User(userName, userEmail);
            _fridge.AddUser(_user);

            Assert.Throws(typeof(DomainException), () => _fridge.AddUser(_user));
        }

        [Test]
        public void CreateFridgeShouldAddOneDomainEvent()
        {
            Assert.AreEqual(1, _fridge.DomainEvents.Count);
        }
    }
}
