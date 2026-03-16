using System;
using Moroshka.Xcp;

namespace Moroshka.Collections
{

/// <summary>
/// Represents an error that occurs when a <see cref="RegistryIndex"/> cannot be built
/// by calling <see cref="IRegistryIndex.Build()"/>.
/// </summary>
public sealed class RegistryIndexBuildException : DetailedException
{
	/// <summary>
	/// Initializes a new instance of the <see cref="RegistryIndexBuildException"/> class.
	/// </summary>
	/// <param name="innerException">The exception that caused the current exception.</param>
	public RegistryIndexBuildException(Exception innerException = null)
		: base("Failed to execute IRegistryIndex.Build().", innerException)
	{
	}
}

}
