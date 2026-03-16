using System;
using System.Collections.Generic;
using System.Linq;

namespace Moroshka.Collections
{

/// <summary>
/// Describes a registry index for storing and searching items.
/// </summary>
public interface IRegistryIndex
{
	/// <summary>
	/// Registers items in the index.
	/// </summary>
	/// <param name="items">A set of items to register.</param>
	void Register(params IRegistryItem[] items);

	/// <summary>
	/// Builds internal structures for fast lookup.
	/// </summary>
	void Build();

	/// <summary>
	/// Returns all registered items.
	/// </summary>
	/// <returns>A sequence of all registry items.</returns>
	IEnumerable<IRegistryItem> All();

	/// <summary>
	/// Returns all items of the specified type.
	/// </summary>
	/// <param name="type">The item type to retrieve.</param>
	/// <returns>A sequence of items of the specified type.</returns>
	IEnumerable<IRegistryItem> All(Type type);

	/// <summary>
	/// Returns all items of the specified type.
	/// </summary>
	/// <typeparam name="T">The item type to retrieve.</typeparam>
	/// <returns>A sequence of items of the specified type.</returns>
	IEnumerable<T> All<T>() where T : class, IRegistryItem => All(typeof(T)).Cast<T>();

	/// <summary>
	/// Finds an item by type and name.
	/// </summary>
	/// <param name="type">The item type.</param>
	/// <param name="name">The item name.</param>
	/// <returns>The found item, or <see langword="null"/> if the item does not exist.</returns>
	IRegistryItem Find(Type type, string name);

	/// <summary>
	/// Finds an item of the specified type by name.
	/// </summary>
	/// <typeparam name="T">The item type.</typeparam>
	/// <param name="name">The item name.</param>
	/// <returns>The found item, or <see langword="null"/> if the item does not exist.</returns>
	T Find<T>(string name) where T : class, IRegistryItem => Find(typeof(T), name) as T;
}
}