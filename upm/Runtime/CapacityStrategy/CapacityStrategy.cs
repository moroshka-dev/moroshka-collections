namespace Moroshka.Collections
{

/// <summary>
/// A strategy for managing the capacity of collections.
/// </summary>
/// <remarks>
/// This class provides a method to calculate the capacity based on its current value and the required size.
/// The new capacity is selected from a predefined array that contains numbers close to powers of two minus one (2^n - 1).
/// These values ensure efficient memory allocation and minimize hash collisions.
/// </remarks>
public sealed class CapacityStrategy : ICapacityStrategy
{
	private static readonly int[] CapacitySizes =
	{
		3,
		7,
		15,
		31,
		63,
		127,
		255,
		511,
		1_023,
		2_047,
		4_095,
		8_191,
		16_383,
		32_767,
		65_535,
		131_071,
		262_143,
		524_287,
		1_048_575
	};

	/// <summary>
	/// Calculates the new capacity based on its current value and the required size.
	/// </summary>
	/// <param name="currentCapacity">The current capacity of the collection.</param>
	/// <param name="requiredSize">The required size of the collection.</param>
	/// <returns>The new capacity of the collection.</returns>
	/// <exception cref="OutOfCapacityException">Thrown if the required size exceeds the maximum value in the capacity array.</exception>
	public int CalculateCapacity(int currentCapacity, int requiredSize)
	{
		if (currentCapacity >= requiredSize) return currentCapacity;

		foreach (var capacity in CapacitySizes)
		{
			if (capacity >= requiredSize) return capacity;
		}

		throw new OutOfCapacityException
		{
			CurrentCapacity = currentCapacity.ToString(),
			RequiredSize = requiredSize.ToString()
		};
	}
}

}
