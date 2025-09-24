using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Moroshka.Protect;
using Moroshka.Xcp;

namespace Moroshka.Collections
{

/// <summary>
/// A high-performance list implementation that uses pooled arrays for efficient memory management.
/// Implements <see cref="IReadOnlyList{T}"/> and <see cref="IDisposable"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public sealed partial class FastList<T> : IReadOnlyList<T>, IDisposable
{
	private static readonly ArrayPool<T> ArrayPool = ArrayPool<T>.Shared;
	private readonly ICapacityStrategy _capacityStrategy;
	private T[] _items;

	/// <summary>
	/// Initializes a new instance of the <see cref="FastList{T}"/> class with the specified initial capacity and capacity strategy.
	/// </summary>
	/// <param name="capacity">The initial capacity of the list. Must be greater than or equal to 1.</param>
	/// <param name="capacityStrategy">The strategy for managing the capacity of the list. If null, a default <see cref="CapacityStrategy"/> is used.</param>
	/// <exception cref="ArgOutOfRangeException"> Thrown if <paramref name="capacity"/> is less than 1 or greater than <see cref="int.MaxValue"/>. </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FastList(int capacity = 1, ICapacityStrategy capacityStrategy = null)
	{
		this.Require(capacity, nameof(capacity), Is.InRange(1, int.MaxValue));
		_capacityStrategy = capacityStrategy ?? new CapacityStrategy();
		_items = ArrayPool.Rent(capacity);
		Capacity = capacity;
		Count = 0;
	}

	/// <summary>
	/// Gets the number of elements contained in the list.
	/// </summary>
	public int Count { get; private set; }

	/// <summary>
	/// Gets the current capacity of the internal array.
	/// </summary>
	public int Capacity { get; private set; }

	/// <summary>
	/// Adds an element to the end of the list.
	/// </summary>
	/// <param name="item">The element to add.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Add(T item)
	{
		var capacity = _capacityStrategy.CalculateCapacity(Capacity, Count + 1);
		if (Count == Capacity) Resize(capacity);
		_items[Count++] = item;
	}

	/// <summary>
	/// Removes the element at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to remove.</param>
	/// <returns><c>true</c> if the element was successfully removed; otherwise, <c>false</c>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool RemoveAt(int index)
	{
		if ((uint)index >= (uint)Count) return false;
		Count--;
		_items[index] = _items[Count];
		_items[Count] = default;
		return true;
	}

	/// <summary>
	/// Gets or sets the element at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get or set.</param>
	/// <returns>The element at the specified index.</returns>
	public T this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _items[index];
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => _items[index] = value;
	}

	/// <summary>
	/// Clears all elements from the list.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Clear()
	{
		Array.Clear(_items, 0, Count);
		Count = 0;
	}

	/// <summary>
	/// Releases the resources used by the list.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Dispose()
	{
		foreach (var item in _items) Dispose(item);
		ArrayPool.Return(_items, clearArray: true);
		_items = Array.Empty<T>();
		Capacity = 0;
		Count = 0;
	}

	/// <summary>
	/// Returns an enumerator that iterates through the list.
	/// </summary>
	/// <returns>An enumerator for the list.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Enumerator GetEnumerator() => new(this);

	/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
	IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

	/// <inheritdoc cref="IEnumerable.GetEnumerator"/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void Dispose(T value)
	{
		if (value is IDisposable disposable) disposable.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Resize(int newSize)
	{
		var newArray = ArrayPool.Rent(newSize);
		Array.Copy(_items, newArray, Count);
		ArrayPool.Return(_items, clearArray: true);
		_items = newArray;
		Capacity = newSize;
	}
}

}
