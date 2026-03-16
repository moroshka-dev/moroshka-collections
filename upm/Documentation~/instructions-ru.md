# Инструкции по использованию

## Содержание

- [Обзор](#обзор)
- [Основные коллекции](#основные-коллекции)
  - [FastList](#fastlist)
  - [AssociationRegistry](#associationregistry)
  - [RegistryIndex](#registryindex)
  - [RegistryItemRef](#registryitemref)
- [Управление емкостью](#управление-емкостью)
- [Обработка ошибок](#обработка-ошибок)
- [Производительность](#производительность)
- [Интеграция с Unity](#интеграция-с-unity)
- [Заключение](#заключение)

## Обзор

Библиотека `Moroshka.Collections` предоставляет высокопроизводительные коллекции, оптимизированные для игровых приложений и других сценариев, где важна предсказуемая производительность и управление памятью.

## Основные коллекции

### FastList

`FastList<T>` - это высокопроизводительная альтернатива стандартному `List<T>`, которая использует пулинг массивов для эффективного управления памятью.

#### Когда использовать FastList

- Когда нужна высокая производительность добавления элементов
- При работе с временными коллекциями, которые часто создаются и уничтожаются
- В игровых циклах, где важна предсказуемая производительность

#### Примеры использования

```csharp
// Создание списка с начальной емкостью
var fastList = new FastList<int>(capacity: 100);

// Добавление элементов
fastList.Add(1);
fastList.Add(2);
fastList.Add(3);

// Доступ к элементам
int first = fastList[0]; // 1
int count = fastList.Count; // 3

// Итерация по элементам
foreach (var item in fastList)
{
    Console.WriteLine(item);
}

// Не забудьте освободить ресурсы
fastList.Dispose();
```

#### Лучшие практики

- Всегда вызывайте `Dispose()` после использования
- Используйте `using` для автоматического освобождения ресурсов
- Устанавливайте разумную начальную емкость для избежания перераспределения памяти

```csharp
using (var fastList = new FastList<GameObject>(capacity: 50))
{
    // Работа со списком
    foreach (var obj in gameObjects)
    {
        if (obj.IsActive)
            fastList.Add(obj);
    }

    // fastList автоматически освободится
}
```

### AssociationRegistry

`AssociationRegistry<TKey, TValue>` - специализированная коллекция, которая управляет ассоциациями между ключами и коллекциями значений. Предоставляет fluent API для создания и управления ассоциациями ключ-значение.

#### Когда использовать AssociationRegistry

- Когда нужно сгруппировать несколько значений под одним ключом
- Для создания коллекций на основе категорий (например, группировка предметов по типу)
- Когда нужен fluent API для построения ассоциаций
- Для управления иерархическими структурами данных

#### Примеры использования

```csharp
// Создание реестра со стратегией емкости
var capacityStrategy = new CapacityStrategy();
var categoryRegistry = new AssociationRegistry<string, string>(capacityStrategy, capacity: 10);

// Создание привязок с fluent API
var fruitsBinding = categoryRegistry.Bind("Fruits");
fruitsBinding.To("Apple").To("Banana").To("Orange");

var vegetablesBinding = categoryRegistry.Bind("Vegetables");
vegetablesBinding.To("Carrot").To("Tomato");

// Получение привязки
var retrievedFruits = categoryRegistry.GetBinding("Fruits");
if (retrievedFruits != null)
{
    Console.WriteLine($"Категория Fruits содержит {retrievedFruits.Count} элементов:");
    foreach (var fruit in retrievedFruits.Values)
    {
        Console.WriteLine($"  - {fruit}");
    }
}

// Добавление к существующей привязке
var existingBinding = categoryRegistry.Bind("Fruits");
existingBinding.To("Grape");

// Отвязка категории
bool unbound = categoryRegistry.Unbind("Vegetables");
Console.WriteLine($"Vegetables отвязана: {unbound}");

// Не забудьте освободить ресурсы
categoryRegistry.Dispose();
```

#### Лучшие практики

- Всегда вызывайте `Dispose()` после использования для освобождения ресурсов
- Используйте `using` для автоматического управления ресурсами
- Проверяйте на null при получении привязок с помощью `GetBinding`
- Используйте fluent API для построения сложных ассоциаций

```csharp
// Система категоризации игровых предметов
using (var itemRegistry = new AssociationRegistry<string, Item>(new CapacityStrategy(), 50))
{
    // Категоризация оружия
    var weapons = itemRegistry.Bind("Weapons");
    weapons.To(sword).To(bow).To(staff);

    // Категоризация брони
    var armor = itemRegistry.Bind("Armor");
    armor.To(helmet).To(chestplate).To(boots);

    // Получение всего оружия для игрока
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

### RegistryIndex

`RegistryIndex` - индекс реестра для поиска зарегистрированных элементов по типу и имени. Индекс строится в два этапа: сначала регистрация через `Register(...)`, затем построение таблиц поиска через `Build()`.

#### Когда использовать RegistryIndex

- Когда нужны быстрые выборки по типу (`All(typeof(T))` / `All<T>()`)
- Когда нужен точечный поиск по имени в рамках типа (`Find(...)` / `Find<T>(...)`)
- Когда допустимы одинаковые имена у разных типов, но не внутри одного типа

#### Базовый пример использования

```csharp
public sealed class WeaponDefinition : IRegistryItem
{
    public string Name { get; set; }
    public int Damage { get; set; }
}

public sealed class PotionDefinition : IRegistryItem
{
    public string Name { get; set; }
    public int Heal { get; set; }
}

var index = new RegistryIndex();
index.Register(
    new WeaponDefinition { Name = "Sword", Damage = 10 },
    new WeaponDefinition { Name = "Bow", Damage = 7 },
    new PotionDefinition { Name = "Sword", Heal = 25 } // одно имя с другим типом допустимо
);

index.Build();

var allItems = index.All();
var weapons = index.All<WeaponDefinition>();
var sword = index.Find<WeaponDefinition>("Sword");
var missing = index.Find<WeaponDefinition>("Axe"); // null
```

#### Важные детали

- До вызова `Build()` типизированные выборки и поиск не готовы к использованию
- Дубликаты `Name` внутри одного типа приводят к `RegistryIndexBuildException`
- Метод `All()` возвращает все зарегистрированные элементы без фильтрации по типу

### RegistryItemRef

`RegistryItemRef` и `RegistryItemRef<T>` - это ссылки на элемент реестра по имени. Они удобны, когда нужно хранить "лёгкую" ссылку и разрешать её в объект только при первом доступе.

#### Пример использования

```csharp
public sealed class WeaponRef : RegistryItemRef<WeaponDefinition>
{
    public WeaponRef(IRegistryIndex index, string name) : base(index, name)
    {
    }
}

var weaponRef = new WeaponRef(index, "Sword");
var weaponA = weaponRef.Resolve(); // первый поиск через index.Find<T>(...)
var weaponB = weaponRef.Resolve(); // возвращается кэшированное значение

WeaponDefinition weaponC = weaponRef; // неявная конверсия через Resolve()
```

## Управление емкостью

### CapacityStrategy

По умолчанию все коллекции используют `CapacityStrategy` для оптимального управления размером внутренних массивов. Стратегия выбирает размеры, близкие к степеням двойки минус один (2^n - 1), что минимизирует коллизии хеширования.

#### Кастомная стратегия емкости

```csharp
public class CustomCapacityStrategy : ICapacityStrategy
{
    public int CalculateCapacity(int currentCapacity, int requiredSize)
    {
        // Ваша логика расчета емкости
        return Math.Max(currentCapacity * 2, requiredSize);
    }
}

// Использование кастомной стратегии
var customList = new FastList<int>(capacity: 10, capacityStrategy: new CustomCapacityStrategy());
```

## Обработка ошибок

### OutOfCapacityException

При превышении максимальной емкости коллекции выбрасывается `OutOfCapacityException`:

```csharp
try
{
    var largeList = new FastList<int>(capacity: 1);
    // Попытка добавить слишком много элементов
}
catch (OutOfCapacityException ex)
{
    Console.WriteLine($"Требуемая емкость: {ex.RequiredSize}");
    Console.WriteLine($"Текущая емкость: {ex.CurrentCapacity}");
}
```

### RegistryIndexBuildException

При ошибке построения индекса (обычно из-за повторяющихся имён внутри одного типа) выбрасывается `RegistryIndexBuildException`.

```csharp
try
{
    var index = new RegistryIndex();
    index.Register(
        new WeaponDefinition { Name = "Sword" },
        new WeaponDefinition { Name = "Sword" } // дубликат в рамках одного типа
    );
    index.Build();
}
catch (RegistryIndexBuildException ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.InnerException?.Message);
}
```

## Производительность

### Рекомендации по производительности

1. **Инициализация с правильной емкостью**: Устанавливайте начальную емкость близко к ожидаемому размеру коллекции
2. **Переиспользование коллекций**: Создавайте коллекции один раз и переиспользуйте их
3. **Использование using**: Всегда освобождайте ресурсы с помощью `Dispose()` или `using`
4. **Выбор правильной коллекции**: Используйте `AssociationRegistry` для ассоциаций ключ-значение, `FastList` для последовательных данных

### Сравнение производительности

```csharp
// Медленно - частые перераспределения
var slowList = new FastList<int>(capacity: 1);
for (int i = 0; i < 10000; i++)
{
    slowList.Add(i);
}

// Быстро - правильная начальная емкость
var fastList = new FastList<int>(capacity: 10000);
for (int i = 0; i < 10000; i++)
{
    fastList.Add(i);
}
```

## Интеграция с Unity

### Использование в MonoBehaviour

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

## Заключение

Библиотека `Moroshka.Collections` предоставляет оптимизированные коллекции для высокопроизводительных приложений. Правильное использование этих коллекций поможет улучшить производительность вашего приложения и обеспечить предсказуемое управление памятью.
