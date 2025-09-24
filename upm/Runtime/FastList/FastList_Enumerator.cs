using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Moroshka.Collections
{

public sealed partial class FastList<T>
{
	/// <summary>
	/// Enumerates the elements of a <see cref="FastList{T}"/>.
	/// </summary>
	public struct Enumerator : IEnumerator<T>
	{
		private readonly FastList<T> _list;
		private int _index;

		/// <summary>
		/// Initializes a new instance of the <see cref="Enumerator"/> struct.
		/// </summary>
		/// <param name="list">The list to enumerate.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal Enumerator(FastList<T> list)
		{
			_list = list;
			_index = 0;
			Current = default;
		}

		/// <summary>
		/// Gets the element at the current position of the enumerator.
		/// </summary>
		public T Current { get; private set; }

		object IEnumerator.Current => Current;

		/// <summary>
		/// Advances the enumerator to the next element of the list.
		/// </summary>
		/// <returns><c>true</c> if the enumerator was successfully advanced; otherwise, <c>false</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool MoveNext()
		{
			if ((uint)_index >= (uint)_list.Count) return false;
			Current = _list._items[_index++];
			return true;
		}

		/// <summary>
		/// Resets the enumerator to its initial position.
		/// </summary>
		public void Reset() => _index = 0;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
		}
	}
}

}