using Pedido.Application.Common;
using FluentAssertions;

namespace Pedido.Tests.Application.Common.Exceptions;

public class NotFoundExceptionTests
{
    [Fact(DisplayName = "Deve criar exceção apenas com mensagem personalizada")]
    public void Constructor_MessageOnly_ShouldSetMessageProperly()
    {
        var exception = new NotFoundException("Recurso não encontrado");
        exception.Message.Should().Be("Recurso não encontrado");
        exception.InnerException.Should().BeNull();
    }

    [Fact(DisplayName = "Deve criar exceção com mensagem e inner exception")]
    public void Constructor_MessageAndInner_ShouldHandleBoth()
    {
        var inner = new Exception("Erro interno");
        var exception = new NotFoundException("Recurso não encontrado", inner);

        exception.Message.Should().Be("Recurso não encontrado");
        exception.InnerException.Should().Be(inner);
    }

    [Fact(DisplayName = "Verifica comportamento com inner exception nula")]
    public void Constructor_WithNullInner_ShouldWorkCorrectly()
    {
        var exception = new NotFoundException("Item não localizado", null);

        exception.Message.Should().Be("Item não localizado");
        exception.InnerException.Should().BeNull();
    }

    [Fact(DisplayName = "Duas instâncias com a mesma mensagem devem ser independentes")]
    public void MultipleInstances_SameMessage_ShouldNotBeSameObject()
    {
        var msg = "Elemento ausente";
        var ex1 = new NotFoundException(msg);
        var ex2 = new NotFoundException(msg);

        ex1.Message.Should().Be(ex2.Message);
        ex1.Should().NotBeSameAs(ex2);
    }
}
