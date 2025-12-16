# Лабораторные работы №3-4: Интерфейс поля и векторы
## Современные технологии ООП

Эти лабораторные работы демонстрируют создание generic-интерфейса для математического поля и его реализацию в классах рациональных и комплексных чисел из лабораторных работ 1-2, а также создание обобщенного класса вектора над произвольным полем.

## Описание заданий

### Лабораторная работа №3: Интерфейс поля (IField)

Реализовать интерфейс поля (`IField<T>`), который включает:
- Методы парсинга строк и генерации случайных чисел
- Операторы: сложения, вычитания, умножения, деления, равенства
- Абстрактное определение операторов через `static abstract members` (C# 11+)
- Универсальные шаблоны (generics) для работы с разными типами полей
- Доработать классы `RationalNumber` и `ComplexNumber` для реализации интерфейса

### Лабораторная работа №4: Класс Vector<T>

Реализовать обобщенный класс вектора (`Vector<T>`) над математическим полем:
- Массив компонент - объекты класса, реализующего `IField<T>`
- Операторы: сложение (+), вычитание (-), векторное произведение (для размерности 3), равенство (==, !=)
- Методы: скалярное произведение, умножение на константу, генерация случайного вектора
- Парсинг строк формата "(a1, a2, ..., an)"
- ToString для вывода вектора

## Технический стек

- **.NET 8.0** (LTS) с поддержкой static abstract members
- **C# 12** с полной поддержкой generic math patterns
- Обновление с .NET 6.0 из лабораторных работ 1-2

## Структура проекта

```
lab_3-5/
├── README.md                           # Этот файл
├── docs/                               # Документация
│   ├── СТООП_лаб_3.pdf                # Методические указания (лаб. 3)
│   └── СТООП_лаб4.pdf                 # Методические указания (лаб. 4)
│
├── lab3-field-interface/               # Библиотека с интерфейсом поля
│   ├── FieldInterface.csproj           # Конфигурация проекта
│   └── src/
│       ├── IField.cs                   # Generic интерфейс поля
│       ├── FieldOperations.cs          # Обобщенные операции над полями
│       └── Vector.cs                   # ⭐ Класс вектора над полем (лаб. 4)
│
├── lab3-rational-numbers/              # Рациональные числа с IField
│   ├── RationalNumberProject.csproj    # Конфигурация проекта
│   └── src/
│       ├── RationalNumber.cs           # Класс с реализацией IField<RationalNumber>
│       └── Program.cs                  # Демонстрация функционала
│
├── lab3-complex-numbers/               # Комплексные числа с IField
│   ├── ComplexNumberProject.csproj     # Конфигурация проекта
│   └── src/
│       ├── ComplexNumber.cs            # Класс с реализацией IField<ComplexNumber>
│       └── Program.cs                  # Демонстрация функционала
│
└── lab4-vector-demo/                   # ⭐ Демонстрация векторов (лаб. 4)
    ├── VectorDemo.csproj               # Конфигурация проекта
    └── src/
        └── Program.cs                  # Демонстрация Vector<T>
```

## Ключевые концепции

### 1. Интерфейс IField<T>

Интерфейс `IField<T>` определяет контракт для типов, образующих алгебраическое поле:

```csharp
public interface IField<T> where T : IField<T>
{
    // Статические свойства
    static abstract T Zero { get; }
    static abstract T One { get; }

    // Статические методы
    static abstract T Parse(string str);
    static abstract T GenerateRandom();

    // Операторы (static abstract)
    static abstract T operator +(T left, T right);
    static abstract T operator -(T left, T right);
    static abstract T operator *(T left, T right);
    static abstract T operator /(T left, T right);
    static abstract bool operator ==(T left, T right);
    static abstract bool operator !=(T left, T right);

    // Свойства экземпляра
    T Inverse { get; }
}
```

**Особенности:**
- **CRTP (Curiously Recurring Template Pattern)**: `where T : IField<T>` обеспечивает самоссылку типа
- **Static abstract members** (C# 11+): позволяет определить контракт для статических операторов
- **Generic Math Pattern**: официальный паттерн .NET для обобщенных математических операций

### 2. FieldOperations - обобщенные утилиты

Класс `FieldOperations` демонстрирует мощь интерфейса - универсальные алгоритмы для любых полей:

```csharp
// Возведение в степень
T result = FieldOperations.Power(value, 3);

// Сумма массива
T sum = FieldOperations.Sum(elem1, elem2, elem3);

// Среднее арифметическое
T avg = FieldOperations.Average(elem1, elem2, elem3);

// Линейная комбинация
T linComb = FieldOperations.LinearCombination(coefficients, elements);
```

### 3. Реализация в RationalNumber

**Изменения:**
- Добавлено: `IField<RationalNumber>` в объявление класса
- Добавлено: `using FieldInterface;`
- Добавлен метод: `GenerateRandom()` без параметров (для интерфейса)
- Сохранен метод: `GenerateRandom(min, max)` для обратной совместимости

```csharp
public class RationalNumber : IField<RationalNumber>,
                               IEquatable<RationalNumber>,
                               IComparable<RationalNumber>
{
    // Все операторы и методы уже соответствуют интерфейсу!
}
```

### 4. Реализация в ComplexNumber

**Изменения (минимальные!):**
- Добавлено: `IField<ComplexNumber>` в объявление класса
- Добавлено: `using FieldInterface;`
- Метод `GenerateRandom()` уже соответствует интерфейсу без изменений!

```csharp
public class ComplexNumber : IField<ComplexNumber>,
                              IEquatable<ComplexNumber>
{
    // Класс уже полностью соответствовал контракту!
}
```

### 5. Класс Vector<T> (Лабораторная работа №4)

Класс `Vector<T>` представляет вектор над произвольным полем с компонентами типа `T`:

```csharp
public class Vector<T> : IEquatable<Vector<T>> where T : IField<T>
{
    private readonly T[] components;

    public int Dimension => components.Length;
    public T this[int index] { get; }  // Индексатор read-only

    // Конструкторы
    public Vector(params T[] components) { }        // Из массива
    public Vector(int dimension) { }                // Нулевой вектор

    // Операторы
    public static Vector<T> operator +(Vector<T> left, Vector<T> right) { }
    public static Vector<T> operator -(Vector<T> left, Vector<T> right) { }
    public static Vector<T> operator *(T scalar, Vector<T> vector) { }
    public static Vector<T> operator *(Vector<T> vector, T scalar) { }

    // Методы
    public T Dot(Vector<T> other) { }               // Скалярное произведение
    public Vector<T> Cross(Vector<T> other) { }     // Векторное (только 3D!)
    public T NormSquared() { }                      // Квадрат нормы
    public bool IsZero() { }                        // Проверка на нулевой

    // Статические методы
    public static Vector<T> Parse(string str) { }
    public static Vector<T> GenerateRandom(int dimension) { }
}
```

**Ключевые особенности:**
- **Неизменяемость (Immutability)**: все операции возвращают новые объекты
- **Векторное произведение**: работает ТОЛЬКО для размерности 3 (математическая корректность)
- **NormSquared вместо Norm**: квадратный корень не определен в общем поле
- **Полиморфизм**: один класс работает с любым типом, реализующим `IField<T>`

## Запуск проектов

### Требования

- .NET SDK 8.0 или выше
- Терминал с поддержкой UTF-8 для корректного отображения русского текста

### Сборка и запуск

#### 1. Сборка библиотеки интерфейса:
```bash
cd lab3-field-interface
dotnet build
```

#### 2. Запуск демонстрации рациональных чисел:
```bash
cd lab3-rational-numbers
dotnet run
```

**Демонстрирует:**
- Базовые операции (конструкторы, парсинг, арифметика, сравнения)
- Работу через интерфейс IField
- Обобщенные операции FieldOperations
- Обобщенные методы с generic constraint

#### 3. Запуск демонстрации комплексных чисел:
```bash
cd lab3-complex-numbers
dotnet run
```

**Демонстрирует:**
- Базовые операции комплексных чисел
- Работу через интерфейс IField
- Обобщенные операции FieldOperations
- Полиморфизм (один код для разных типов полей)

#### 4. Запуск демонстрации векторов (лаб. 4):
```bash
cd lab4-vector-demo
dotnet run
```

**Демонстрирует:**
- Векторы рациональных чисел (создание, операции, скалярное произведение)
- Векторы комплексных чисел
- Векторное произведение в 3D (базисные векторы, свойства)
- Обработку ошибок (разные размерности, неверный формат)

#### 5. Сборка всех проектов одновременно:
```bash
# Из директории lab_3-5
dotnet build lab3-field-interface/FieldInterface.csproj
dotnet build lab3-rational-numbers/RationalNumberProject.csproj
dotnet build lab3-complex-numbers/ComplexNumberProject.csproj
dotnet build lab4-vector-demo/VectorDemo.csproj
```

## Примеры использования

### Обобщенный метод для любого поля

```csharp
// Один метод работает с любым типом, реализующим IField<T>
static T Square<T>(T value) where T : IField<T>
{
    return value * value;  // Операторы работают!
}

// Использование:
var rational = new RationalNumber(3, 4);
var result1 = Square(rational);  // 9/16

var complex = new ComplexNumber(3, 4);
var result2 = Square(complex);   // -7.00+24.00i
```

### Универсальная функция для работы с полями

```csharp
static void PrintFieldProperties<T>(T value) where T : IField<T>
{
    Console.WriteLine($"Значение: {value}");
    Console.WriteLine($"Квадрат: {value * value}");
    Console.WriteLine($"Обратный: {value.Inverse}");
    Console.WriteLine($"Проверка: {value} * {value.Inverse} = {value * value.Inverse}");
}

// Работает и с рациональными, и с комплексными числами!
PrintFieldProperties(new RationalNumber(2, 3));
PrintFieldProperties(new ComplexNumber(3, 4));
```

### Примеры с векторами (лаб. 4)

```csharp
// Создание векторов рациональных чисел
var v1 = new Vector<RationalNumber>(
    new RationalNumber(1, 2),
    new RationalNumber(3, 4)
);
var v2 = new Vector<RationalNumber>(
    new RationalNumber(1, 3),
    new RationalNumber(1, 4)
);

// Операции
Console.WriteLine($"v1 + v2 = {v1 + v2}");          // (5/6, 1/1)
Console.WriteLine($"2 * v1 = {new RationalNumber(2) * v1}");

// Скалярное произведение
var dot = v1.Dot(v2);
Console.WriteLine($"v1 · v2 = {dot}");               // Элемент поля

// Векторное произведение (3D)
var i = new Vector<RationalNumber>(
    new RationalNumber(1), new RationalNumber(0), new RationalNumber(0)
);
var j = new Vector<RationalNumber>(
    new RationalNumber(0), new RationalNumber(1), new RationalNumber(0)
);
Console.WriteLine($"i × j = {i.Cross(j)}");         // (0/1, 0/1, 1/1)

// Парсинг
var parsed = Vector<ComplexNumber>.Parse("(3+4i, -2i, 5.5)");
Console.WriteLine($"Parsed: {parsed}");
```

### Универсальный метод для векторов любого поля

```csharp
// Проверка ортогональности для векторов любого поля
static bool AreOrthogonal<T>(Vector<T> v1, Vector<T> v2) where T : IField<T>
{
    return v1.Dot(v2) == T.Zero;
}

// Работает и с рациональными, и с комплексными векторами!
var ratV1 = new Vector<RationalNumber>(new RationalNumber(1), new RationalNumber(0));
var ratV2 = new Vector<RationalNumber>(new RationalNumber(0), new RationalNumber(1));
Console.WriteLine($"Ортогональны: {AreOrthogonal(ratV1, ratV2)}");  // true

var compV1 = new Vector<ComplexNumber>(new ComplexNumber(1, 0), new ComplexNumber(0, 1));
var compV2 = new Vector<ComplexNumber>(new ComplexNumber(0, -1), new ComplexNumber(1, 0));
Console.WriteLine($"Ортогональны: {AreOrthogonal(compV1, compV2)}");  // true
```

## Архитектурные решения

### 1. Разделение проектов
- **Интерфейс в отдельной библиотеке** - возможность переиспользования для других типов
- **Независимые проекты для типов** - чистая структура и версионирование

### 2. Static abstract members
- Современный подход C# 11+ для generic math
- Компилятор гарантирует наличие операторов на этапе компиляции
- Операторы вызываются напрямую без обертки

### 3. Обратная совместимость
- Метод `GenerateRandom(min, max)` сохранен в RationalNumber
- Добавлена перегрузка `GenerateRandom()` для интерфейса
- Существующий код продолжает работать

### 4. Immutability
- Все операции возвращают новые объекты
- Классы остаются неизменяемыми (immutable)
- Потокобезопасность при чтении

## Критерии выполнения

### Лабораторная работа №3

- ✅ Создан интерфейс `IField<T>` с static abstract members
- ✅ Интерфейс содержит все требуемые операторы и методы
- ✅ `RationalNumber` реализует `IField<RationalNumber>`
- ✅ `ComplexNumber` реализует `IField<ComplexNumber>`
- ✅ Все проекты компилируются без ошибок
- ✅ Можно писать обобщенный код с операторами для любого `IField<T>`
- ✅ FieldOperations работает с обоими типами
- ✅ Демонстрационные программы показывают все возможности

### Лабораторная работа №4

- ✅ Создан класс `Vector<T>` с generic constraint `where T : IField<T>`
- ✅ Реализованы все операторы: +, -, *, ==, !=
- ✅ Реализовано скалярное произведение (Dot) для любой размерности
- ✅ Реализовано векторное произведение (Cross) только для размерности 3
- ✅ Реализован парсинг формата "(a1, a2, ..., an)"
- ✅ Реализована генерация случайных векторов
- ✅ Работает с `Vector<RationalNumber>` и `Vector<ComplexNumber>`
- ✅ Корректная обработка ошибок (разные размерности, неверный формат)
- ✅ Демонстрация показывает все возможности класса

## Преимущества реализации

### 1. Типобезопасность
Компилятор проверяет наличие всех операций:
```csharp
void Process<T>(T value) where T : IField<T>
{
    var result = value + T.One;  // Гарантированно компилируется
}
```

### 2. Переиспользование кода
Одна реализация алгоритма для всех типов полей:
```csharp
// Один раз написано - работает везде
T Power<T>(T value, int n) where T : IField<T> { ... }
```

### 3. Расширяемость
Легко добавить новые типы полей:
```csharp
public class GaloisField : IField<GaloisField> { ... }
// Автоматически получает все методы FieldOperations!
```
