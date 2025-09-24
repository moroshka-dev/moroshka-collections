using System;
using System.Collections.Generic;
using Moroshka.Protect;

namespace Moroshka.Collections
{

/// <summary>
/// Represents an association registry that manages key associations between keys and values.
/// Implements the <see cref="IAssociationRegistry{TKey, TValue}"/> interface.
/// </summary>
/// <typeparam name="TKey">The type of the key. Must be a reference type.</typeparam>
/// <typeparam name="TValue">The type of the value. Must be a reference type.</typeparam>
public abstract class AssociationRegistry<TKey, TValue> : IAssociationRegistry<TKey, TValue>
	where TKey : class
	where TValue : class
{
	private readonly Dictionary<TKey, IKeyAssociation<TKey, TValue>> _bindings;

	/// <summary>
	/// Initializes a new instance of the <see cref="AssociationRegistry{TKey, TValue}"/> class with the specified initial capacity and capacity strategy.
	/// </summary>
	/// <param name="capacity">The initial capacity of the internal dictionary.</param>
	/// <param name="capacityStrategy">The strategy for managing the capacity of the internal dictionary. Must not be null.</param>
	/// <exception cref="RequireException">Thrown if <paramref name="capacityStrategy"/> is null.</exception>
	protected AssociationRegistry(ICapacityStrategy capacityStrategy, int capacity)
	{
		this.Require(capacityStrategy,  nameof(capacityStrategy), Is.Not.Null);
		capacity = capacityStrategy.CalculateCapacity(0, capacity);
		_bindings = new Dictionary<TKey, IKeyAssociation<TKey, TValue>>(capacity);
	}

	protected bool IsDisposed { get; private set; }

	/// <summary>
	/// Creates or retrieves an existing key association for the specified key.
	/// </summary>
	/// <param name="key">The key to bind. Must not be null.</param>
	/// <returns>An <see cref="IKeyAssociation{TKey, TValue}"/> instance associated with the key.</returns>
	/// <exception cref="RequireException">Thrown if <paramref name="key"/> is null.</exception>
	/// <exception cref="RequireException">Thrown if `Dispose` has been called.</exception>
	public IKeyAssociation<TKey, TValue> Bind(TKey key)
	{
		this.Require(IsDisposed, nameof(IsDisposed),  Is.False);
		this.Require(key,  nameof(key), Is.Not.Null);
		if (_bindings.TryGetValue(key, out var binding)) return binding;
		binding = GetRawBinding(key);
		_bindings.Add(key, binding);
		return binding;
	}

	/// <summary>
	/// Unbinds the specified key and disposes of its associated key association.
	/// </summary>
	/// <param name="key">The key to unbind. Must not be null.</param>
	/// <returns><c>true</c> if the key was successfully unbound; otherwise, <c>false</c>.</returns>
	/// <exception cref="RequireException">Thrown if <paramref name="key"/> is null.</exception>
	/// <exception cref="RequireException">Thrown if `Dispose` has been called.</exception>
	public bool Unbind(TKey key)
	{
		this.Require(IsDisposed, nameof(IsDisposed),  Is.False);
		this.Require(key,  nameof(key), Is.Not.Null);
		if (!_bindings.Remove(key, out var binding)) return false;
		binding.Dispose();
		return true;
	}

	/// <summary>
	/// Retrieves the key association associated with the specified key.
	/// </summary>
	/// <param name="key">The key to retrieve the key association for. Must not be null.</param>
	/// <returns>The <see cref="IKeyAssociation{TKey, TValue}"/> instance associated with the key, or <c>null</c> if no key association exists.</returns>
	/// <exception cref="RequireException">Thrown if <paramref name="key"/> is null.</exception>
	/// /// <exception cref="RequireException">Thrown if `Dispose` has been called.</exception>
	public IKeyAssociation<TKey, TValue> GetBinding(TKey key)
	{
		this.Require(IsDisposed, nameof(IsDisposed),  Is.False);
		this.Require(key,  nameof(key), Is.Not.Null);
		return _bindings.GetValueOrDefault(key);
	}

	/// <summary>
	/// Disposes of all resources held by this association registry.
	/// </summary>
	public virtual void Dispose()
	{
		foreach (var binding in _bindings.Values) binding.Dispose();
		_bindings.Clear();
		IsDisposed = true;
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Creates a raw key association for the specified key.
	/// This method can be overridden in derived classes to provide custom key association implementations.
	/// </summary>
	/// <param name="key">The key to create a key association for. Must not be null.</param>
	/// <returns>A new <see cref="IKeyAssociation{TKey, TValue}"/> instance associated with the key.</returns>
	protected abstract IKeyAssociation<TKey, TValue> GetRawBinding(TKey key);
}

}
