using System;
using ComplexNumbers;
using FieldInterface;

namespace ComplexNumberDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ КЛАССА КОМПЛЕКСНЫХ ЧИСЕЛ (IField) ===\n");

            // БЛОК 1: Базовые операции (из лабораторной работы 2)
            DemonstrateBasicOperations();

            // БЛОК 2: Демонстрация интерфейса IField<ComplexNumber>
            DemonstrateFieldInterface();

            // БЛОК 3: Обобщенные операции через FieldOperations
            DemonstrateGenericOperations();

            // БЛОК 4: Обобщенные методы и полиморфизм
            DemonstrateGenericMethodsAndPolymorphism();

            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void DemonstrateBasicOperations()
        {
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("БЛОК 1: БАЗОВЫЕ ОПЕРАЦИИ КОМПЛЕКСНЫХ ЧИСЕЛ");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            // 1. Тестирование конструкторов
            Console.WriteLine("1. КОНСТРУКТОРЫ:");
            var c1 = new ComplexNumber(3, 4);
            var c2 = new ComplexNumber(5);
            var c3 = new ComplexNumber();
            Console.WriteLine($"ComplexNumber(3, 4) = {c1}");
            Console.WriteLine($"ComplexNumber(5) = {c2}");
            Console.WriteLine($"ComplexNumber() = {c3}\n");

            // 2. Тестирование парсинга строк
            Console.WriteLine("2. ПАРСИНГ СТРОК:");
            try
            {
                var c4 = ComplexNumber.Parse("2+4i");
                var c5 = ComplexNumber.Parse("-4i");
                var c6 = ComplexNumber.Parse("3.5-2.1i");
                var c7 = ComplexNumber.Parse("7");
                Console.WriteLine($"Parse('2+4i') = {c4}");
                Console.WriteLine($"Parse('-4i') = {c5}");
                Console.WriteLine($"Parse('3.5-2.1i') = {c6}");
                Console.WriteLine($"Parse('7') = {c7}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка парсинга: {ex.Message}");
            }

            // Тестирование неверного формата
            try
            {
                var cError = ComplexNumber.Parse("invalid");
            }
            catch (InvalidComplexFormatException ex)
            {
                Console.WriteLine($"Ожидаемая ошибка: {ex.Message}");
            }
            Console.WriteLine();

            // 3. Арифметические операции
            Console.WriteLine("3. АРИФМЕТИЧЕСКИЕ ОПЕРАЦИИ:");
            var a = new ComplexNumber(2, 3);
            var b = new ComplexNumber(1, -2);
            Console.WriteLine($"a = {a}, b = {b}");
            Console.WriteLine($"a + b = {a + b}");
            Console.WriteLine($"a - b = {a - b}");
            Console.WriteLine($"a * b = {a * b}");
            Console.WriteLine($"a / b = {a / b}\n");

            // 4. Операции сравнения
            Console.WriteLine("4. ОПЕРАЦИИ РАВЕНСТВА:");
            var d = new ComplexNumber(2, 3);
            Console.WriteLine($"a = {a}, d = {d}");
            Console.WriteLine($"a == d: {a == d}");
            Console.WriteLine($"a != d: {a != d}");
            Console.WriteLine($"a == b: {a == b}\n");

            // 5. Специальные элементы
            Console.WriteLine("5. СПЕЦИАЛЬНЫЕ ЭЛЕМЕНТЫ:");
            Console.WriteLine($"Ноль: {ComplexNumber.Zero}");
            Console.WriteLine($"Единица: {ComplexNumber.One}");
            Console.WriteLine($"Обратный к 2+3i: {new ComplexNumber(2, 3).Inverse}");
            Console.WriteLine($"Обратный к -1-2i: {new ComplexNumber(-1, -2).Inverse}\n");

            // 6. Генерация случайных чисел
            Console.WriteLine("6. ГЕНЕРАЦИЯ СЛУЧАЙНЫХ ЧИСЕЛ:");
            for (int i = 0; i < 3; i++)
            {
                var random = ComplexNumber.GenerateRandom();
                Console.WriteLine($"Случайное число {i + 1}: {random}");
            }
            Console.WriteLine();

            // 7. Тестирование исключений
            Console.WriteLine("7. ОБРАБОТКА ИСКЛЮЧЕНИЙ:");

            // Деление на ноль
            try
            {
                var zero = new ComplexNumber(0, 0);
                var result = a / zero;
            }
            catch (ComplexDivisionByZeroException ex)
            {
                Console.WriteLine($"Ошибка деления: {ex.Message}");
            }

            // Обратный элемент нуля
            try
            {
                var zeroInverse = ComplexNumber.Zero.Inverse;
            }
            catch (ComplexDivisionByZeroException ex)
            {
                Console.WriteLine($"Ошибка обратного элемента: {ex.Message}");
            }
        }

        static void DemonstrateFieldInterface()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════════════");
            Console.WriteLine("БЛОК 2: ДЕМОНСТРАЦИЯ ИНТЕРФЕЙСА IField<ComplexNumber>");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            // 1. Статические свойства через интерфейс
            Console.WriteLine("1. СТАТИЧЕСКИЕ СВОЙСТВА ПОЛЯ:");
            Console.WriteLine($"Zero (нулевой элемент): {ComplexNumber.Zero}");
            Console.WriteLine($"One (единичный элемент): {ComplexNumber.One}");
            Console.WriteLine($"Проверка: Zero + One = {ComplexNumber.Zero + ComplexNumber.One}");
            Console.WriteLine($"Проверка: One * ComplexNumber(2+3i) = {ComplexNumber.One * new ComplexNumber(2, 3)}\n");

            // 2. Парсинг через интерфейс
            Console.WriteLine("2. ПАРСИНГ (метод интерфейса):");
            var parsed1 = ComplexNumber.Parse("3+4i");
            var parsed2 = ComplexNumber.Parse("-2.5i");
            Console.WriteLine($"Parse('3+4i'): {parsed1}");
            Console.WriteLine($"Parse('-2.5i'): {parsed2}\n");

            // 3. Генерация случайного числа (метод интерфейса)
            Console.WriteLine("3. ГЕНЕРАЦИЯ СЛУЧАЙНОГО ЧИСЛА:");
            Console.WriteLine("Метод GenerateRandom() из интерфейса IField:");
            for (int i = 0; i < 5; i++)
            {
                var random = ComplexNumber.GenerateRandom();
                Console.WriteLine($"  Случайное число {i + 1}: {random}");
            }
            Console.WriteLine();

            // 4. Операторы через интерфейс
            Console.WriteLine("4. ОПЕРАТОРЫ (определены в интерфейсе):");
            var x = new ComplexNumber(1, 2);
            var y = new ComplexNumber(3, -1);
            Console.WriteLine($"x = {x}, y = {y}");
            Console.WriteLine($"x + y = {x + y}  (оператор из IField)");
            Console.WriteLine($"x - y = {x - y}  (оператор из IField)");
            Console.WriteLine($"x * y = {x * y}  (оператор из IField)");
            Console.WriteLine($"x / y = {x / y}  (оператор из IField)");
            Console.WriteLine($"x == y: {x == y}  (оператор из IField)");
            Console.WriteLine($"x != y: {x != y}  (оператор из IField)\n");

            // 5. Обратный элемент (свойство интерфейса)
            Console.WriteLine("5. ОБРАТНЫЙ ЭЛЕМЕНТ (Inverse property):");
            var val = new ComplexNumber(3, 4);
            var inv = val.Inverse;
            Console.WriteLine($"Значение: {val}");
            Console.WriteLine($"Обратный: {inv}");
            Console.WriteLine($"Произведение: {val} * {inv} = {val * inv}");
            Console.WriteLine($"Проверка: {val * inv} == One: {val * inv == ComplexNumber.One}");
        }

        static void DemonstrateGenericOperations()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════════════");
            Console.WriteLine("БЛОК 3: ОБОБЩЕННЫЕ ОПЕРАЦИИ (FieldOperations)");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            var c1 = new ComplexNumber(1, 2);
            var c2 = new ComplexNumber(-1, 1);
            var c3 = new ComplexNumber(2, -1);

            // 1. Возведение в степень
            Console.WriteLine("1. ВОЗВЕДЕНИЕ В СТЕПЕНЬ:");
            Console.WriteLine($"({c1})^0 = {FieldOperations.Power(c1, 0)}");
            Console.WriteLine($"({c1})^1 = {FieldOperations.Power(c1, 1)}");
            Console.WriteLine($"({c1})^2 = {FieldOperations.Power(c1, 2)}");
            Console.WriteLine($"({c1})^3 = {FieldOperations.Power(c1, 3)}");
            Console.WriteLine($"({c1})^-1 = {FieldOperations.Power(c1, -1)}  (обратный элемент)\n");

            // 2. Сумма массива
            Console.WriteLine("2. СУММА МАССИВА:");
            var sum = FieldOperations.Sum(c1, c2, c3);
            Console.WriteLine($"Sum({c1}, {c2}, {c3}) = {sum}");
            Console.WriteLine($"Проверка: {c1} + {c2} + {c3} = {c1 + c2 + c3}\n");

            // 3. Произведение массива
            Console.WriteLine("3. ПРОИЗВЕДЕНИЕ МАССИВА:");
            var product = FieldOperations.Product(c1, c2, c3);
            Console.WriteLine($"Product({c1}, {c2}, {c3}) = {product}");
            Console.WriteLine($"Проверка: {c1} * {c2} * {c3} = {c1 * c2 * c3}\n");

            // 4. Среднее арифметическое
            Console.WriteLine("4. СРЕДНЕЕ АРИФМЕТИЧЕСКОЕ:");
            var avg = FieldOperations.Average(c1, c2, c3);
            Console.WriteLine($"Average({c1}, {c2}, {c3}) = {avg}\n");

            // 5. Линейная комбинация
            Console.WriteLine("5. ЛИНЕЙНАЯ КОМБИНАЦИЯ:");
            var coefficients = new[] {
                new ComplexNumber(2, 0),
                new ComplexNumber(1, 1),
                new ComplexNumber(0, -1)
            };
            var elements = new[] { c1, c2, c3 };
            var linComb = FieldOperations.LinearCombination(coefficients, elements);
            Console.WriteLine($"2*({c1}) + (1+1i)*({c2}) + (-i)*({c3}) = {linComb}\n");

            // 6. Проверка на Zero и One
            Console.WriteLine("6. ПРОВЕРКА НА СПЕЦИАЛЬНЫЕ ЭЛЕМЕНТЫ:");
            Console.WriteLine($"IsZero({ComplexNumber.Zero}): {FieldOperations.IsZero(ComplexNumber.Zero)}");
            Console.WriteLine($"IsZero({c1}): {FieldOperations.IsZero(c1)}");
            Console.WriteLine($"IsOne({ComplexNumber.One}): {FieldOperations.IsOne(ComplexNumber.One)}");
            Console.WriteLine($"IsOne({c1}): {FieldOperations.IsOne(c1)}\n");

            // 7. Таблица операций
            Console.WriteLine("7. ТАБЛИЦЫ ОПЕРАЦИЙ:");
            var elements2 = new[] {
                new ComplexNumber(1, 0),
                new ComplexNumber(0, 1),
                new ComplexNumber(1, 1)
            };
            FieldOperations.PrintOperationTable("+", (a, b) => a + b, elements2);
            FieldOperations.PrintOperationTable("*", (a, b) => a * b, elements2);
        }

        static void DemonstrateGenericMethodsAndPolymorphism()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════════════");
            Console.WriteLine("БЛОК 4: ПОЛИМОРФИЗМ И ОБОБЩЕННЫЕ МЕТОДЫ");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            var a = new ComplexNumber(3, 4);
            var b = new ComplexNumber(1, -1);

            Console.WriteLine("1. ОБОБЩЕННЫЕ АРИФМЕТИЧЕСКИЕ ОПЕРАЦИИ:");
            Console.WriteLine($"a = {a}, b = {b}");
            Console.WriteLine($"GenericAdd(a, b) = {GenericAdd(a, b)}");
            Console.WriteLine($"GenericSubtract(a, b) = {GenericSubtract(a, b)}");
            Console.WriteLine($"GenericMultiply(a, b) = {GenericMultiply(a, b)}");
            Console.WriteLine($"GenericDivide(a, b) = {GenericDivide(a, b)}\n");

            Console.WriteLine("2. ОБОБЩЕННАЯ ИНФОРМАЦИЯ О ЭЛЕМЕНТЕ ПОЛЯ:");
            PrintFieldInfo(a);

            Console.WriteLine("\n3. ДЕМОНСТРАЦИЯ УНИВЕРСАЛЬНОСТИ:");
            Console.WriteLine("Один и тот же обобщенный метод работает с ComplexNumber!");
            var values = new[] {
                new ComplexNumber(1, 0),
                new ComplexNumber(0, 1),
                new ComplexNumber(1, 1)
            };
            PrintArraySum(values);

            Console.WriteLine("\n4. ДЕМОНСТРАЦИЯ POWER С КОМПЛЕКСНЫМИ ЧИСЛАМИ:");
            Console.WriteLine("Мнимая единица i в различных степенях:");
            var i = new ComplexNumber(0, 1);
            Console.WriteLine($"i^0 = {FieldOperations.Power(i, 0)}");
            Console.WriteLine($"i^1 = {FieldOperations.Power(i, 1)}");
            Console.WriteLine($"i^2 = {FieldOperations.Power(i, 2)}");
            Console.WriteLine($"i^3 = {FieldOperations.Power(i, 3)}");
            Console.WriteLine($"i^4 = {FieldOperations.Power(i, 4)}");
            Console.WriteLine($"Наблюдаем периодичность: i^0=1, i^1=i, i^2=-1, i^3=-i, i^4=1");

            Console.WriteLine("\n5. СРАВНЕНИЕ С РАЦИОНАЛЬНЫМИ ЧИСЛАМИ:");
            Console.WriteLine("Интерфейс IField обеспечивает единый API для:");
            Console.WriteLine("  - Рациональных чисел (RationalNumber)");
            Console.WriteLine("  - Комплексных чисел (ComplexNumber)");
            Console.WriteLine("  - Любых других типов, образующих поле!");
            Console.WriteLine("\nОбщие методы FieldOperations работают с обоими типами!");
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
