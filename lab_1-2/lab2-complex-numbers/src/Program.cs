using System;
using ComplexNumbers;

namespace ComplexNumberDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ КЛАССА КОМПЛЕКСНЫХ ЧИСЕЛ ===\n");

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
            for (int i = 0; i < 5; i++)
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

            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}