using System;
using Moroshka.Xcp;

namespace Moroshka.Collections
{

/// <summary>
/// Represents the exception thrown when a collection operation exceeds the available capacity.
/// </summary>
public sealed class OutOfCapacityException : DetailedException
{
	/// <summary>
	/// Initializes a new instance of the <see cref="OutOfCapacityException"/> class with a specified error message and optional inner exception.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">The exception that is the cause of the current exception or null if no inner exception is specified.</param>
	public OutOfCapacityException(string message, Exception innerException = null)
		: base(message, innerException)
	{
		Code = "OUT_OF_CAPACITY";
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="OutOfCapacityException"/> class with a default error message and optional inner exception.
	/// </summary>
	/// <param name="innerException">The exception that is the cause of the current exception or null if no inner exception is specified.</param>
	public OutOfCapacityException(Exception innerException = null)
		: this("Required capacity is too large", innerException)
	{
	}

	/// <summary>
	/// Gets or sets the current capacity of the collection when the exception was thrown.
	/// </summary>
	/// <value>The current capacity value as a string.</value>
	public string CurrentCapacity
	{
		get => (string)Data[nameof(CurrentCapacity)];
		set => Data[nameof(CurrentCapacity)] = value;
	}

	/// <summary>
	/// Gets or sets the required size that caused the capacity exception.
	/// </summary>
	/// <value>The required size value as a string.</value>
	public string RequiredSize
	{
		get => (string)Data[nameof(RequiredSize)];
		set => Data[nameof(RequiredSize)] = value;
	}
}

}
