using FluentAssertions;
using Pedido.Domain.Custumer.Entities;
using Pedido.Domain.Custumer.Exceptions;
using Pedido.Domain.Custumer.ValueObjects;
using Xunit;

public class ClienteTests
{
    [Fact]
    public void CriarCliente_Valido_DeveFuncionar()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cpf = "123.456.789-00";
        var nome = Name.Create("Amanda Cardoso");

        // Act
        var cliente = new Cliente(id, cpf, nome);

        // Assert
        cliente.Id.Should().Be(id);
        cliente.Cpf.Should().Be(cpf);
        cliente.Nome.Value.Should().Be("Amanda Cardoso");
    }

    [Fact]
    public void CriarCliente_ComNomeInvalido_DeveLancarExcecao()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cpf = "123.456.789-00";

        // Act & Assert
        Action act = () => new Cliente(id, cpf, Name.Create(""));
        act.Should().Throw<ClienteDomainException>()
           .WithMessage("O nome não pode ser vazio");
    }
}
