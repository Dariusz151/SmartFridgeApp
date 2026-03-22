using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Linq;
using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class KitchensTests
    {
        private Kitchen _kitchen;

        [SetUp]
        public void BaseSetUp()
        {
            _kitchen = new Kitchen("lodowka", "Solika 5", "BEKO");
        }

        [Test]
        public void Fridge_CreateNew_ShouldAddOneDomainEvent()
        {
            ClassicAssert.AreEqual(1, _kitchen.DomainEvents.Count);
            ClassicAssert.AreEqual(typeof(KitchenCreatedEvent), _kitchen.DomainEvents.First().GetType());
        }

        [Test]
        public void Fridge_CreateWithEmptyName_ShouldThrowException()
        {
            Assert.Throws(typeof(InvalidInputException), () => _ = new Kitchen("", "address", "desc"));
        }

        [Test]
        public void Fridge_UpdateWithInvalidName_ShouldThrowException()
        {
            Assert.Throws(typeof(InvalidInputException), () => _kitchen.ChangeKitchenName(""));
        }

        [Test]
        public void Fridge_UpdateWithInvalidDesc_ShouldThrowException()
        {
            Assert.Throws(typeof(InvalidInputException), () => _kitchen.ChangeKitchenDesc(""));
        }

        [Test]
        public void Fridge_UpdateWithValidName_ShouldBeFine()
        {
            _kitchen.ChangeKitchenName("UpdatedFridge");
            ClassicAssert.AreEqual("UpdatedFridge", _kitchen.Name);
        }

        [Test]
        public void Fridge_UpdateWithValidDesc_ShouldBeFine()
        {
            _kitchen.ChangeKitchenDesc("UpdatedFridgeDesc");
            ClassicAssert.AreEqual("UpdatedFridgeDesc", _kitchen.Desc);
        }
    }
}