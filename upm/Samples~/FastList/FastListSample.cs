using UnityEngine;

namespace Moroshka.Collections.Samples
{

/// <summary>
/// Simple example of using FastList API.
/// Demonstrates basic operations: creation, adding, removing, iteration and clearing.
/// </summary>
public class FastListSample : MonoBehaviour
{
	private FastList<string> _stringList;

	private void Start()
	{
		// Creating FastList with initial capacity
		_stringList = new FastList<string>(capacity: 3);
		Debug.Log($"FastList created with capacity: {_stringList.Capacity}");

		// Adding elements
		_stringList.Add("First element");
		_stringList.Add("Second element");
		_stringList.Add("Third element");
		Debug.Log($"Elements added: {_stringList.Count}");

		// Access by index
		Debug.Log($"First element: {_stringList[0]}");
		Debug.Log($"Last element: {_stringList[^1]}");

		// Modifying element
		_stringList[1] = "Modified element";
		Debug.Log($"Modified element: {_stringList[1]}");

		// Iterating over a list
		for (var i = 0; i < _stringList.Count; i++)
		{
			Debug.Log($"[{i}]: {_stringList[i]}");
		}

		// Iteration with foreach
		foreach (var item in _stringList)
		{
			Debug.Log($"Element: {item}");
		}

		// Removing element by index
		var removed = _stringList.RemoveAt(1);
		Debug.Log($"Element removed: {removed}, current size: {_stringList.Count}");

		// Clearing list
		_stringList.Clear();
		Debug.Log($"List cleared, size: {_stringList.Count}");
	}

	private void OnDestroy()
	{
		// Releasing resources
		_stringList?.Dispose();
	}
}

}
