using System;
using System.Collections.Generic;
using System.Linq;

namespace Moroshka.Collections
{

/// <summary>
/// Registry index implementation with lookup by type and name.
/// </summary>
public sealed class RegistryIndex : IRegistryIndex
{
	private readonly List<IRegistryItem> _items = new();
	private Dictionary<Type, List<IRegistryItem>> _lookupByType;
	private Dictionary<Type, Dictionary<string, IRegistryItem>> _lookupByTypeAndName;

	/// <inheritdoc />
	public void Register(params IRegistryItem[] items)
	{
		_items.AddRange(items);
	}

	/// <inheritdoc />
	public void Build()
	{
		try
		{
			var lookup = _items.ToLookup(x => x.GetType());
			_lookupByType = lookup.ToDictionary(x => x.Key, y => y.ToList());
			_lookupByTypeAndName = lookup.ToDictionary(x => x.Key, y => y.ToDictionary(x => x.Name));
		}
		catch (Exception exception)
		{
			this.ThrowBuildException(exception);
		}
	}

	/// <inheritdoc />
	public IEnumerable<IRegistryItem> All()
	{
		return _items;
	}

	/// <inheritdoc />
	public IEnumerable<IRegistryItem> All(Type type)
	{
		return _lookupByType.TryGetValue(type, out var resultList)
			? resultList
			: Enumerable.Empty<IRegistryItem>();
	}

	/// <inheritdoc />
	public IRegistryItem Find(Type type, string name)
	{
		return _lookupByTypeAndName.TryGetValue(type, out var lookupByName)
			? lookupByName.GetValueOrDefault(name)
			: null;
	}
}

}
