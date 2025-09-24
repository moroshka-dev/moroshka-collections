using BenchmarkDotNet.Running;
using Moroshka.Collections.Benchmark;

BenchmarkRunner.Run<BenchmarkFastList>();

/*
| Method               | Mean      | Error    | StdDev   | Rank | Gen0   | Gen1   | Allocated |
|--------------------- |----------:|---------:|---------:|-----:|-------:|-------:|----------:|
| List_Add             |  10.57 us | 0.211 us | 0.449 us |    1 | 7.8125 | 1.5564 |  131400 B |
| FastList_Add         |  42.69 us | 0.555 us | 0.519 us |    7 |      - |      - |      64 B |
| List_Indexer_Get     |  15.47 us | 0.305 us | 0.484 us |    3 | 7.8125 | 1.5564 |  131400 B |
| FastList_Indexer_Get |  40.30 us | 0.526 us | 0.492 us |    6 |      - |      - |      64 B |
| List_Indexer_Set     |  12.63 us | 0.248 us | 0.331 us |    2 | 2.3804 | 0.2899 |   40056 B |
| FastList_Indexer_Set |  28.87 us | 0.527 us | 0.493 us |    4 |      - |      - |      64 B |
| List_Remove          | 577.79 us | 3.048 us | 2.702 us |    8 | 7.8125 | 0.9766 |  131400 B |
| FastList_Remove      |  43.27 us | 0.369 us | 0.328 us |    7 |      - |      - |      64 B |
| List_Clear           |  10.65 us | 0.211 us | 0.553 us |    1 | 7.8125 | 1.5564 |  131400 B |
| FastList_Clear       |  37.99 us | 0.484 us | 0.453 us |    5 |      - |      - |      64 B |
| List_Iteration       |  16.31 us | 0.384 us | 1.114 us |    3 | 7.8125 | 1.5564 |  131400 B |
| FastList_Iteration   |  42.40 us | 0.649 us | 0.607 us |    7 |      - |      - |      64 B |
*/
