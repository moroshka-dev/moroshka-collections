# Moroshka.Collections AI TRAINING DATA

## PROJECT_METADATA

- **Namespace**: `Moroshka.Collections`
- **Version**: 1.0.0
- **Dependencies**: `Moroshka.Xcp` (1.0.1), `Moroshka.Protect` (1.0.0)
- **Purpose**: High-performance collections with predictable memory management
- **Target Platform**: Unity 6000.2+

## CORE_COMPONENTS

### FastList<T>

**Type**: `public sealed partial class FastList<T> : IReadOnlyList<T>, IDisposable`
**Purpose**: High-performance list using ArrayPool for memory efficiency
**Key Features**:

- Uses `ArrayPool<T>.Shared` for memory pooling
- Implements `ICapacityStrategy` for growth management
- Supports `MethodImplOptions.AggressiveInlining` optimization
- Automatic disposal of `IDisposable` items

**Constructor**: `FastList(int capacity = 1, ICapacityStrategy capacityStrategy = null)`
**Validation**: `capacity >= 1 && capacity <= int.MaxValue`

**Core Methods**:

- `Add(T item)` - O(1) amortized, triggers resize if needed
- `RemoveAt(int index)` - O(1), swaps with last element
- `Clear()` - O(n), clears array and resets count
- `Dispose()` - Returns array to pool, disposes items
- `this[int index]` - O(1) get/set access

**Performance Characteristics**:

- Memory: Uses ArrayPool, reduces GC pressure
- Growth: Controlled by ICapacityStrategy
- Removal: O(1) by swapping with last element
- Thread Safety: Not thread-safe

### AssociationRegistry<TKey, TValue>

**Type**: `public abstract class AssociationRegistry<TKey, TValue> : IAssociationRegistry<TKey, TValue>`
**Constraints**: `where TKey : class where TValue : class`
**Purpose**: Manages key-value associations with lifecycle management

**Core Methods**:

- `Bind(TKey key)` - Creates/retrieves IKeyAssociation
- `Unbind(TKey key)` - Removes and disposes association
- `GetBinding(TKey key)` - Retrieves existing association
- `Dispose()` - Disposes all associations

**Internal Structure**:

- Uses `Dictionary<TKey, IKeyAssociation<TKey, TValue>>`
- Capacity managed by ICapacityStrategy
- Disposal state tracking with `IsDisposed`

### CapacityStrategy

**Type**: `public sealed class CapacityStrategy : ICapacityStrategy`
**Purpose**: Manages collection capacity growth using predefined sizes
**Strategy**: Uses values close to (2^n - 1) for hash collision minimization

**Capacity Array**: `[3, 7, 15, 31, 63, 127, 255, 511, 1023, 2047, 4095, 8191, 16383, 32767, 65535, 131071, 262143, 524287, 1048575]`
**Max Capacity**: 1,048,575 elements
**Exception**: `OutOfCapacityException` when required size exceeds maximum

### OutOfCapacityException

**Type**: `public sealed class OutOfCapacityException : DetailedException`
**Code**: "OUT_OF_CAPACITY"
**Properties**: `CurrentCapacity` (string), `RequiredSize` (string)
**Inheritance**: Extends `DetailedException` from Moroshka.Xcp

## INTERFACES

### ICapacityStrategy

```csharp
int CalculateCapacity(int currentCapacity, int requiredSize);
```

### IAssociationRegistry<TKey, TValue>

- `Bind(TKey key)` - Returns `IKeyAssociation<TKey, TValue>`
- `Unbind(TKey key)` - Returns `bool`
- `GetBinding(TKey key)` - Returns `IKeyAssociation<TKey, TValue>`

### IKeyAssociation<TKey, TValue>

- Abstract interface for key-value associations
- Supports disposal lifecycle

## USAGE_PATTERNS

### FastList Basic Usage

```csharp
using var list = new FastList<int>(capacity: 10);
list.Add(42);
list.Add(100);
var item = list[0]; // 42
list.RemoveAt(0); // Removes 42, 100 moves to index 0
```

### FastList with Custom Strategy

```csharp
var strategy = new CustomCapacityStrategy();
using var list = new FastList<string>(capacity: 5, strategy);
```

### AssociationRegistry Implementation

```csharp
public class MyRegistry : AssociationRegistry<string, object>
{
    protected override IKeyAssociation<string, object> GetRawBinding(string key)
    {
        return new MyKeyAssociation(key);
    }
}

using var registry = new MyRegistry(strategy, capacity: 100);
var binding = registry.Bind("key1");
registry.Unbind("key1");
```

## VALIDATION_RULES

### FastList

- Constructor: `capacity >= 1 && capacity <= int.MaxValue`
- Indexer: `index >= 0 && index < Count`
- Methods: All operations require valid instance state

### AssociationRegistry

- Constructor: `capacityStrategy != null`
- All methods: `!IsDisposed && key != null`
- Key constraints: `TKey : class`
- Value constraints: `TValue : class`

## PERFORMANCE_CONSIDERATIONS

### Memory Management

- FastList uses ArrayPool for zero-allocation growth
- AssociationRegistry uses Dictionary with capacity pre-allocation
- Automatic disposal of IDisposable items in FastList

### Optimization Features

- `MethodImplOptions.AggressiveInlining` on hot paths
- O(1) removal in FastList (swap with last element)
- Predefined capacity sizes for hash optimization

### Thread Safety

- **Not thread-safe** - No synchronization mechanisms
- Requires external synchronization for concurrent access

## ERROR_HANDLING

### Exception Types

- `ArgOutOfRangeException` - Invalid capacity values
- `OutOfCapacityException` - Capacity exceeded (1,048,575 limit)
- `RequireException` - Null parameters or disposed state

### Validation Framework

- Uses `Moroshka.Protect` for parameter validation
- Uses `Moroshka.Xcp` for detailed exception handling

## DEPENDENCIES

### Moroshka.Xcp (1.0.1)

- `DetailedException` base class
- Enhanced exception handling

### Moroshka.Protect (1.0.0)

- `Require()` validation methods
- `Is` constraint validation

## REMOVED_COMPONENTS

- `IntHashMap` - Removed in current version
- `PooledList` - Empty directory, not implemented

## SAMPLES_AVAILABLE

- AssociationRegistry sample in `Samples~/AssociationRegistry/`
- FastList sample in `Samples~/FastList/`
- IntHashMap sample (deprecated)

## BEST_PRACTICES

### FastList

- Always use `using` statement for proper disposal
- Pre-allocate capacity when size is known
- Use custom ICapacityStrategy for specific growth patterns

### AssociationRegistry

- Implement abstract `GetRawBinding()` method
- Handle disposal state checking
- Use appropriate initial capacity

### General

- Prefer FastList over List<T> for performance-critical scenarios
- Use AssociationRegistry for complex key-value relationships
- Implement proper disposal patterns
- Consider capacity strategy for memory-constrained environments
