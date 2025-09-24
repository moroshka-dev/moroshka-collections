using System;
using System.Collections.Generic;
using Moroshka.Protect;

namespace Moroshka.Collections
{

/// <summary>
/// Represents a key association between a key and a collection of values.
/// Implements the <see cref="IKeyAssociation{TKey, TValue}"/> interface.
/// </summary>
/// <typeparam name="TKey">The type of the key. Must be a reference type.</typeparam>
/// <typeparam name="TValue">The type of the values. Must be a reference type.</typeparam>
public class KeyAssociation<TKey, TValue> : IKeyAssociation<TKey, TValue>
	where TKey : class
	where TValue : class
{
	private readonly FastList<TValue> _values;
	protected bool IsDisposed;

	/// <summary>
	/// Initializes a new instance of the <see cref="KeyAssociation{TKey, TValue}"/> class with the specified capacity strategy and key.
	/// </summary>
	/// <param name="capacityStrategy">The strategy for managing the capacity of the internal list. Must not be null.</param>
	/// <param name="key">The key associated with this binding. Must not be null.</param>
	/// <exception cref="RequireException">Thrown if <paramref name="capacityStrategy"/> or <paramref name="key"/> is null.</exception>
	public KeyAssociation(ICapacityStrategy capacityStrategy, TKey key)
	{
		this.Require(capacityStrategy, nameof(capacityStrategy), Is.Not.Null);
		this.Require(key, nameof(key), Is.Not.Null);
		_values = new FastList<TValue>(1, capacityStrategy);
		Key = key;
	}

	/// <summary>
	/// Gets the key associated with this key association.
	/// </summary>
	public TKey Key { get; }

	/// <summary>
	/// Gets the collection of values associated with this key association.
	/// </summary>
	public IReadOnlyList<TValue> Values => _values;

	/// <summary>
	/// Gets the number of values in this key association.
	/// </summary>
	public int Count => _values.Count;

	/// <summary>
	/// Adds a value to this key association.
	/// </summary>
	/// <param name="value">The value to add. Must not be null.</param>
	/// <returns>The current key association instance with the added value.</returns>
	/// <exception cref="RequireException">Thrown if <paramref name="value"/> is null.</exception>
	/// <exception cref="RequireException">Thrown if `Dispose` has been called.</exception>
	public IKeyAssociation<TKey, TValue> To(TValue value)
	{
		this.Require(IsDisposed, nameof(IsDisposed),  Is.False);
		this.Require(value, nameof(value), Is.Not.Null);
		_values.Add(value);
		return this;
	}

	/// <summary>
	/// Clears all values from this key association.
	/// </summary>
	public void ClearValues()
	{
		this.Require(IsDisposed, nameof(IsDisposed),  Is.False);
		_values.Clear();
	}

	/// <summary>
	/// Disposes of all resources held by this key association.
	/// </summary>
	public void Dispose()
	{
		_values.Dispose();
		IsDisposed = true;
		GC.SuppressFinalize(this);
	}
}

}
