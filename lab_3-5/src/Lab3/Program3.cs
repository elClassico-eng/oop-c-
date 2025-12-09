using System;

namespace FieldAlgebra.Lab3
{
    class Program3
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ ИНТЕРФЕЙСА IFIELD ===\n");

            // 1. Тестирование RationalNumber
            Console.WriteLine("1. РАЦИОНАЛЬНЫЕ ЧИСЛА");
            TestRationalNumbers();

            // 2. Тестирование ComplexNumber
            Console.WriteLine("\n2. КОМПЛЕКСНЫЕ ЧИСЛА");
            TestComplexNumbers();

            // 3. Полиморфизм через интерфейс
            Console.WriteLine("\n3. ПОЛИМОРФИЗМ ЧЕРЕЗ ИНТЕРФЕЙС");
            TestPolymorphism();

            // 4. Граничные случаи
            Console.WriteLine("\n4. ГРАНИЧНЫЕ СЛУЧАИ И ВАЛИДАЦИЯ");
            TestEdgeCases();

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void TestRationalNumbers()
        {
            // Создание через конструкторы
            RationalNumber r1 = new RationalNumber(3, 4);
            RationalNumber r2 = new RationalNumber(5, 6);
            Console.WriteLine($"r1 = {r1}, r2 = {r2}");

            // Создание через Parse
            RationalNumber r3 = RationalNumber.Parse("2/3");
            RationalNumber r4 = RationalNumber.Parse("7:9");
            Console.WriteLine($"r3 = {r3}, r4 = {r4}");

            // Арифметические операции
            Console.WriteLine($"r1 + r2 = {r1 + r2}");
            Console.WriteLine($"r1 - r2 = {r1 - r2}");
            Console.WriteLine($"r1 * r2 = {r1 * r2}");
            Console.WriteLine($"r1 / r2 = {r1 / r2}");
            Console.WriteLine($"-r1 = {-r1}");

            // Специальные элементы
            Console.WriteLine($"Zero = {RationalNumber.Zero}");
            Console.WriteLine($"One = {RationalNumber.One}");
            Console.WriteLine($"Inverse(r1) = {r1.Inverse}");
            Console.WriteLine($"IsZero(r1) = {r1.IsZero}");
            Console.WriteLine($"IsZero(Zero) = {RationalNumber.Zero.IsZero}");

            // Генерация случайных чисел
            RationalNumber rand = RationalNumber.Random();
            Console.WriteLine($"Random = {rand}");

            RationalNumber randRange = RationalNumber.GenerateRandom(new RationalNumber(1, 4), new RationalNumber(3, 2));
            Console.WriteLine($"Random in range [1/4, 3/2] = {randRange}");
        }

        static void TestComplexNumbers()
        {
            // Создание через конструкторы
            ComplexNumber c1 = new ComplexNumber(2, 3);
            ComplexNumber c2 = new ComplexNumber(4, -5);
            Console.WriteLine($"c1 = {c1}, c2 = {c2}");

            // Создание через Parse
            ComplexNumber c3 = ComplexNumber.Parse("2+4i");
            ComplexNumber c4 = ComplexNumber.Parse("-3i");
            ComplexNumber c5 = ComplexNumber.Parse("5");
            ComplexNumber c6 = ComplexNumber.Parse("i");
            Console.WriteLine($"c3 = {c3}, c4 = {c4}, c5 = {c5}, c6 = {c6}");

            // Арифметические операции
            Console.WriteLine($"c1 + c2 = {c1 + c2}");
            Console.WriteLine($"c1 - c2 = {c1 - c2}");
            Console.WriteLine($"c1 * c2 = {c1 * c2}");
            Console.WriteLine($"c1 / c2 = {c1 / c2}");
            Console.WriteLine($"-c1 = {-c1}");

            // Специальные элементы
            Console.WriteLine($"Zero = {ComplexNumber.Zero}");
            Console.WriteLine($"One = {ComplexNumber.One}");
            Console.WriteLine($"Inverse(c1) = {c1.Inverse}");
            Console.WriteLine($"IsZero(c1) = {c1.IsZero}");
            Console.WriteLine($"IsZero(Zero) = {ComplexNumber.Zero.IsZero}");

            // Генерация случайных чисел
            ComplexNumber rand = ComplexNumber.Random();
            Console.WriteLine($"Random = {rand}");

            ComplexNumber randRange = ComplexNumber.GenerateRandom(new ComplexNumber(-2, -2), new ComplexNumber(2, 2));
            Console.WriteLine($"Random in range [-2-2i, 2+2i] = {randRange}");
        }

