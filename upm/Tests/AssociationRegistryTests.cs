using System;
using Moroshka.Protect;
using NUnit.Framework;
using Is = NUnit.Framework.Is;

namespace Moroshka.Collections.Tests
{

[TestFixture]
internal sealed class AssociationRegistryTests
{
	private ICapacityStrategy _capacityStrategy;

	[SetUp]
	public void SetUp()
	{
		_capacityStrategy = new CapacityStrategy();
	}

	[Test]
	public void Constructor_InitializesWithValidParameters()
	{
		// Arrange & Act
		IAssociationRegistry<string, string> registry = new AssociationRegistryTest<string, string>(_capacityStrategy, 10);

		// Assert
		Assert.That(registry, Is.Not.Null);
	}

	[Test]
	public void Constructor_ThrowsExceptionForNullCapacityStrategy()
	{
		// Act & Assert
		Assert.Throws<RequireException>(() => _ = new AssociationRegistryTest<string, string>(null, 10));
	}

	[Test]
	public void Bind_CreatesNewBindingForKey()
	{
		// Arrange
		const string key = "TestKey";
		IAssociationRegistry<string, string> registry = new AssociationRegistryTest<string, string>(_capacityStrategy, 10);

		// Act
		var binding = registry.Bind(key);

		// Assert
		Assert.That(binding, Is.Not.Null);
		Assert.That(binding.Key, Is.EqualTo(key));
	}

	[Test]
	public void Bind_ReturnsExistingBindingForKey()
	{
		// Arrange
		const string key = "TestKey";
		IAssociationRegistry<string, string> registry = new AssociationRegistryTest<string, string>(_capacityStrategy, 10);
		var firstBinding = registry.Bind(key);

		// Act
		var secondBinding = registry.Bind(key);

		// Assert
		Assert.That(secondBinding, Is.SameAs(firstBinding));
	}

	[Test]
	public void Unbind_RemovesBindingForKey()
	{
		// Arrange
		const string key = "TestKey";
		IAssociationRegistry<string, string> registry = new AssociationRegistryTest<string, string>(_capacityStrategy, 10);
		registry.Bind(key);

		// Act
		var result = registry.Unbind(key);

		// Assert
		Assert.That(result, Is.True);
		Assert.That(registry.GetBinding(key), Is.Null);
	}

	[Test]
	public void Unbind_ReturnsFalseForNonExistentKey()
	{
		// Arrange
		const string key = "NonExistentKey";
		IAssociationRegistry<string, string> registry = new AssociationRegistryTest<string, string>(_capacityStrategy, 10);

		// Act
		var result = registry.Unbind(key);

		// Assert
		Assert.That(result, Is.False);
	}

	[Test]
	public void GetBinding_ReturnsBindingForExistingKey()
	{
		// Arrange
		const string key = "TestKey";
		IAssociationRegistry<string, string> registry = new AssociationRegistryTest<string, string>(_capacityStrategy, 10);
		var binding = registry.Bind(key);

		// Act
		var result = registry.GetBinding(key);

		// Assert
		Assert.That(result, Is.SameAs(binding));
	}

	[Test]
	public void GetBinding_ReturnsNullForNonExistentKey()
	{
		// Arrange
		const string key = "NonExistentKey";
		IAssociationRegistry<string, string> registry = new AssociationRegistryTest<string, string>(_capacityStrategy, 10);

		// Act
		var result = registry.GetBinding(key);

		// Assert
		Assert.That(result, Is.Null);
	}

	[Test]
	public void Dispose_ReleasesAllResources()
	{
		// Arrange
		const string key = "TestKey";
		IAssociationRegistry<string, TestDisposable> registry =
			new AssociationRegistryTest<string, TestDisposable>(_capacityStrategy, 10);
		var disposableValue = new TestDisposable();
		var binding = registry.Bind(key);
		binding.To(disposableValue);

		// Act
		registry.Dispose();

		// Assert
		Assert.That(disposableValue.IsDisposed, Is.True);
		Assert.Throws<RequireException>(() => _ = registry.GetBinding(key));
	}

	[Test]
	public void UseAfterDispose_ThrowsException()
	{
		// Arrange
		IAssociationRegistry<string, string> registry = new AssociationRegistryTest<string, string>(_capacityStrategy, 10);
		registry.Dispose();

		// Act & Assert
		Assert.Throws<RequireException>(() => registry.Bind("TestKey"));
	}

	#region Nested

	private sealed class AssociationRegistryTest<TKey, TValue> : AssociationRegistry<TKey, TValue>
		where TKey : class
		where TValue : class
	{
		private readonly ICapacityStrategy _capacityStrategy;

		public AssociationRegistryTest(ICapacityStrategy capacityStrategy, int capacity) : base(capacityStrategy, capacity)
		{
			_capacityStrategy = capacityStrategy;
		}

		protected override IKeyAssociation<TKey, TValue> GetRawBinding(TKey key)
		{
			return new KeyAssociation<TKey, TValue>(_capacityStrategy, key);
		}
	}

	private sealed class TestDisposable : IDisposable
	{
		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			IsDisposed = true;
		}
	}

	#endregion

}

}
