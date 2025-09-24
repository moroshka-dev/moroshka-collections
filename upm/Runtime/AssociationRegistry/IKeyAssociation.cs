using System;
using System.Collections.Generic;

namespace Moroshka.Collections
{

/// <summary>
/// Represents a key association between a key and a collection of values.
/// </summary>
/// <typeparam name="TKey">The type of the key. Must be a reference type.</typeparam>
/// <typeparam name="TValue">The type of the values. Must be a reference type.</typeparam>
public interface IKeyAssociation<out TKey, TValue> : IDisposable
	where TKey : class
	where TValue : class
{
	/// <summary>
	/// Gets the key associated with this key association.
	/// </summary>
	TKey Key { get; }

	/// <summary>
	/// Gets the collection of values associated with this key association.
	/// </summary>
	IReadOnlyList<TValue> Values { get; }

	/// <summary>
	/// Gets the number of values in this key association.
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Adds a value to this key association.
	/// </summary>
	/// <param name="value">The value to add. Must not be null.</param>
	/// <returns>The current key association instance with the added value.</returns>
	IKeyAssociation<TKey, TValue> To(TValue value);

	/// <summary>
	/// Clears all values from this key association.
	/// </summary>
	void ClearValues();
}

}
