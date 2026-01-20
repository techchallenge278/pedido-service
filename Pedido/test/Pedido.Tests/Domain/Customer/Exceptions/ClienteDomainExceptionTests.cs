using Pedido.Domain.Custumer.Exceptions;
using FluentAssertions;
using Xunit;

namespace Pedido.Tests.Domain.Customers.Exceptions;

public class ClienteDomainExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessageProperty()
    {
        // Arrange: mensagem de erro de exemplo
        var message = "Test error message";

        // Act: criando a exceção
        var exception = new ClienteDomainException(message);

        // Assert: verifica se a mensagem foi atribuída corretamente
        exception.Should().NotBeNull();
        exception.Message.Should().Be(message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetBothProperties()
    {
        // Arrange
        var message = "Test error message";
        var innerException = new InvalidOperationException("Inner exception");

        // Act
        var exception = new ClienteDomainException(message, innerException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void Constructor_WithEmptyMessage_ShouldStillCreateException()
    {
        // Arrange
        var emptyMessage = string.Empty;

        // Act
        var exception = new ClienteDomainException(emptyMessage);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(emptyMessage);
    }
}
