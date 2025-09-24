// ReSharper disable CollectionNeverQueried.Local
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;

namespace Moroshka.Collections.Benchmark;

[MemoryDiagnoser]
[RankColumn]
[SuppressMessage("Performance", "CA1822")]
public class BenchmarkFastList
{
	private const int ItemCount = 10000;

	[Benchmark]
	public void List_Add()
	{
		var list = new List<int>();
		for (var i = 0; i < ItemCount; i++)
		{
			list.Add(i);
		}
	}

	[Benchmark]
	public void FastList_Add()
	{
		using var fastList = new FastList<int>();
		for (var i = 0; i < ItemCount; i++)
		{
			fastList.Add(i);
		}
	}

	[Benchmark]
	public int List_Indexer_Get()
	{
		var list = new List<int>();
		for (var i = 0; i < ItemCount; i++)
		{
			list.Add(i);
		}

		var sum = 0;
		for (var i = 0; i < ItemCount; i++)
		{
			sum += list[i];
		}
		return sum;
	}

	[Benchmark]
	public int FastList_Indexer_Get()
	{
		using var fastList = new FastList<int>();
		for (var i = 0; i < ItemCount; i++)
		{
			fastList.Add(i);
		}

		var sum = 0;
		for (var i = 0; i < ItemCount; i++)
		{
			sum += fastList[i];
		}
		return sum;
	}

	[Benchmark]
	public void List_Indexer_Set()
	{
		var list = new List<int>(ItemCount);
		for (var i = 0; i < ItemCount; i++)
		{
			list.Add(0);
		}

		for (var i = 0; i < ItemCount; i++)
		{
			list[i] = i;
		}
	}

	[Benchmark]
	public void FastList_Indexer_Set()
	{
		using var fastList = new FastList<int>(ItemCount);
		for (var i = 0; i < ItemCount; i++)
		{
			fastList.Add(0);
		}

		for (var i = 0; i < ItemCount; i++)
		{
			fastList[i] = i;
		}
	}

	[Benchmark]
	public void List_Remove()
	{
		var list = new List<int>();
		for (var i = 0; i < ItemCount; i++)
		{
			list.Add(i);
		}

		for (var i = ItemCount - 1; i >= 0; i -= 2)
		{
			list.RemoveAt(i);
		}
	}

	[Benchmark]
	public void FastList_Remove()
	{
		using var fastList = new FastList<int>();
		for (var i = 0; i < ItemCount; i++)
		{
			fastList.Add(i);
		}

		for (var i = ItemCount - 1; i >= 0; i -= 2)
		{
			fastList.RemoveAt(i);
		}
	}

	[Benchmark]
	public void List_Clear()
	{
		var list = new List<int>();
		for (var i = 0; i < ItemCount; i++)
		{
			list.Add(i);
		}
		list.Clear();
	}

	[Benchmark]
	public void FastList_Clear()
	{
		using var fastList = new FastList<int>();
		for (var i = 0; i < ItemCount; i++)
		{
			fastList.Add(i);
		}
		fastList.Clear();
	}

	[Benchmark]
	public int List_Iteration()
	{
		var list = new List<int>();
		for (var i = 0; i < ItemCount; i++)
		{
			list.Add(i);
		}

		var sum = 0;
		foreach (var item in list)
		{
			sum += item;
		}
		return sum;
	}

	[Benchmark]
	public int FastList_Iteration()
	{
		using var fastList = new FastList<int>();
		for (var i = 0; i < ItemCount; i++)
		{
			fastList.Add(i);
		}

		var sum = 0;
		foreach (var item in fastList)
		{
			sum += item;
		}
		return sum;
	}
}
