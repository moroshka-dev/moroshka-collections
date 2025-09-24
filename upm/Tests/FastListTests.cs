using System;
using System.Collections.Generic;
using Moroshka.Protect;
using NUnit.Framework;
using Is = NUnit.Framework.Is;

namespace Moroshka.Collections.Tests
{

[TestFixture]
internal sealed class FastListTests
{
	private FastList<int> _list;

	[SetUp]
	public void SetUp()
	{
		_list = new FastList<int>(capacity: 4);
	}

	[TearDown]
	public void TearDown()
	{
		_list.Dispose();
	}

	[Test]
	public void Add_AddsElementsToList()
	{
		// Arrange
		const int expectedCount = 3;

		// Act
		_list.Add(10);
		_list.Add(20);
		_list.Add(30);

		// Assert
		Assert.That(_list.Count, Is.EqualTo(expectedCount));
		Assert.That(_list[0], Is.EqualTo(10));
		Assert.That(_list[1], Is.EqualTo(20));
		Assert.That(_list[2], Is.EqualTo(30));
	}

	[Test]
	public void Add_IncreasesCapacityWhenNeeded()
	{
		// Arrange
		const int elementsToAdd = 5;

		// Act
		for (var i = 0; i < elementsToAdd; i++) _list.Add(i);

		// Assert
		Assert.That(_list.Count, Is.EqualTo(elementsToAdd));
		Assert.That(_list.Capacity, Is.GreaterThanOrEqualTo(elementsToAdd));
	}

	[Test]
	public void RemoveAt_RemovesElementAtIndex()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);
		_list.Add(30);

		// Act
		var result = _list.RemoveAt(1);

		// Assert
		Assert.That(result, Is.True);
		Assert.That(_list.Count, Is.EqualTo(2));
		Assert.That(_list[0], Is.EqualTo(10));
		Assert.That(_list[1], Is.EqualTo(30));
	}

	[Test]
	public void RemoveAt_ReturnsFalseForInvalidIndex()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);

		// Act
		var result = _list.RemoveAt(5);

		// Assert
		Assert.That(result, Is.False);
		Assert.That(_list.Count, Is.EqualTo(2));
	}

	[Test]
	public void Indexer_AccessesElementsByIndex()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);

		// Act & Assert
		Assert.That(_list[0], Is.EqualTo(10));
		Assert.That(_list[1], Is.EqualTo(20));
	}

	[Test]
	public void Indexer_SetsValueAtIndex()
	{
		// Arrange
		_list.Add(10);

		// Act
		_list[0] = 20;

		// Assert
		Assert.That(_list[0], Is.EqualTo(20));
	}

	[Test]
	public void Clear_ClearsAllElements()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);

		// Act
		_list.Clear();

		// Assert
		Assert.That(_list.Count, Is.EqualTo(0));
	}

	[Test]
	public void GetEnumerator_EnumeratesElements()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);
		_list.Add(30);

		// Act
		var elements = new List<int>();
		foreach (var item in _list) elements.Add(item);

		// Assert
		Assert.That(elements, Is.EquivalentTo(new[] { 10, 20, 30 }));
	}

	[Test]
	public void Constructor_ThrowsExceptionForInvalidCapacity()
	{
		// Arrange & Act & Assert
		Assert.Throws<RequireException>(() => _ = new FastList<int>(capacity: 0));
	}

	[Test]
	public void Dispose_ReleasesResources()
	{
		// Arrange
		var list = new FastList<TestDisposable>();
		var disposableValue = new TestDisposable();
		list.Add(disposableValue);

		// Act
		list.Dispose();

		// Assert
		Assert.That(list.Count, Is.EqualTo(0));
		Assert.That(list.Capacity, Is.EqualTo(0));
		Assert.That(disposableValue.IsDisposed, Is.True);
	}

	[Test]
	public void IEnumerableT_GetEnumerator_ReturnsEnumerator()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);
		_list.Add(30);

		// Act
		IEnumerable<int> enumerable = _list;
		var enumerator = enumerable.GetEnumerator();

		// Assert
		Assert.That(enumerator, Is.Not.Null);
		Assert.That(enumerator.MoveNext(), Is.True);
		Assert.That(enumerator.Current, Is.EqualTo(10));
		Assert.That(enumerator.MoveNext(), Is.True);
		Assert.That(enumerator.Current, Is.EqualTo(20));
		Assert.That(enumerator.MoveNext(), Is.True);
		Assert.That(enumerator.Current, Is.EqualTo(30));
		Assert.That(enumerator.MoveNext(), Is.False);

		enumerator.Dispose();
	}

	[Test]
	public void Enumerator_Reset_ResetsToBeginning()
	{
		// Arrange
		_list.Add(10);
		_list.Add(20);
		_list.Add(30);

		var enumerator = _list.GetEnumerator();

		Assert.That(enumerator.MoveNext(), Is.True);
		Assert.That(enumerator.Current, Is.EqualTo(10));
		Assert.That(enumerator.MoveNext(), Is.True);
		Assert.That(enumerator.Current, Is.EqualTo(20));

		enumerator.Reset();

		// Assert
		Assert.That(enumerator.MoveNext(), Is.True);
		Assert.That(enumerator.Current, Is.EqualTo(10));
		Assert.That(enumerator.MoveNext(), Is.True);
		Assert.That(enumerator.Current, Is.EqualTo(20));
		Assert.That(enumerator.MoveNext(), Is.True);
		Assert.That(enumerator.Current, Is.EqualTo(30));
		Assert.That(enumerator.MoveNext(), Is.False);
		enumerator.Dispose();
	}

	private sealed class TestDisposable : IDisposable
	{
		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			IsDisposed = true;
		}
	}
}

}
