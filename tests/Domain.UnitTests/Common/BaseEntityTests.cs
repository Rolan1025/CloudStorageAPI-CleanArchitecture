using System;
using System.Linq;
using Azure;
using FluentAssertions;
using NUnit.Framework;
using CloudStorageAPICleanArchitecture.Domain.Common;

namespace CloudStorageAPICleanArchitecture.Domain.UnitTests.Common
{
    // Clase derivada mínima para poder instanciar BaseEntity en pruebas
    public class DummyEntity : BaseEntity
    {
        // Puedes dejarla vacía, solo existe para no ser abstracta.
    }

    // Un evento de dominio mínimo para pruebas
    public class DummyEvent : BaseEvent
    {
        public string Data { get; }
        public DummyEvent(string data)
        {
            Data = data;
        }
    }

    [TestFixture]
    public class BaseEntityTests
    {
        [Test]
        public void Initially_NoDomainEvents()
        {
            // Arrange
            var entity = new DummyEntity();

            // Act
            var events = entity.DomainEvents;

            // Assert
            events.Should().BeEmpty("porque no se han agregado eventos aún");
        }

        [Test]
        public void AddDomainEvent_ShouldStoreEvent()
        {
            // Arrange
            var entity = new DummyEntity();
            var domainEvent = new DummyEvent("test");

            // Act
            entity.AddDomainEvent(domainEvent);

            // Assert
            entity.DomainEvents.Should().HaveCount(1);
            entity.DomainEvents.First().Should().BeOfType<DummyEvent>();
            ((DummyEvent)entity.DomainEvents.First()).Data.Should().Be("test");
        }

        [Test]
        public void RemoveDomainEvent_ShouldRemoveEvent()
        {
            // Arrange
            var entity = new DummyEntity();
            var domainEvent1 = new DummyEvent("test1");
            var domainEvent2 = new DummyEvent("test2");
            entity.AddDomainEvent(domainEvent1);
            entity.AddDomainEvent(domainEvent2);

            // Act
            entity.RemoveDomainEvent(domainEvent1);

            // Assert
            entity.DomainEvents.Should().HaveCount(1);
            entity.DomainEvents.First().Should().Be(domainEvent2);
        }

        [Test]
        public void ClearDomainEvents_ShouldRemoveAll()
        {
            // Arrange
            var entity = new DummyEntity();
            entity.AddDomainEvent(new DummyEvent("test1"));
            entity.AddDomainEvent(new DummyEvent("test2"));

            // Act
            entity.ClearDomainEvents();

            // Assert
            entity.DomainEvents.Should().BeEmpty();
        }

        [Test]
        public void ITableEntityProperties_ShouldBeSetAndRetrieved()
        {
            // Arrange
            var entity = new DummyEntity
            {
                PartitionKey = "123",
                RowKey = "456",
                Timestamp = DateTimeOffset.Now,
                ETag = new ETag("some-etag")
            };

            // Act & Assert
            entity.PartitionKey.Should().Be("123");
            entity.RowKey.Should().Be("456");
            entity.Timestamp.Should().NotBeNull();
            entity.ETag.ToString().Should().Be("some-etag");
        }
    }
}
