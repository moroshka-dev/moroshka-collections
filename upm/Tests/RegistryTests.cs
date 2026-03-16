using System;
using System.Collections.Generic;
using System.Linq;
using Moroshka.Xcp;
using NUnit.Framework;
using Is = NUnit.Framework.Is;

namespace Moroshka.Collections.Tests
{

[TestFixture]
internal sealed class RegistryIndexTests
{
	private RegistryIndex _registryIndex;

	[SetUp]
	public void SetUp()
	{
		_registryIndex = new RegistryIndex();
	}

	[Test]
	public void All_ReturnsAllRegisteredItems()
	{
		// Arrange
		var sword = new TestRegistryItem("Sword");
		var potion = new AnotherTestRegistryItem("Potion");
		_registryIndex.Register(sword, potion);
		_registryIndex.Build();

		// Act
		var allItems = _registryIndex.All().ToArray();

		// Assert
		Assert.That(allItems, Has.Length.EqualTo(2));
		Assert.That(allItems, Contains.Item(sword));
		Assert.That(allItems, Contains.Item(potion));
	}

	[Test]
	public void AllByType_ReturnsOnlyRequestedType()
	{
		// Arrange
		var sword = new TestRegistryItem("Sword");
		var shield = new TestRegistryItem("Shield");
		var potion = new AnotherTestRegistryItem("Potion");
		_registryIndex.Register(sword, shield, potion);
		_registryIndex.Build();

		// Act
		var typedItems = _registryIndex.All(typeof(TestRegistryItem)).ToArray();

		// Assert
		Assert.That(typedItems, Is.EquivalentTo(new IRegistryItem[] { sword, shield }));
	}

	[Test]
	public void AllByType_ReturnsEmptyWhenTypeIsNotRegistered()
	{
		// Arrange
		_registryIndex.Register(new TestRegistryItem("Sword"));
		_registryIndex.Build();

		// Act
		var typedItems = _registryIndex.All(typeof(AnotherTestRegistryItem)).ToArray();

		// Assert
		Assert.That(typedItems, Is.Empty);
	}

	[Test]
	public void Find_ReturnsItemByTypeAndName()
	{
		// Arrange
		var sword = new TestRegistryItem("Sword");
		_registryIndex.Register(sword, new TestRegistryItem("Shield"));
		_registryIndex.Build();

		// Act
		var found = _registryIndex.Find(typeof(TestRegistryItem), "Sword");

		// Assert
		Assert.That(found, Is.SameAs(sword));
	}

	[Test]
	public void Find_ReturnsNullWhenItemIsMissing()
	{
		// Arrange
		_registryIndex.Register(new TestRegistryItem("Sword"));
		_registryIndex.Build();

		// Act
		var found = _registryIndex.Find(typeof(TestRegistryItem), "Bow");

		// Assert
		Assert.That(found, Is.Null);
	}

	[Test]
	public void InterfaceGenericMethods_WorkForAllAndFind()
	{
		// Arrange
		IRegistryIndex index = _registryIndex;
		var sword = new TestRegistryItem("Sword");
		var potion = new AnotherTestRegistryItem("Potion");
		index.Register(sword, potion);
		index.Build();

		// Act
		var allTyped = index.All<TestRegistryItem>().ToArray();
		var found = index.Find<TestRegistryItem>("Sword");

		// Assert
		Assert.That(allTyped, Is.EquivalentTo(new[] { sword }));
		Assert.That(found, Is.SameAs(sword));
	}

	[Test]
	public void InterfaceGenericFind_ReturnsNullWhenItemIsMissing()
	{
		// Arrange
		IRegistryIndex index = _registryIndex;
		index.Register(new TestRegistryItem("Sword"));
		index.Build();

		// Act
		var found = index.Find<TestRegistryItem>("Missing");

		// Assert
		Assert.That(found, Is.Null);
	}

	[Test]
	public void Build_ThrowsWhenOneTypeContainsMultipleDuplicateNames()
	{
		// Arrange
		_registryIndex.Register(
			new TestRegistryItem("Sword"),
			new TestRegistryItem("Sword"),
			new TestRegistryItem("Sword"));

		// Act & Assert
		Assert.Throws<RegistryIndexBuildException>(() => _registryIndex.Build());
	}

	[Test]
	public void Build_ThrowsDetailedExceptionWithContext_WhenDuplicateNamesDetected()
	{
		// Arrange
		_registryIndex.Register(
			new TestRegistryItem("Sword"),
			new TestRegistryItem("Sword"));

		// Act
		var exception = Assert.Throws<RegistryIndexBuildException>(() => _registryIndex.Build());

		// Assert
		Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
		Assert.That(exception.Context, Is.EqualTo(nameof(RegistryIndex)));
		Assert.That(exception.Member, Is.EqualTo("Build"));
		Assert.That(int.TryParse(exception.Line, out var line), Is.True);
		Assert.That(line, Is.GreaterThan(0));
	}

	[Test]
	public void Build_AllowsSameNameForDifferentTypes()
	{
		// Arrange
		var weapon = new TestRegistryItem("Shared");
		var potion = new AnotherTestRegistryItem("Shared");
		_registryIndex.Register(weapon, potion);

		// Act
		_registryIndex.Build();
		var foundWeapon = _registryIndex.Find(typeof(TestRegistryItem), "Shared");
		var foundPotion = _registryIndex.Find(typeof(AnotherTestRegistryItem), "Shared");

		// Assert
		Assert.That(foundWeapon, Is.SameAs(weapon));
		Assert.That(foundPotion, Is.SameAs(potion));
	}

