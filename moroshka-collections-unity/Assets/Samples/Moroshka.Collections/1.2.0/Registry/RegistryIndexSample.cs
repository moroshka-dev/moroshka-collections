using UnityEngine;

namespace Moroshka.Collections.Samples
{

/// <summary>
/// Simple example of using RegistryIndex API.
/// Demonstrates registering items, building the index, and searching by type and name.
/// </summary>
public class RegistryIndexSample : MonoBehaviour
{
	private IRegistryIndex _registryIndex;

	private void Start()
	{
		_registryIndex = new RegistryIndex();

		// Registering items of different types.
		_registryIndex.Register(
			new WeaponItem("Sword", 10),
			new WeaponItem("Bow", 7),
			new PotionItem("Health", 25));

		// Building internal lookup structures.
		_registryIndex.Build();
		Debug.Log("Registry index is built and ready for queries.");

		// Query all items of a specific type.
		foreach (var weapon in _registryIndex.All<WeaponItem>())
		{
			Debug.Log($"Weapon: {weapon.Name}, Damage: {weapon.Damage}");
		}

		// Find item by type + name.
		var potion = _registryIndex.Find<PotionItem>("Health");
		if (potion != null)
		{
			Debug.Log($"Found potion: {potion.Name}, Restore: {potion.RestoreValue}");
		}

		// Resolve by typed reference.
		var swordRef = new WeaponItemRef(_registryIndex, "Sword");
		var sword = swordRef.Resolve();
		Debug.Log($"Resolved from ref: {sword.Name}, Damage: {sword.Damage}");
	}
}

internal sealed class WeaponItemRef : RegistryItemRef<WeaponItem>
{
	public WeaponItemRef(IRegistryIndex registryIndex, string name) : base(registryIndex, name)
	{
	}
}

internal sealed class WeaponItem : IRegistryItem
{
	public WeaponItem(string name, int damage)
	{
		Name = name;
		Damage = damage;
	}

	public string Name { get; set; }

	public int Damage { get; }
}

internal sealed class PotionItem : IRegistryItem
{
	public PotionItem(string name, int restoreValue)
	{
		Name = name;
		RestoreValue = restoreValue;
	}

	public string Name { get; set; }

	public int RestoreValue { get; }
}

}
