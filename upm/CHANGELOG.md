# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-09-17

### Added

- High-Performance Collections:
  - Optimized for high performance with predictable memory management and efficient collision resolution.
  - Added `FastList<T>` as a high-performance list implementation using ArrayPool for efficient memory management.
  - Implements `IReadOnlyList<T>` and `IDisposable` interfaces with pooled array support.
- Key-Value Binding System:
  - Added `AssociationRegistry<TKey, TValue>` for managing key associations between keys and collections of values.
  - Added `KeyAssociation<TKey, TValue>` as implementation of key-to-value collection associations.
  - Includes supporting interfaces `IAssociationRegistry<TKey, TValue>` and `IKeyAssociation<TKey, TValue>`.
  - Provides fluent API for binding keys to multiple values with automatic cleanup.
- Capacity Management:
  - Added `CapacityStrategy` class implementing predefined capacity sizes close to (2^n - 1) for hash collision optimization.
  - Includes `ICapacityStrategy` interface for custom capacity management strategies.
  - Ensures efficient memory allocation and minimizes hash collisions through optimized capacity growth.
- Exception Handling:
  - Added `OutOfCapacityException` for handling cases when collection operations exceed available capacity.
  - Provides detailed error information including current and required capacity values.
