using System;

namespace Moroshka.Collections
{

[Serializable]
/// <summary>
/// Base reference to a registry item by name.
/// </summary>
public abstract class RegistryItemRef
{
	/// <summary>
	/// Registry index used to resolve references.
	/// </summary>
	protected readonly IRegistryIndex RegistryIndex;

	/// <summary>
	/// Initializes a reference to a registry item.
	/// </summary>
	/// <param name="registryIndex">The registry index.</param>
	/// <param name="name">The item name in the registry.</param>
	protected RegistryItemRef(IRegistryIndex registryIndex, string name)
	{
		RegistryIndex = registryIndex;
		Name = name;
	}

	/// <summary>
	/// Item name in the registry.
	/// </summary>
	public string Name { get; }
}

}