	[Test]
	public void Build_WithoutItems_KeepsIndexQueryable()
	{
		// Act
		_registryIndex.Build();

		// Assert
		Assert.That(_registryIndex.All(), Is.Empty);
		Assert.That(_registryIndex.All(typeof(TestRegistryItem)), Is.Empty);
		Assert.That(_registryIndex.Find(typeof(TestRegistryItem), "Missing"), Is.Null);
	}
}

[TestFixture]
internal sealed class RegistryIndexBuildExceptionTests
{
	[Test]
	public void Constructor_WithoutInnerException_UsesDefaultMessage()
	{
		// Act
		var exception = new RegistryIndexBuildException();

		// Assert
	Assert.That(exception.Message, Is.EqualTo("Failed to execute IRegistryIndex.Build()."));
		Assert.That(exception.InnerException, Is.Null);
	}

	[Test]
	public void Constructor_WithInnerException_SetsInnerException()
	{
		// Arrange
		var innerException = new InvalidOperationException("duplicate key");

		// Act
		var exception = new RegistryIndexBuildException(innerException);

		// Assert
	Assert.That(exception.Message, Is.EqualTo("Failed to execute IRegistryIndex.Build()."));
		Assert.That(exception.InnerException, Is.SameAs(innerException));
	}

	[Test]
	public void InheritsFromDetailedException()
	{
		// Act
		var exception = new RegistryIndexBuildException();

		// Assert
		Assert.That(exception, Is.InstanceOf<DetailedException>());
	}
}

[TestFixture]
internal sealed class RegistryItemRefTests
{
	[Test]
	public void Name_ReturnsValueFromConstructor()
	{
		// Arrange
		var index = new CountingRegistryIndex();
		var itemRef = new TestRegistryItemRef(index, "Sword");

		// Act & Assert
		Assert.That(itemRef.Name, Is.EqualTo("Sword"));
	}

	[Test]
	public void Resolve_ReturnsItemAndCachesLookup()
	{
		// Arrange
		var item = new TestRegistryItem("Sword");
		var index = new CountingRegistryIndex(item);
		var itemRef = new TestRegistryItemRef(index, "Sword");

		// Act
		var first = itemRef.Resolve();
		var second = itemRef.Resolve();

		// Assert
		Assert.That(first, Is.SameAs(item));
		Assert.That(second, Is.SameAs(item));
		Assert.That(index.FindCallCount, Is.EqualTo(1));
	}

	[Test]
	public void ImplicitConversion_ResolvesReference()
	{
		// Arrange
		var item = new TestRegistryItem("Sword");
		var index = new CountingRegistryIndex(item);
		var itemRef = new TestRegistryItemRef(index, "Sword");

		// Act
		TestRegistryItem value = itemRef;

		// Assert
		Assert.That(value, Is.SameAs(item));
		Assert.That(index.FindCallCount, Is.EqualTo(1));
	}

	[Test]
	public void Resolve_ReturnsNullWhenRegistryDoesNotContainItem()
	{
		// Arrange
		var index = new CountingRegistryIndex();
		var itemRef = new TestRegistryItemRef(index, "Missing");

		// Act
		var value = itemRef.Resolve();

		// Assert
		Assert.That(value, Is.Null);
		Assert.That(index.FindCallCount, Is.EqualTo(1));
	}

	[Test]
	public void Resolve_WhenItemMissing_DoesNotCacheNullResult()
	{
		// Arrange
		var index = new CountingRegistryIndex();
		var itemRef = new TestRegistryItemRef(index, "Missing");

		// Act
		var first = itemRef.Resolve();
		var second = itemRef.Resolve();

		// Assert
		Assert.That(first, Is.Null);
		Assert.That(second, Is.Null);
		Assert.That(index.FindCallCount, Is.EqualTo(2));
	}
}

internal sealed class TestRegistryItemRef : RegistryItemRef<TestRegistryItem>
{
	public TestRegistryItemRef(IRegistryIndex registryIndex, string name) : base(registryIndex, name)
	{
	}
}

internal sealed class CountingRegistryIndex : IRegistryIndex
{
	private readonly Dictionary<(Type type, string name), IRegistryItem> _items = new();

	public CountingRegistryIndex(params IRegistryItem[] items)
	{
		foreach (var item in items) _items[(item.GetType(), item.Name)] = item;
	}

	public int FindCallCount { get; private set; }

	public void Register(params IRegistryItem[] items)
	{
		foreach (var item in items) _items[(item.GetType(), item.Name)] = item;
	}

	public void Build()
	{
	}

	public IEnumerable<IRegistryItem> All()
	{
		return _items.Values;
	}

	public IEnumerable<IRegistryItem> All(Type type)
	{
		return _items
			.Where(pair => pair.Key.type == type)
			.Select(pair => pair.Value);
	}

	public IRegistryItem Find(Type type, string name)
	{
		FindCallCount++;
		return _items.GetValueOrDefault((type, name));
	}
}

internal sealed class TestRegistryItem : IRegistryItem
{
	public TestRegistryItem(string name)
	{
		Name = name;
	}

	public string Name { get; set; }
}

internal sealed class AnotherTestRegistryItem : IRegistryItem
{
	public AnotherTestRegistryItem(string name)
	{
		Name = name;
	}

	public string Name { get; set; }
}

}
