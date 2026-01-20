using Pedido.Domain.Shared.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Pedido.Tests.Domain.Shared.ValueObjects;

public class ValueObjectTests
{
    // Classe de teste simples para ValueObject
    private class TestValueObject : ValueObject
    {
        public string Value1 { get; }
        public int Value2 { get; }

        public TestValueObject(string value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value1;
            yield return Value2;
        }
    }

    // Classe de teste para simular valores nulos
    private class TestValueObjectWithNull : ValueObject
    {
        public string Value1 { get; }
        public string? Value2 { get; }

        public TestValueObjectWithNull(string value1, string? value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value1;
            yield return Value2;
        }
    }

    [Fact(DisplayName = "ValueObject: Equals retorna true para objetos com valores iguais")]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        var obj1 = new TestValueObject("abc", 100);
        var obj2 = new TestValueObject("abc", 100);

        obj1.Equals(obj2).Should().BeTrue();
    }

    [Fact(DisplayName = "ValueObject: Equals retorna false para valores diferentes")]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        var obj1 = new TestValueObject("abc", 100);
        var obj2 = new TestValueObject("abc", 200);

        obj1.Equals(obj2).Should().BeFalse();
    }

    [Fact(DisplayName = "ValueObject: Equals retorna false para null")]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        var obj = new TestValueObject("abc", 100);
        obj.Equals(null).Should().BeFalse();
    }

    [Fact(DisplayName = "ValueObject: Equals retorna false para tipos diferentes")]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        var obj = new TestValueObject("abc", 100);
        var other = new object();

        obj.Equals(other).Should().BeFalse();
    }

    [Fact(DisplayName = "ValueObject: HashCode é consistente para valores iguais")]
    public void GetHashCode_WithSameValues_ShouldReturnSameHash()
    {
        var obj1 = new TestValueObject("abc", 100);
        var obj2 = new TestValueObject("abc", 100);

        obj1.GetHashCode().Should().Be(obj2.GetHashCode());
    }

    [Fact(DisplayName = "ValueObject: HashCode diferente para valores distintos")]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHash()
    {
        var obj1 = new TestValueObject("abc", 100);
        var obj2 = new TestValueObject("abc", 200);

        obj1.GetHashCode().Should().NotBe(obj2.GetHashCode());
    }

    [Fact(DisplayName = "ValueObject: operadores == e != funcionam corretamente")]
    public void Operators_ShouldBehaveCorrectly()
    {
        var a = new TestValueObject("x", 1);
        var b = new TestValueObject("x", 1);
        var c = new TestValueObject("y", 2);
        TestValueObject? n = null;

        // Igualdade e desigualdade
        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
        (a != c).Should().BeTrue();
        (n == null).Should().BeTrue();
        (n != a).Should().BeTrue();
    }

    [Fact(DisplayName = "ValueObject: GetHashCode trata valores nulos sem erro")]
    public void GetHashCode_WithNullValue_ShouldHandleNull()
    {
        var obj = new TestValueObjectWithNull("abc", null);

        // Apenas garante que não lança erro e retorna valor consistente
        var hash = obj.GetHashCode();
        hash.Should().NotBe(0);
    }

    [Fact(DisplayName = "ValueObject: Equals funciona corretamente com null em propriedades")]
    public void Equals_WithNullProperties_ShouldCompareCorrectly()
    {
        var obj1 = new TestValueObjectWithNull("abc", null);
        var obj2 = new TestValueObjectWithNull("abc", null);
        var obj3 = new TestValueObjectWithNull("abc", "teste");

        obj1.Equals(obj2).Should().BeTrue();
        obj1.Equals(obj3).Should().BeFalse();
    }

    [Fact(DisplayName = "ValueObject: Comparação de cadeia complexa")]
    public void ComplexComparison_ShouldBehaveCorrectly()
    {
        var list1 = new List<TestValueObject>
        {
            new TestValueObject("a", 1),
            new TestValueObject("b", 2)
        };
        var list2 = new List<TestValueObject>
        {
            new TestValueObject("a", 1),
            new TestValueObject("b", 2)
        };
        var list3 = new List<TestValueObject>
        {
            new TestValueObject("a", 1),
            new TestValueObject("b", 3)
        };

        // Comparações item a item simulando um ValueObject "composto"
        for (int i = 0; i < list1.Count; i++)
        {
            list1[i].Equals(list2[i]).Should().BeTrue();
            list1[i].Equals(list3[i]).Should().Be(i == 0); // só o primeiro igual
        }
    }
}
