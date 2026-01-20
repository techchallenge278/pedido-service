using Pedido.Domain.Shared.Exceptions;
using FluentAssertions;
using Xunit;

namespace Pedido.Tests.Domain.Shared.Exceptions;

public class DomainExceptionTests
{
    // DomainException é abstrata, então criamos uma subclasse de teste
    private class TestDomainException : DomainException
    {
        public TestDomainException(string message) : base(message) { }
        public TestDomainException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    [Fact]
    public void DomainException_WithMessage_ShouldInitializeCorrectly()
    {
        // Mensagem simples para teste
        var message = "Erro de teste simples";

        // Criação da exceção
        var exception = new TestDomainException(message);

        // Confirma se a exceção não é nula e possui a mensagem correta
        exception.Should().NotBeNull();
        exception.Message.Should().Be(message);
        exception.InnerException.Should().BeNull(); // Não passou inner, então deve ser nulo
    }

    [Fact]
    public void DomainException_WithMessageAndInnerException_ShouldContainInner()
    {
        // Configura inner exception para teste
        var inner = new InvalidOperationException("Inner error");
        var message = "Erro com inner exception";

        // Criação da exceção com inner
        var exception = new TestDomainException(message, inner);

        // Validação da mensagem e da inner exception
        exception.Should().NotBeNull();
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(inner);
    }

    [Fact]
    public void DomainException_WithEmptyMessage_ShouldStillCreateException()
    {
        // Teste com mensagem vazia
        var exception = new TestDomainException("");

        // Deve criar a exceção, mas a mensagem será vazia
        exception.Should().NotBeNull();
        exception.Message.Should().Be("");
    }

    [Fact]
    public void DomainException_WithWhitespaceMessage_ShouldTrimOrKeep()
    {
        // Mensagem com espaços apenas
        var exception = new TestDomainException("   ");

        // Verifica se a exceção ainda é criada
        exception.Should().NotBeNull();
        exception.Message.Should().Be("   "); // Aqui não aplicamos trim, apenas mantém o input
    }

    [Fact]
    public void DomainException_InnerExceptionNull_ShouldNotFail()
    {
        // Criação de exceção passando null explicitamente como inner
        var exception = new TestDomainException("Mensagem teste", null!);

        // Deve aceitar sem lançar erro
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Mensagem teste");
        exception.InnerException.Should().BeNull();
    }

    [Fact]
    public void DomainException_MultipleInstances_WithSameMessage_ShouldHaveDifferentReferences()
    {
        var message = "Mesmo texto, instâncias diferentes";

        var ex1 = new TestDomainException(message);
        var ex2 = new TestDomainException(message);

        // Verifica que são instâncias diferentes, mesmo com mesma mensagem
        ex1.Should().NotBeSameAs(ex2);
        ex1.Message.Should().Be(ex2.Message);
    }

    [Fact]
    public void DomainException_ThrowAndCatch_ShouldPreserveType()
    {
        var message = "Erro para throw/catch";

        // Simula o throw/catch da exceção
        TestDomainException? caught = null;
        try
        {
            throw new TestDomainException(message);
        }
        catch (TestDomainException ex)
        {
            caught = ex;
        }

        // Verifica se foi capturada corretamente
        caught.Should().NotBeNull();
        caught!.Message.Should().Be(message);
    }

    [Fact]
    public void DomainException_WithNestedInnerExceptions_ShouldMaintainHierarchy()
    {
        // Inner mais profundo
        var deepest = new ArgumentNullException("param");
        var middle = new TestDomainException("Middle layer", deepest);
        var top = new TestDomainException("Top layer", middle);

        // Validação da cadeia
        top.InnerException.Should().Be(middle);
        top.InnerException!.InnerException.Should().Be(deepest);
        top.Message.Should().Be("Top layer");
    }

    [Fact]
    public void DomainException_ToString_ShouldIncludeMessage()
    {
        var message = "Mensagem para ToString";
        var exception = new TestDomainException(message);

        // ToString deve conter a mensagem original
        exception.ToString().Should().Contain(message);
    }
}
