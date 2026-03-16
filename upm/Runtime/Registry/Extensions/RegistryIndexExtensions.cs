using System;
using System.Runtime.CompilerServices;

namespace Moroshka.Collections
{

/// <summary>
/// Provides helper methods for working with <see cref="RegistryIndex"/>.
/// </summary>
internal static class RegistryIndexExtensions
{
	/// <summary>
	/// Throws a <see cref="RegistryIndexBuildException"/> with caller context information.
	/// </summary>
	/// <param name="container">The registry index instance where the build error occurred.</param>
	/// <param name="innerException">The source exception that triggered the build failure.</param>
	/// <param name="member">The caller member name captured automatically.</param>
	/// <param name="line">The caller source line number captured automatically.</param>
	/// <exception cref="RegistryIndexBuildException">Always thrown.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void ThrowBuildException(
		this RegistryIndex container,
		Exception innerException,
		[CallerMemberName] string member = "",
		[CallerLineNumber] int line = 0)
	{
		throw new RegistryIndexBuildException(innerException)
		{
			Context = container.GetType().Name,
			Member = member,
			Line = line.ToString()
		};
	}
}

}
