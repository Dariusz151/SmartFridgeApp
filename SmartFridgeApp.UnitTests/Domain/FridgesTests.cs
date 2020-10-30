using NUnit.Framework;
using SmartFridgeApp.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.Models.Users;
using SmartFridgeApp.Domain.SeedWork.Exceptions;
using SmartFridgeApp.Domain.Models.Fridges.Events;
using System.Linq;

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
        public void Fridge_AddSameUser_ShouldThrowDomainException()
        {
            _user = new User(userName, userEmail);
            _fridge.AddUser(_user);

            Assert.Throws(typeof(DomainException), () => _fridge.AddUser(_user));
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(55)]
        [Test]
        public void Fridge_AddUsers_ShouldHaveAmountOfUsersInList(int count)
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
        public void Fridge_CreateNew_ShouldAddOneDomainEvent()
        {
            Assert.AreEqual(1, _fridge.DomainEvents.Count);
            Assert.AreEqual(typeof(FridgeCreatedEvent), _fridge.DomainEvents.ElementAt(0).GetType());
        }

        [Test]
        public void ExistingFridge_AddUser_ShouldHaveTwoDomainEvents()
        {
            _user = new User(userName, userEmail);
            _fridge.AddUser(_user);
            Assert.AreEqual(2, _fridge.DomainEvents.Count);
            Assert.AreEqual(typeof(FridgeCreatedEvent), _fridge.DomainEvents.ElementAt(0).GetType());
            Assert.AreEqual(typeof(UserAddedEvent), _fridge.DomainEvents.ElementAt(1).GetType());

        }

        [Test]
        public void ExistingFridge_AddNullUser_ShouldThrowDomainException()
        {
            Assert.Throws(typeof(DomainException), () => _fridge.AddUser(_user));
            Assert.AreEqual(1, _fridge.DomainEvents.Count);
        }

        [Test]
        public void Fridge_RemoveUser_ShouldDeleteFromListAndHaveProperDomainEvents()
        {
            _user = new User(userName, userEmail);
            _fridge.AddUser(_user);
            _fridge.RemoveUser(_user.Id);

            var usersCounts = _fridge.GetFridgeUsers().Count;

            Assert.AreEqual(3, _fridge.DomainEvents.Count);
            Assert.AreEqual(0, usersCounts);
            Assert.AreEqual(typeof(FridgeCreatedEvent), _fridge.DomainEvents.ElementAt(0).GetType());
            Assert.AreEqual(typeof(UserAddedEvent), _fridge.DomainEvents.ElementAt(1).GetType());
            Assert.AreEqual(typeof(UserRemovedEvent), _fridge.DomainEvents.ElementAt(2).GetType());
        }

        [Test]
        public void Fridge_RemoveUserThatNotExist_ShouldThrowException()
        {
            _user = new User(userName, userEmail);
            
            var usersCounts = _fridge.GetFridgeUsers().Count;

            Assert.AreEqual(1, _fridge.DomainEvents.Count);
            Assert.Throws(typeof(UserNotExistException), () => _fridge.RemoveUser(_user.Id));
        }

        [Test]
        public void Fridge_CreateWithEmptyName_ShouldThrowException()
        {
            Fridge fridge2;
            Assert.Throws(typeof(DomainException), () => fridge2 = new Fridge("", "adress", "desc"));
        }

        [Test]
        public void Fridge_UpdateWithInvalidName_ShouldThrowException()
        {
            Assert.Throws(typeof(DomainException), () => _fridge.ChangeFridgeName(""));
        }

        [Test]
        public void Fridge_UpdateWithInvalidDesc_ShouldThrowException()
        {
            Assert.Throws(typeof(DomainException), () => _fridge.ChangeFridgeDesc(""));
        }

        [Test]
        public void Fridge_UpdateWithValidName_ShouldBeFine()
        {
            _fridge.ChangeFridgeName("UpdatedFridge");
           Assert.AreEqual("UpdatedFridge", _fridge.Name);
        }

        [Test]
        public void Fridge_UpdateWithValidDesc_ShouldBeFine()
        {
            _fridge.ChangeFridgeDesc("UpdatedFridgeDesc");
            Assert.AreEqual("UpdatedFridgeDesc", _fridge.Desc);
        }
    }
}
