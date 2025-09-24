using UnityEngine;

namespace Moroshka.Collections.Samples
{

/// <summary>
/// Simple example of using AssociationRegistry API.
/// Demonstrates basic operations: creating bindings, adding values, retrieving data.
/// </summary>
public class AssociationRegistrySample : MonoBehaviour
{
	private AssociationRegistry<string, string> _categoryRegistry;

	private void Start()
	{
		// Creating AssociationRegistry with capacity strategy
		var capacityStrategy = new CapacityStrategy();
		_categoryRegistry = new AssociationRegistry<string, string>(capacityStrategy, capacity: 5);

		Debug.Log("AssociationRegistry created for binding categories to values");

		// Creating bindings
		var fruitsBinding = _categoryRegistry.Bind("Fruits");
		fruitsBinding.To("Apple").To("Banana").To("Orange");
		Debug.Log($"Added {fruitsBinding.Count} elements to 'Fruits' category");

		var vegetablesBinding = _categoryRegistry.Bind("Vegetables");
		vegetablesBinding.To("Carrot").To("Tomato");
		Debug.Log($"Added {vegetablesBinding.Count} elements to 'Vegetables' category");

		// Retrieving binding
		var retrievedFruits = _categoryRegistry.GetBinding("Fruits");
		if (retrievedFruits != null)
		{
			Debug.Log($"Retrieved binding for 'Fruits': {retrievedFruits.Count} elements");
			foreach (var fruit in retrievedFruits.Values)
			{
				Debug.Log($"  - {fruit}");
			}
		}

		// Checking binding existence
		var nonExistent = _categoryRegistry.GetBinding("Meat");
		if (nonExistent == null)
		{
			Debug.Log("Binding for 'Meat' does not exist");
		}

		// Adding to existing binding
		var existingBinding = _categoryRegistry.Bind("Fruits"); // Get an existing one
		existingBinding.To("Grape");
		Debug.Log($"Added element to existing binding. New size: {existingBinding.Count}");

		// Unbinding category
		var unbound = _categoryRegistry.Unbind("Vegetables");
		Debug.Log($"Category 'Vegetables' unbound: {unbound}");

		// Check after unbinding
		var checkBinding = _categoryRegistry.GetBinding("Vegetables");
		Debug.Log($"After unbinding, binding for 'Vegetables': {(checkBinding == null ? "does not exist" : "exists")}");
	}

	private void OnDestroy()
	{
		// Releasing resources
		_categoryRegistry?.Dispose();
	}
}

}
