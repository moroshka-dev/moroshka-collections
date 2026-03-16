namespace Moroshka.Collections
{

/// <summary>
/// Represents an item that can be registered in the registry.
/// </summary>
public interface IRegistryItem
{
	/// <summary>
	/// Unique item name within its type scope.
	/// </summary>
	string Name { get; set; }
}

}
