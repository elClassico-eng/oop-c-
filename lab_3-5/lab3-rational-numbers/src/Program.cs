using System;
using RationalNumbers;
using FieldInterface;

namespace RationalNumberDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ КЛАССА РАЦИОНАЛЬНЫХ ЧИСЕЛ (IField) ===\n");

            // БЛОК 1: Базовые операции (из лабораторной работы 1)
            DemonstrateBasicOperations();

            // БЛОК 2: Демонстрация интерфейса IField<RationalNumber>
            DemonstrateFieldInterface();

            // БЛОК 3: Обобщенные операции через FieldOperations
            DemonstrateGenericOperations();

            // БЛОК 4: Обобщенные методы с generic constraint
            DemonstrateGenericMethods();

            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void DemonstrateBasicOperations()
        {
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("БЛОК 1: БАЗОВЫЕ ОПЕРАЦИИ РАЦИОНАЛЬНЫХ ЧИСЕЛ");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            // 1. Тестирование конструкторов
            Console.WriteLine("1. КОНСТРУКТОРЫ:");
            var r1 = new RationalNumber(4, 8);  // Должно сократиться до 1/2
            var r2 = new RationalNumber(5);     // 5/1
            var r3 = new RationalNumber();      // 0/1
            Console.WriteLine($"RationalNumber(4, 8) = {r1}");
            Console.WriteLine($"RationalNumber(5) = {r2}");
            Console.WriteLine($"RationalNumber() = {r3}\n");

            // 2. Тестирование парсинга строк
            Console.WriteLine("2. ПАРСИНГ СТРОК:");
            try
            {
                var r4 = RationalNumber.Parse("3:7");
                var r5 = RationalNumber.Parse("6/9");  // Должно сократиться до 2/3
                var r6 = RationalNumber.Parse("-2/4"); // Должно сократиться до -1/2
                Console.WriteLine($"Parse('3:7') = {r4}");
                Console.WriteLine($"Parse('6/9') = {r5}");
                Console.WriteLine($"Parse('-2/4') = {r6}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка парсинга: {ex.Message}");
            }

            // Тестирование неверного формата
            try
            {
                var rError = RationalNumber.Parse("invalid");
            }
            catch (InvalidRationalFormatException ex)
            {
                Console.WriteLine($"Ожидаемая ошибка: {ex.Message}");
            }
            Console.WriteLine();

            // 3. Арифметические операции
            Console.WriteLine("3. АРИФМЕТИЧЕСКИЕ ОПЕРАЦИИ:");
            var a = new RationalNumber(1, 2);  // 1/2
            var b = new RationalNumber(1, 3);  // 1/3
            Console.WriteLine($"a = {a}, b = {b}");
            Console.WriteLine($"a + b = {a + b}");
            Console.WriteLine($"a - b = {a - b}");
            Console.WriteLine($"a * b = {a * b}");
            Console.WriteLine($"a / b = {a / b}\n");

            // 4. Операции сравнения
            Console.WriteLine("4. ОПЕРАЦИИ СРАВНЕНИЯ:");
            var c = new RationalNumber(2, 4);  // 1/2
            Console.WriteLine($"a = {a}, c = {c}");
            Console.WriteLine($"a == c: {a == c}");
            Console.WriteLine($"a != c: {a != c}");
            Console.WriteLine($"a < b: {a < b}");
            Console.WriteLine($"a > b: {a > b}");
            Console.WriteLine($"a <= b: {a <= b}");
            Console.WriteLine($"a >= b: {a >= b}\n");

            // 5. Специальные элементы
            Console.WriteLine("5. СПЕЦИАЛЬНЫЕ ЭЛЕМЕНТЫ:");
            Console.WriteLine($"Ноль: {RationalNumber.Zero}");
            Console.WriteLine($"Единица: {RationalNumber.One}");
            Console.WriteLine($"Обратный к 2/3: {new RationalNumber(2, 3).Inverse}");
            Console.WriteLine($"Обратный к -3/4: {new RationalNumber(-3, 4).Inverse}\n");

            // 6. Генерация случайных чисел
            Console.WriteLine("6. ГЕНЕРАЦИЯ СЛУЧАЙНЫХ ЧИСЕЛ (с диапазоном):");
            var min = new RationalNumber(1, 10);  // 0.1
            var max = new RationalNumber(1, 1);   // 1.0
            Console.WriteLine($"Диапазон: от {min} до {max}");
            for (int i = 0; i < 3; i++)
            {
                var random = RationalNumber.GenerateRandom(min, max);
                Console.WriteLine($"Случайное число {i + 1}: {random}");
            }
            Console.WriteLine();

            // 7. Тестирование исключений
            Console.WriteLine("7. ОБРАБОТКА ИСКЛЮЧЕНИЙ:");

            // Деление на ноль в конструкторе
            try
            {
                var rError1 = new RationalNumber(1, 0);
            }
            catch (RationalDivisionByZeroException ex)
            {
                Console.WriteLine($"Ошибка конструктора: {ex.Message}");
            }

            // Деление на ноль в операторе
            try
            {
                var zero = new RationalNumber(0, 1);
                var result = a / zero;
            }
            catch (RationalDivisionByZeroException ex)
            {
                Console.WriteLine($"Ошибка деления: {ex.Message}");
            }

            // Обратный элемент нуля
            try
            {
                var zeroInverse = RationalNumber.Zero.Inverse;
            }
            catch (RationalDivisionByZeroException ex)
            {
                Console.WriteLine($"Ошибка обратного элемента: {ex.Message}");
            }
        }

        static void DemonstrateFieldInterface()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════════════");
            Console.WriteLine("БЛОК 2: ДЕМОНСТРАЦИЯ ИНТЕРФЕЙСА IField<RationalNumber>");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            // 1. Статические свойства через интерфейс
            Console.WriteLine("1. СТАТИЧЕСКИЕ СВОЙСТВА ПОЛЯ:");
            Console.WriteLine($"Zero (нулевой элемент): {RationalNumber.Zero}");
            Console.WriteLine($"One (единичный элемент): {RationalNumber.One}");
            Console.WriteLine($"Проверка: Zero + One = {RationalNumber.Zero + RationalNumber.One}");
            Console.WriteLine($"Проверка: One * RationalNumber(2/3) = {RationalNumber.One * new RationalNumber(2, 3)}\n");

            // 2. Парсинг через интерфейс
            Console.WriteLine("2. ПАРСИНГ (метод интерфейса):");
            var parsed1 = RationalNumber.Parse("3/4");
            var parsed2 = RationalNumber.Parse("5:7");
            Console.WriteLine($"Parse('3/4'): {parsed1}");
            Console.WriteLine($"Parse('5:7'): {parsed2}\n");

            // 3. Генерация случайного числа (новый метод интерфейса)
            Console.WriteLine("3. ГЕНЕРАЦИЯ СЛУЧАЙНОГО ЧИСЛА (без параметров):");
            Console.WriteLine("Метод GenerateRandom() из интерфейса IField:");
            for (int i = 0; i < 5; i++)
            {
                var random = RationalNumber.GenerateRandom();
                Console.WriteLine($"  Случайное число {i + 1}: {random}");
            }
            Console.WriteLine();

            // 4. Операторы через интерфейс
            Console.WriteLine("4. ОПЕРАТОРЫ (определены в интерфейсе):");
            var x = new RationalNumber(1, 2);
            var y = new RationalNumber(1, 3);
            Console.WriteLine($"x = {x}, y = {y}");
            Console.WriteLine($"x + y = {x + y}  (оператор из IField)");
            Console.WriteLine($"x - y = {x - y}  (оператор из IField)");
            Console.WriteLine($"x * y = {x * y}  (оператор из IField)");
            Console.WriteLine($"x / y = {x / y}  (оператор из IField)");
            Console.WriteLine($"x == y: {x == y}  (оператор из IField)");
            Console.WriteLine($"x != y: {x != y}  (оператор из IField)\n");

            // 5. Обратный элемент (свойство интерфейса)
            Console.WriteLine("5. ОБРАТНЫЙ ЭЛЕМЕНТ (Inverse property):");
            var val = new RationalNumber(3, 4);
            var inv = val.Inverse;
            Console.WriteLine($"Значение: {val}");
            Console.WriteLine($"Обратный: {inv}");
            Console.WriteLine($"Произведение: {val} * {inv} = {val * inv}");
            Console.WriteLine($"Проверка: {val * inv} == One: {val * inv == RationalNumber.One}");
        }

        static void DemonstrateGenericOperations()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════════════");
            Console.WriteLine("БЛОК 3: ОБОБЩЕННЫЕ ОПЕРАЦИИ (FieldOperations)");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            var r1 = new RationalNumber(1, 2);   // 1/2
            var r2 = new RationalNumber(1, 3);   // 1/3
            var r3 = new RationalNumber(1, 4);   // 1/4

            // 1. Возведение в степень
            Console.WriteLine("1. ВОЗВЕДЕНИЕ В СТЕПЕНЬ:");
            Console.WriteLine($"({r1})^0 = {FieldOperations.Power(r1, 0)}");
            Console.WriteLine($"({r1})^1 = {FieldOperations.Power(r1, 1)}");
            Console.WriteLine($"({r1})^2 = {FieldOperations.Power(r1, 2)}");
            Console.WriteLine($"({r1})^3 = {FieldOperations.Power(r1, 3)}");
            Console.WriteLine($"({r1})^-1 = {FieldOperations.Power(r1, -1)}  (обратный элемент)");
            Console.WriteLine($"({r1})^-2 = {FieldOperations.Power(r1, -2)}\n");

            // 2. Сумма массива
            Console.WriteLine("2. СУММА МАССИВА:");
            var sum = FieldOperations.Sum(r1, r2, r3);
            Console.WriteLine($"Sum({r1}, {r2}, {r3}) = {sum}");
            Console.WriteLine($"Проверка: {r1} + {r2} + {r3} = {r1 + r2 + r3}\n");

            // 3. Произведение массива
            Console.WriteLine("3. ПРОИЗВЕДЕНИЕ МАССИВА:");
            var product = FieldOperations.Product(r1, r2, r3);
            Console.WriteLine($"Product({r1}, {r2}, {r3}) = {product}");
            Console.WriteLine($"Проверка: {r1} * {r2} * {r3} = {r1 * r2 * r3}\n");

            // 4. Среднее арифметическое
            Console.WriteLine("4. СРЕДНЕЕ АРИФМЕТИЧЕСКОЕ:");
            var avg = FieldOperations.Average(r1, r2, r3);
            Console.WriteLine($"Average({r1}, {r2}, {r3}) = {avg}");
            Console.WriteLine($"Проверка: ({r1} + {r2} + {r3}) / 3 = {sum / new RationalNumber(3, 1)}\n");

            // 5. Линейная комбинация
            Console.WriteLine("5. ЛИНЕЙНАЯ КОМБИНАЦИЯ:");
            var coefficients = new[] { new RationalNumber(2, 1), new RationalNumber(3, 1), new RationalNumber(1, 2) };
            var elements = new[] { r1, r2, r3 };
            var linComb = FieldOperations.LinearCombination(coefficients, elements);
            Console.WriteLine($"2*({r1}) + 3*({r2}) + (1/2)*({r3}) = {linComb}\n");

            // 6. Проверка на Zero и One
            Console.WriteLine("6. ПРОВЕРКА НА СПЕЦИАЛЬНЫЕ ЭЛЕМЕНТЫ:");
            Console.WriteLine($"IsZero({RationalNumber.Zero}): {FieldOperations.IsZero(RationalNumber.Zero)}");
            Console.WriteLine($"IsZero({r1}): {FieldOperations.IsZero(r1)}");
            Console.WriteLine($"IsOne({RationalNumber.One}): {FieldOperations.IsOne(RationalNumber.One)}");
            Console.WriteLine($"IsOne({r1}): {FieldOperations.IsOne(r1)}\n");

            // 7. Таблица операций
            Console.WriteLine("7. ТАБЛИЦЫ ОПЕРАЦИЙ:");
            var elements2 = new[] {
                new RationalNumber(1, 2),
                new RationalNumber(1, 3),
                new RationalNumber(2, 3)
            };
            FieldOperations.PrintOperationTable("+", (a, b) => a + b, elements2);
            FieldOperations.PrintOperationTable("*", (a, b) => a * b, elements2);
        }

        static void DemonstrateGenericMethods()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════════════");
            Console.WriteLine("БЛОК 4: ОБОБЩЕННЫЕ МЕТОДЫ С GENERIC CONSTRAINT");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            var a = new RationalNumber(2, 3);
            var b = new RationalNumber(3, 4);

            Console.WriteLine("1. ОБОБЩЕННЫЕ АРИФМЕТИЧЕСКИЕ ОПЕРАЦИИ:");
            Console.WriteLine($"a = {a}, b = {b}");
            Console.WriteLine($"GenericAdd(a, b) = {GenericAdd(a, b)}");
            Console.WriteLine($"GenericSubtract(a, b) = {GenericSubtract(a, b)}");
            Console.WriteLine($"GenericMultiply(a, b) = {GenericMultiply(a, b)}");
            Console.WriteLine($"GenericDivide(a, b) = {GenericDivide(a, b)}\n");

            Console.WriteLine("2. ОБОБЩЕННАЯ ИНФОРМАЦИЯ О ЭЛЕМЕНТЕ ПОЛЯ:");
            PrintFieldInfo(a);

            Console.WriteLine("\n3. ДЕМОНСТРАЦИЯ ПОЛИМОРФИЗМА:");
            Console.WriteLine("Один и тот же обобщенный метод работает с RationalNumber!");
            var values = new[] {
                new RationalNumber(1, 2),
                new RationalNumber(1, 3),
                new RationalNumber(1, 6)
            };
            PrintArraySum(values);
        }

        // Обобщенные методы - работают с любым типом, реализующим IField<T>
        static T GenericAdd<T>(T a, T b) where T : IField<T>
        {
            return a + b;  // Операторы работают в обобщенном коде!
        }

        static T GenericSubtract<T>(T a, T b) where T : IField<T>
        {
            return a - b;
        }

        static T GenericMultiply<T>(T a, T b) where T : IField<T>
        {
            return a * b;
        }

        static T GenericDivide<T>(T a, T b) where T : IField<T>
        {
            return a / b;
        }

        static void PrintFieldInfo<T>(T value) where T : IField<T>
        {
            Console.WriteLine($"Информация об элементе поля:");
            Console.WriteLine($"  Значение: {value}");
            Console.WriteLine($"  Обратный элемент: {value.Inverse}");
            Console.WriteLine($"  value * value.Inverse = {value * value.Inverse}");
            Console.WriteLine($"  value + Zero = {value + T.Zero}");
            Console.WriteLine($"  value * One = {value * T.One}");
        }

        static void PrintArraySum<T>(T[] values) where T : IField<T>
        {
            Console.WriteLine($"Массив из {values.Length} элементов:");
            foreach (var val in values)
            {
                Console.Write($"{val}  ");
            }
            Console.WriteLine($"\nСумма: {FieldOperations.Sum(values)}");
        }
    }
}
