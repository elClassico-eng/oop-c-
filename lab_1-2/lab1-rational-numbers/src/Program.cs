using System;
using RationalNumbers;

namespace RationalNumberDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ КЛАССА РАЦИОНАЛЬНЫХ ЧИСЕЛ ===\n");

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
            Console.WriteLine("6. ГЕНЕРАЦИЯ СЛУЧАЙНЫХ ЧИСЕЛ:");
            var min = new RationalNumber(1, 10);  // 0.1
            var max = new RationalNumber(1, 1);   // 1.0
            Console.WriteLine($"Диапазон: от {min} до {max}");
            for (int i = 0; i < 5; i++)
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

            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}