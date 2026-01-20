using Pedido.Domain.Shared.Entities;
using FluentAssertions;
using Xunit;

namespace Pedido.Tests.Domain.Shared.Entities;

public class EntityTests
{
    private class TestEntity : Entity
    {
        public TestEntity() : base() { }
    }

    [Fact]
    public void Constructor_ShouldInitializeIdAndCreatedAt()
    {
        // Criando uma nova entidade para verificar se ID e data de criação são atribuídos corretamente
        var entity = new TestEntity();

        // Valida se o ID não está vazio e se CreatedAt está próximo do horário de Brasília atual
        entity.Id.Should().NotBeEmpty();
        entity.CreatedAt.Should().BeCloseTo(Entity.GetBrasilDateTime(), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void SetUpdatedAt_ShouldSetUpdatedAtProperty()
    {
        // Inicializa entidade e captura valor atual de UpdatedAt
        var entity = new TestEntity();
        var initialUpdatedAt = entity.UpdatedAt;

        // Atualiza a propriedade UpdatedAt
        entity.SetUpdatedAt();

        // Confirma se UpdatedAt foi preenchido e está depois do valor inicial
        entity.UpdatedAt.Should().NotBeNull();
        entity.UpdatedAt.Should().BeAfter(initialUpdatedAt ?? DateTime.MinValue);
        entity.UpdatedAt.Should().BeCloseTo(Entity.GetBrasilDateTime(), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void GetBrasilDateTime_ShouldReturnCurrentDateTime()
    {
        // Obtém duas chamadas do método para garantir consistência
        var firstCall = Entity.GetBrasilDateTime();
        var secondCall = Entity.GetBrasilDateTime();

        // Confere se os dois valores estão próximos (considerando o offset UTC-3)
        firstCall.Should().BeCloseTo(secondCall, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Equals_WithSameId_ShouldReturnTrue()
    {
        // Criando duas entidades e forçando o mesmo ID para testar Equals
        var id = Guid.NewGuid();
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();
        typeof(Entity).GetProperty("Id")!.SetValue(entity1, id);
        typeof(Entity).GetProperty("Id")!.SetValue(entity2, id);

        // Verifica se Equals retorna true quando IDs são iguais
        var result = entity1.Equals(entity2);
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentId_ShouldReturnFalse()
    {
        // Entidades com IDs distintos devem ser diferentes
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        var result = entity1.Equals(entity2);
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Equals deve retornar false ao comparar com null
        var entity = new TestEntity();
        var result = entity.Equals((object?)null);

        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Equals não deve considerar objetos de tipos diferentes como iguais
        var entity = new TestEntity();
        var someObject = new object();

        var result = entity.Equals(someObject);
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameForSameId()
    {
        // Entidades com o mesmo ID devem ter o mesmo hash code
        var id = Guid.NewGuid();
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();
        typeof(Entity).GetProperty("Id")!.SetValue(entity1, id);
        typeof(Entity).GetProperty("Id")!.SetValue(entity2, id);

        var hash1 = entity1.GetHashCode();
        var hash2 = entity2.GetHashCode();

        hash1.Should().Be(hash2);
    }

    [Fact]
    public void OperatorEquals_WithSameId_ShouldReturnTrue()
    {
        // Testa o operador == para duas entidades com o mesmo ID
        var id = Guid.NewGuid();
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();
        typeof(Entity).GetProperty("Id")!.SetValue(entity1, id);
        typeof(Entity).GetProperty("Id")!.SetValue(entity2, id);

        var result = entity1 == entity2;
        result.Should().BeTrue();
    }

    [Fact]
    public void OperatorEquals_WithNullAndNull_ShouldReturnTrue()
    {
        // Comparando duas referências nulas, deve retornar true
        TestEntity? entity1 = null;
        TestEntity? entity2 = null;
        var result = entity1 == entity2;

        result.Should().BeTrue();
    }

    [Fact]
    public void OperatorNotEquals_WithDifferentEntities_ShouldReturnTrue()
    {
        // Operador != deve retornar true para entidades diferentes
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        var result = entity1 != entity2;
        result.Should().BeTrue();
    }
}
