using System;

namespace Moroshka.Collections
{

[Serializable]
/// <summary>
/// Strongly typed reference to a registry item.
/// </summary>
/// <typeparam name="T">The registry item type.</typeparam>
public abstract class RegistryItemRef<T> : RegistryItemRef where T : class, IRegistryItem
{
	/// <summary>
	/// Cached value of the resolved reference.
	/// </summary>
	protected T Value;

	/// <summary>
	/// Initializes a strongly typed reference to a registry item.
	/// </summary>
	/// <param name="registryIndex">The registry index.</param>
	/// <param name="name">The item name in the registry.</param>
	protected RegistryItemRef(IRegistryIndex registryIndex, string name)
		: base(registryIndex, name)
	{
	}

	/// <summary>
	/// Resolves the reference and returns the registry item.
	/// </summary>
	/// <returns>An item of the specified type.</returns>
	public T Resolve()
	{
		return Value ??= RegistryIndex.Find<T>(Name);
	}

	/// <summary>
	/// Implicitly converts a reference to the registry item value.
	/// </summary>
	/// <param name="link">The item reference.</param>
	/// <returns>The resolved registry item.</returns>
	public static implicit operator T(RegistryItemRef<T> link)
	{
		return link.Resolve();
	}
}

}