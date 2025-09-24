namespace Moroshka.Collections
{

/// <summary>
/// Interface for a strategy to manage the capacity of collections.
/// </summary>
public interface ICapacityStrategy
{
	/// <summary>
	/// Calculates the new capacity based on the current capacity and the required size.
	/// </summary>
	/// <param name="currentCapacity">The current capacity of the collection.</param>
	/// <param name="requiredSize">The required size of the collection.</param>
	/// <returns>The new capacity of the collection.</returns>
	int CalculateCapacity(int currentCapacity, int requiredSize);
}

}