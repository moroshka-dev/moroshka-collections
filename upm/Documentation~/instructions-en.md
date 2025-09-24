# Usage Instructions

## Table of Contents

- [Overview](#overview)
- [Main Collections](#main-collections)
  - [FastList](#fastlist)
  - [AssociationRegistry](#associationregistry)
- [Capacity Management](#capacity-management)
- [Error Handling](#error-handling)
- [Performance](#performance)
- [Unity Integration](#unity-integration)
- [Conclusion](#conclusion)

## Overview

The `Moroshka.Collections` library provides high-performance collections optimized for game applications and other scenarios where predictable performance and memory management are crucial.

## Main Collections

### FastList

`FastList<T>` is a high-performance alternative to the standard `List<T>` that uses array pooling for efficient memory management.

#### When to use FastList

- When you need high performance for adding elements
- When working with temporary collections that are frequently created and destroyed
- In game loops where predictable performance is important

#### Usage Examples

```csharp
// Creating a list with initial capacity
var fastList = new FastList<int>(capacity: 100);

// Adding elements
fastList.Add(1);
fastList.Add(2);
fastList.Add(3);

// Accessing elements
int first = fastList[0]; // 1
int count = fastList.Count; // 3

// Iterating through elements
foreach (var item in fastList)
{
    Console.WriteLine(item);
}

// Don't forget to release resources
fastList.Dispose();
```

#### Best Practices

- Always call `Dispose()` after use
- Use `using` for automatic resource disposal
- Set a reasonable initial capacity to avoid memory reallocation

```csharp
using (var fastList = new FastList<GameObject>(capacity: 50))
{
    // Working with the list
    foreach (var obj in gameObjects)
    {
        if (obj.IsActive)
            fastList.Add(obj);
    }

    // fastList will be automatically disposed
}
```

### AssociationRegistry

`AssociationRegistry<TKey, TValue>` is a specialized collection that manages associations between keys and collections of values. It provides a fluent API for creating and managing key-value associations.

#### When to use AssociationRegistry

- When you need to group multiple values under a single key
- For creating category-based collections (e.g., grouping items by type)
- When you need a fluent API for building associations
- For managing hierarchical data structures

#### Usage Examples

```csharp
// Creating a registry with capacity strategy
var capacityStrategy = new CapacityStrategy();
var categoryRegistry = new AssociationRegistry<string, string>(capacityStrategy, capacity: 10);

// Creating bindings with fluent API
var fruitsBinding = categoryRegistry.Bind("Fruits");
fruitsBinding.To("Apple").To("Banana").To("Orange");

var vegetablesBinding = categoryRegistry.Bind("Vegetables");
vegetablesBinding.To("Carrot").To("Tomato");

// Retrieving binding
var retrievedFruits = categoryRegistry.GetBinding("Fruits");
if (retrievedFruits != null)
{
    Console.WriteLine($"Fruits category has {retrievedFruits.Count} items:");
    foreach (var fruit in retrievedFruits.Values)
    {
        Console.WriteLine($"  - {fruit}");
    }
}

// Adding to existing binding
var existingBinding = categoryRegistry.Bind("Fruits");
existingBinding.To("Grape");

// Unbinding category
bool unbound = categoryRegistry.Unbind("Vegetables");
Console.WriteLine($"Vegetables unbound: {unbound}");

// Don't forget to dispose
categoryRegistry.Dispose();
```

#### Best Practices

- Always call `Dispose()` after use to free resources
- Use `using` statements for automatic resource management
- Check for null when retrieving bindings with `GetBinding`
- Use the fluent API for building complex associations

```csharp
// Game item categorization system
using (var itemRegistry = new AssociationRegistry<string, Item>(new CapacityStrategy(), 50))
{
    // Categorize weapons
    var weapons = itemRegistry.Bind("Weapons");
    weapons.To(sword).To(bow).To(staff);

    // Categorize armor
    var armor = itemRegistry.Bind("Armor");
    armor.To(helmet).To(chestplate).To(boots);

    // Get all weapons for a player
    var playerWeapons = itemRegistry.GetBinding("Weapons");
    if (playerWeapons != null)
    {
        foreach (var weapon in playerWeapons.Values)
        {
            weapon.Equip(player);
        }
    }
}
```

## Capacity Management

### CapacityStrategy

By default, all collections use `CapacityStrategy` for optimal management of internal array sizes. The strategy selects sizes close to powers of two minus one (2^n - 1), which minimizes hash collisions.

#### Custom Capacity Strategy

```csharp
public class CustomCapacityStrategy : ICapacityStrategy
{
    public int CalculateCapacity(int currentCapacity, int requiredSize)
    {
        // Your capacity calculation logic
        return Math.Max(currentCapacity * 2, requiredSize);
    }
}

// Using custom strategy
var customList = new FastList<int>(capacity: 10, capacityStrategy: new CustomCapacityStrategy());
```

## Error Handling

### OutOfCapacityException

When the maximum capacity is exceeded, an `OutOfCapacityException` is thrown:

```csharp
try
{
    var largeList = new FastList<int>(capacity: 1);
    // Attempting to add too many elements
}
catch (OutOfCapacityException ex)
{
    Console.WriteLine($"Required capacity: {ex.RequiredSize}");
    Console.WriteLine($"Current capacity: {ex.CurrentCapacity}");
}
```

## Performance

### Performance Recommendations

1. **Initialize with correct capacity**: Set initial capacity close to the expected collection size
2. **Reuse collections**: Create collections once and reuse them
3. **Use using statements**: Always release resources with `Dispose()` or `using`
4. **Choose the right collection**: Use `AssociationRegistry` for key-value associations, `FastList` for sequential data

### Performance Comparison

```csharp
// Slow - frequent reallocations
var slowList = new FastList<int>(capacity: 1);
for (int i = 0; i < 10000; i++)
{
    slowList.Add(i);
}

// Fast - proper initial capacity
var fastList = new FastList<int>(capacity: 10000);
for (int i = 0; i < 10000; i++)
{
    fastList.Add(i);
}
```

## Unity Integration

### Using in MonoBehaviour

```csharp
public class GameManager : MonoBehaviour
{
    private FastList<Enemy> _enemies;
    private AssociationRegistry<string, GameObject> _categoryRegistry;

    private void Awake()
    {
        _enemies = new FastList<Enemy>(capacity: 100);
        _categoryRegistry = new AssociationRegistry<string, GameObject>(new CapacityStrategy(), capacity: 20);
    }

    private void OnDestroy()
    {
        _enemies?.Dispose();
        _categoryRegistry?.Dispose();
    }

    public void SpawnEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void CategorizeObject(string category, GameObject obj)
    {
        var binding = _categoryRegistry.Bind(category);
        binding.To(obj);
    }
}
```

## Conclusion

The `Moroshka.Collections` library provides optimized collections for high-performance applications. Proper use of these collections will help improve your application's performance and ensure predictable memory management.