        static void TestPolymorphism()
        {
            Console.WriteLine("Тестирование с RationalNumber:");
            TestField(RationalNumber.Parse("2/3"), RationalNumber.Parse("3/4"));

            Console.WriteLine("\nТестирование с ComplexNumber:");
            TestField(ComplexNumber.Parse("1+2i"), ComplexNumber.Parse("3-4i"));
        }

        static void TestField<T>(T a, T b) where T : IField<T>
        {
            Console.WriteLine($"a = {a}");
            Console.WriteLine($"b = {b}");
            Console.WriteLine($"a + b = {a + b}");
            Console.WriteLine($"a * b = {a * b}");
            Console.WriteLine($"a == b: {a == b}");
            Console.WriteLine($"Zero: {T.Zero}");
            Console.WriteLine($"One: {T.One}");
        }

        static void TestEdgeCases()
        {
            // Граничные случаи для RationalNumber
            Console.WriteLine("RationalNumber:");

            try
            {
                RationalNumber.Parse("1/0");
            }
            catch (InvalidRationalFormatException ex)
            {
                Console.WriteLine($"✓ Parse(\"1/0\"): {ex.Message}");
            }

            try
            {
                RationalNumber.Parse("abc");
            }
            catch (InvalidRationalFormatException ex)
            {
                Console.WriteLine($"✓ Parse(\"abc\"): {ex.Message}");
            }

            try
            {
                RationalNumber r = RationalNumber.Zero;
                var inv = r.Inverse;
            }
            catch (RationalDivisionByZeroException ex)
            {
                Console.WriteLine($"✓ Inverse(Zero): {ex.Message}");
            }

            try
            {
                RationalNumber r1 = new RationalNumber(1, 2);
                RationalNumber r2 = RationalNumber.Zero;
                var result = r1 / r2;
            }
            catch (RationalDivisionByZeroException ex)
            {
                Console.WriteLine($"✓ Деление на ноль: {ex.Message}");
            }

            try
            {
                RationalNumber.GenerateRandom(new RationalNumber(3, 2), new RationalNumber(1, 4));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✓ GenerateRandom(max, min): {ex.Message}");
            }

            // Граничные случаи для ComplexNumber
            Console.WriteLine("\nComplexNumber:");

            try
            {
                ComplexNumber.Parse("abc");
            }
            catch (InvalidComplexFormatException ex)
            {
                Console.WriteLine($"✓ Parse(\"abc\"): {ex.Message}");
            }

            try
            {
                ComplexNumber c = ComplexNumber.Zero;
                var inv = c.Inverse;
            }
            catch (ComplexDivisionByZeroException ex)
            {
                Console.WriteLine($"✓ Inverse(Zero): {ex.Message}");
            }

            try
            {
                ComplexNumber c1 = new ComplexNumber(1, 2);
                ComplexNumber c2 = ComplexNumber.Zero;
                var result = c1 / c2;
            }
            catch (ComplexDivisionByZeroException ex)
            {
                Console.WriteLine($"✓ Деление на ноль: {ex.Message}");
            }

            // Тестирование специальных форматов Parse
            try
            {
                ComplexNumber i = ComplexNumber.Parse("i");
                ComplexNumber plusI = ComplexNumber.Parse("+i");
                ComplexNumber minusI = ComplexNumber.Parse("-i");
                Console.WriteLine($"✓ Parse специальных форматов: i={i}, +i={plusI}, -i={minusI}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }
        }
    }
}
