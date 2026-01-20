using Pedido.Application.Commands;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Pedido.Tests.Application.Commands;

public class CreatePedidoCommandValidatorTests
{
    private readonly CreatePedidoCommandValidator _validator;

    public CreatePedidoCommandValidatorTests()
    {
        // Inicializa o validador que será usado em todos os testes
        _validator = new CreatePedidoCommandValidator();
    }

    [Fact(DisplayName = "Comando válido não deve gerar erros de validação")]
    public void Validate_ValidCommand_ShouldNotHaveErrors()
    {
        var command = new CreatePedidoCommand
        {
            ClienteId = Guid.NewGuid(),
            Items = new List<CreatePedidoItemCommand>
            {
                new() { ProdutoId = Guid.NewGuid(), Quant = 2 }
            }
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Comando com lista de itens vazia deve gerar erro")]
    public void Validate_EmptyItems_ShouldFailValidation()
    {
        var command = new CreatePedidoCommand
        {
            ClienteId = Guid.NewGuid(),
            Items = new List<CreatePedidoItemCommand>()
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Items);
    }

    [Fact(DisplayName = "Comando com lista de itens nula deve gerar erro")]
    public void Validate_NullItems_ShouldFailValidation()
    {
        var command = new CreatePedidoCommand
        {
            ClienteId = Guid.NewGuid(),
            Items = null!
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Items);
    }

    [Fact(DisplayName = "Item com ProdutoId vazio deve gerar erro")]
    public void Validate_ItemWithEmptyProdutoId_ShouldFail()
    {
        var command = new CreatePedidoCommand
        {
            ClienteId = Guid.NewGuid(),
            Items = new List<CreatePedidoItemCommand>
            {
                new() { ProdutoId = Guid.Empty, Quant = 1 }
            }
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor("Items[0].ProdutoId");
    }

    [Theory(DisplayName = "Itens com quantidade inválida devem gerar erro")]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_ItemWithInvalidQuantity_ShouldFail(int quantidade)
    {
        var command = new CreatePedidoCommand
        {
            ClienteId = Guid.NewGuid(),
            Items = new List<CreatePedidoItemCommand>
            {
                new() { ProdutoId = Guid.NewGuid(), Quant = quantidade }
            }
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor("Items[0].Quant");
    }

    [Fact(DisplayName = "ClienteId vazio deve gerar erro de validação")]
    public void Validate_EmptyClienteId_ShouldFail()
    {
        var command = new CreatePedidoCommand
        {
            ClienteId = Guid.Empty,
            Items = new List<CreatePedidoItemCommand>
            {
                new() { ProdutoId = Guid.NewGuid(), Quant = 1 }
            }
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ClienteId);
    }

    [Fact(DisplayName = "ClienteId nulo é permitido para pedidos anônimos")]
    public void Validate_NullClienteId_ShouldPass()
    {
        var command = new CreatePedidoCommand
        {
            ClienteId = null,
            Items = new List<CreatePedidoItemCommand>
            {
                new() { ProdutoId = Guid.NewGuid(), Quant = 1 }
            }
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.ClienteId);
    }

    [Fact(DisplayName = "Vários erros podem ser detectados ao mesmo tempo")]
    public void Validate_MultipleInvalidFields_ShouldDetectAllErrors()
    {
        var command = new CreatePedidoCommand
        {
            ClienteId = Guid.Empty,
            Items = new List<CreatePedidoItemCommand>
            {
                new() { ProdutoId = Guid.Empty, Quant = 0 }
            }
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ClienteId);
        result.ShouldHaveValidationErrorFor("Items[0].ProdutoId");
        result.ShouldHaveValidationErrorFor("Items[0].Quant");
    }
}
