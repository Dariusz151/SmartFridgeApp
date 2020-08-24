using NUnit.Framework;
using SmartFridgeApp.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.Models.Users;
using SmartFridgeApp.Domain.SeedWork.Exceptions;

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

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(55)]
        public void AddUsersToFridgeShouldHaveAmountOfUsersInList(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _user = new User(userName, userEmail);
                _fridge.AddUser(_user);
            }
            
            var countUsers = _fridge.GetFridgeUsers().Count;

            Assert.AreEqual(count, countUsers);
        }

        [Test]
        public void AddSameUserToFridgeShouldThrowDomainException()
        {
            _user = new User(userName, userEmail);
            _fridge.AddUser(_user);

            Assert.Throws(typeof(DomainException), () => _fridge.AddUser(_user));
        }

        [Test]
        public void CreateNewFridgeShouldAddOneDomainEvent()
        {
            Assert.AreEqual(1, _fridge.DomainEvents.Count);
        }

        [Test]
        public void RemoveUserFromFridgeShouldDeleteFromList()
        {
            _user = new User(userName, userEmail);
            _fridge.AddUser(_user);
            _fridge.RemoveUser(_user.Id);

            var usersCounts = _fridge.GetFridgeUsers().Count;

            Assert.AreEqual(3, _fridge.DomainEvents.Count);
            Assert.AreEqual(0, usersCounts);
        }

        [Test]
        public void RemoveUserThatNotExistShouldThrowException()
        {
            _user = new User(userName, userEmail);
            
            var usersCounts = _fridge.GetFridgeUsers().Count;

            Assert.AreEqual(1, _fridge.DomainEvents.Count);
            Assert.Throws(typeof(DomainException), () => _fridge.RemoveUser(_user.Id));
        }

        [Test]
        public void CreateFridgeWithEmptyNameShouldThrowException()
        {
            Fridge fridge2;
            Assert.Throws(typeof(DomainException), () => fridge2 = new Fridge("", "adress", "desc"));
        }

        [Test]
        public void UpdateFridgeWithInvalidNameShouldThrowException()
        {
            Assert.Throws(typeof(DomainException), () => _fridge.ChangeFridgeName(""));
        }

        [Test]
        public void UpdateFridgeWithValidNameShouldBeFine()
        {
            _fridge.ChangeFridgeName("UpdatedFridge");
           Assert.AreEqual("UpdatedFridge", _fridge.Name);
        }
    }
}
