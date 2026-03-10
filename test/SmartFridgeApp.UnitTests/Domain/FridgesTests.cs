using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Linq;
using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class FridgesTests
    {
        private Fridge _fridge;

        [SetUp]
        public void BaseSetUp()
        {
            _fridge = new Fridge("lodowka", "Solika 5", "BEKO");
        }

        [Test]
        public void Fridge_CreateNew_ShouldAddOneDomainEvent()
        {
            ClassicAssert.AreEqual(1, _fridge.DomainEvents.Count);
            ClassicAssert.AreEqual(typeof(FridgeCreatedEvent), _fridge.DomainEvents.First().GetType());
        }

        [Test]
        public void Fridge_CreateWithEmptyName_ShouldThrowException()
        {
            Assert.Throws(typeof(InvalidInputException), () => _ = new Fridge("", "address", "desc"));
        }

        [Test]
        public void Fridge_UpdateWithInvalidName_ShouldThrowException()
        {
            Assert.Throws(typeof(InvalidInputException), () => _fridge.ChangeFridgeName(""));
        }

        [Test]
        public void Fridge_UpdateWithInvalidDesc_ShouldThrowException()
        {
            Assert.Throws(typeof(InvalidInputException), () => _fridge.ChangeFridgeDesc(""));
        }

        [Test]
        public void Fridge_UpdateWithValidName_ShouldBeFine()
        {
            _fridge.ChangeFridgeName("UpdatedFridge");
            ClassicAssert.AreEqual("UpdatedFridge", _fridge.Name);
        }

        [Test]
        public void Fridge_UpdateWithValidDesc_ShouldBeFine()
        {
            _fridge.ChangeFridgeDesc("UpdatedFridgeDesc");
            ClassicAssert.AreEqual("UpdatedFridgeDesc", _fridge.Desc);
        }
    }
}