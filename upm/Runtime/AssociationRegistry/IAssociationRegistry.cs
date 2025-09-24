using System;

namespace Moroshka.Collections
{

/// <summary>
/// Represents an association registry that manages key associations between keys and values.
/// </summary>
/// <typeparam name="TKey">The type of the key. Must be a reference type.</typeparam>
/// <typeparam name="TValue">The type of the value. Must be a reference type.</typeparam>
public interface IAssociationRegistry<TKey, TValue> : IDisposable
	where TKey : class
	where TValue : class
{
	/// <summary>
	/// Creates or retrieves an existing key association for the specified key.
	/// </summary>
	/// <param name="key">The key to bind. Must not be null.</param>
	/// <returns>An <see cref="IKeyAssociation{TKey, TValue}"/> instance associated with the key.</returns>
	IKeyAssociation<TKey, TValue> Bind(TKey key);

	/// <summary>
	/// Unbinds the specified key and disposes of its associated key association.
	/// </summary>
	/// <param name="key">The key to unbind. Must not be null.</param>
	/// <returns><c>true</c> if the key was successfully unbound; otherwise, <c>false</c>.</returns>
	bool Unbind(TKey key);

	/// <summary>
	/// Retrieves the key association associated with the specified key.
	/// </summary>
	/// <param name="key">The key to retrieve the key association for. Must not be null.</param>
	/// <returns>The <see cref="IKeyAssociation{TKey, TValue}"/> instance associated with the key, or <c>null</c> if no key association exists.</returns>
	IKeyAssociation<TKey, TValue> GetBinding(TKey key);
}

}
