using Pedido.Application.Common;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace Pedido.Tests.Application.Common.Exceptions;

public class ValidationExceptionTests
{
    [Fact(DisplayName = "Deve inicializar exceção com mensagem customizada")]
    public void Constructor_MessageOnly_ShouldAssignMessage()
    {
        // Criando a exceção apenas com a mensagem
        var exception = new ValidationException("Falha na validação");

        // Verifica se a mensagem foi atribuída corretamente
        exception.Message.Should().Be("Falha na validação");
    }

    [Fact(DisplayName = "Deve inicializar exceção com mensagem e inner exception")]
    public void Constructor_MessageAndInner_ShouldAssignBoth()
    {
        // Exceção interna simulada
        var inner = new Exception("Erro interno");

        // Criando a exceção com mensagem e inner exception
        var exception = new ValidationException("Falha na validação", inner);

        // Valida se a mensagem e inner exception estão corretos
        exception.Message.Should().Be("Falha na validação");
        exception.InnerException.Should().Be(inner);
    }

    [Fact(DisplayName = "Verifica comportamento com inner exception nula")]
    public void Constructor_NullInner_ShouldHandleGracefully()
    {
        // Criando exceção sem inner exception
        var exception = new ValidationException("Erro sem inner", null);

        // Confirma que a exceção foi criada e não possui inner
        exception.Message.Should().Be("Erro sem inner");
        exception.InnerException.Should().BeNull();
    }

    [Fact(DisplayName = "Verifica que múltiplas instâncias com a mesma mensagem são independentes")]
    public void MultipleInstances_SameMessage_ShouldBeIndependent()
    {
        var msg = "Mesma mensagem";

        var ex1 = new ValidationException(msg);
        var ex2 = new ValidationException(msg);

        // Devem ter a mesma mensagem
        ex1.Message.Should().Be(ex2.Message);

        // Mas são objetos diferentes
        ex1.Should().NotBeSameAs(ex2);
    }
}
