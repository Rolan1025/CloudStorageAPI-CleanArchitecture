using FluentAssertions;
using CloudStorageAPICleanArchitecture.Application.Common.Exceptions;
using NUnit.Framework;

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.Common.Exceptions
{
    [TestFixture]
    public class ForbiddenAccessExceptionTests
    {
        [Test]
        public void DefaultConstructor_ShouldCreateExceptionInstance()
        {
            // Act
            var exception = new ForbiddenAccessException();

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ForbiddenAccessException>();
            exception.InnerException.Should().BeNull();
            exception.Message.Should().NotBeNull("porque la propiedad Message de Exception nunca debe ser nula");
        }
    }
}
