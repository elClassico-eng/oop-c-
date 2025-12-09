using System;

namespace FieldAlgebra.Lab4
{
    class Program4
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ ВЕКТОРОВ ===\n");

            // 1. Векторы с рациональными числами
            Console.WriteLine("1. ВЕКТОРЫ С РАЦИОНАЛЬНЫМИ ЧИСЛАМИ");
            TestRationalVectors();

            // 2. Векторы с комплексными числами
            Console.WriteLine("\n2. ВЕКТОРЫ С КОМПЛЕКСНЫМИ ЧИСЛАМИ");
            TestComplexVectors();

            // 3. Граничные случаи
            Console.WriteLine("\n3. ГРАНИЧНЫЕ СЛУЧАИ И ВАЛИДАЦИЯ");
            TestEdgeCases();

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void TestRationalVectors()
        {
            // Создание векторов
            Vector<RationalNumber> v1 = new Vector<RationalNumber>(
                new RationalNumber(1, 2),
                new RationalNumber(3, 4),
                new RationalNumber(5, 6)
            );
            Vector<RationalNumber> v2 = new Vector<RationalNumber>(
                new RationalNumber(2, 3),
                new RationalNumber(4, 5),
                new RationalNumber(6, 7)
            );
            Console.WriteLine($"v1 = {v1}");
            Console.WriteLine($"v2 = {v2}");

            // Арифметические операции
            Console.WriteLine($"\nАрифметические операции:");
            Console.WriteLine($"v1 + v2 = {v1 + v2}");
            Console.WriteLine($"v1 - v2 = {v1 - v2}");

            RationalNumber scalar = new RationalNumber(2, 1);
            Console.WriteLine($"v1 * {scalar} = {v1 * scalar}");
            Console.WriteLine($"-v1 = {-v1}");

            // Скалярное произведение
            Console.WriteLine($"\nСкалярное произведение:");
            Console.WriteLine($"v1 · v2 = {v1.DotProduct(v2)}");

            // Векторное произведение (для 3D)
            Console.WriteLine($"\nВекторное произведение:");
            Console.WriteLine($"v1 × v2 = {v1.CrossProduct(v2)}");

            // Модуль вектора
            Console.WriteLine($"\nМодуль вектора:");
            Console.WriteLine($"||v1|| = {v1.Magnitude():F4}");
            Console.WriteLine($"||v1||² = {v1.MagnitudeSquared()}");

            // Нормализация
            try
            {
                Vector<RationalNumber> normalized = v1.Normalize();
                Console.WriteLine($"Нормализованный v1 = {normalized}");
                Console.WriteLine($"||normalized|| = {normalized.Magnitude():F4}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка нормализации: {ex.Message}");
            }

            // Парсинг
            Console.WriteLine($"\nПарсинг:");
            Vector<RationalNumber> parsed = Vector<RationalNumber>.Parse("(1/2, 3/4, 5/6)");
            Console.WriteLine($"Parse(\"(1/2, 3/4, 5/6)\") = {parsed}");

            // Случайные векторы
            Console.WriteLine($"\nСлучайные векторы:");
            Vector<RationalNumber> rand = Vector<RationalNumber>.Random(3);
            Console.WriteLine($"Random(3) = {rand}");

            Vector<RationalNumber> randRange = Vector<RationalNumber>.GenerateRandom(
                3,
                new RationalNumber(1, 10),
                new RationalNumber(1, 2)
            );
            Console.WriteLine($"GenerateRandom в диапазоне [1/10, 1/2] = {randRange}");

            // Специальные векторы
            Console.WriteLine($"\nСпециальные векторы:");
            Vector<RationalNumber> zero = Vector<RationalNumber>.Zero(3);
            Console.WriteLine($"Zero(3) = {zero}, IsZero = {zero.IsZero()}");

            Vector<RationalNumber> basis = Vector<RationalNumber>.BasisVector(3, 1);
            Console.WriteLine($"BasisVector(3, 1) = {basis}");
        }

        static void TestComplexVectors()
        {
            // Создание векторов
            Vector<ComplexNumber> v1 = new Vector<ComplexNumber>(
                new ComplexNumber(1, 2),
                new ComplexNumber(3, -4),
                new ComplexNumber(5, 6)
            );
            Vector<ComplexNumber> v2 = new Vector<ComplexNumber>(
                new ComplexNumber(2, -1),
                new ComplexNumber(4, 3),
                new ComplexNumber(-5, 2)
            );
            Console.WriteLine($"v1 = {v1}");
            Console.WriteLine($"v2 = {v2}");

            // Арифметические операции
            Console.WriteLine($"\nАрифметические операции:");
            Console.WriteLine($"v1 + v2 = {v1 + v2}");
            Console.WriteLine($"v1 - v2 = {v1 - v2}");

            ComplexNumber scalar = new ComplexNumber(2, 1);
            Console.WriteLine($"v1 * {scalar} = {v1 * scalar}");

            // Скалярное произведение
            Console.WriteLine($"\nСкалярное произведение:");
            Console.WriteLine($"v1 · v2 = {v1.DotProduct(v2)}");

            // Векторное произведение
            Console.WriteLine($"\nВекторное произведение:");
            Console.WriteLine($"v1 × v2 = {v1.CrossProduct(v2)}");

            // Модуль вектора
            Console.WriteLine($"\nМодуль вектора:");
            Console.WriteLine($"||v1|| = {v1.Magnitude():F4}");

            // Парсинг
            Console.WriteLine($"\nПарсинг:");
            Vector<ComplexNumber> parsed = Vector<ComplexNumber>.Parse("(2+3i, -4i, 5)");
            Console.WriteLine($"Parse(\"(2+3i, -4i, 5)\") = {parsed}");

            // FromDouble/ToDouble
            Console.WriteLine($"\nFromDouble/ToDouble:");
            ComplexNumber fromDouble = ComplexNumber.FromDouble(3.14);
            Console.WriteLine($"FromDouble(3.14) = {fromDouble}");
            Console.WriteLine($"ToDouble() = {fromDouble.ToDouble():F2}");
        }

        static void TestEdgeCases()
        {
            // Граничные случаи для векторов
            Console.WriteLine("Векторы:");

            // Разные размерности
            try
            {
                Vector<RationalNumber> v1 = new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(2, 1));
                Vector<RationalNumber> v2 = new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(2, 1), new RationalNumber(3, 1));
                var result = v1 + v2;
            }
            catch (VectorDimensionMismatchException ex)
            {
                Console.WriteLine($"✓ Сложение векторов разной размерности: {ex.Message}");
            }

            // Векторное произведение не-3D
            try
            {
                Vector<RationalNumber> v1 = new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(2, 1));
                Vector<RationalNumber> v2 = new Vector<RationalNumber>(new RationalNumber(3, 1), new RationalNumber(4, 1));
                var result = v1.CrossProduct(v2);
            }
            catch (VectorOperationException ex)
            {
                Console.WriteLine($"✓ Векторное произведение 2D векторов: {ex.Message}");
            }

            // Нормализация нулевого вектора
            try
            {
                Vector<RationalNumber> zero = Vector<RationalNumber>.Zero(3);
                var normalized = zero.Normalize();
            }
            catch (VectorOperationException ex)
            {
                Console.WriteLine($"✓ Нормализация нулевого вектора: {ex.Message}");
            }

            // Некорректный Parse
            try
            {
                Vector<RationalNumber>.Parse("[1/2, 3/4");
            }
            catch (InvalidVectorFormatException ex)
            {
                Console.WriteLine($"✓ Parse(\"[1/2, 3/4\"): Неверный формат");
            }

            try
            {
                Vector<RationalNumber>.Parse("(a, b)");
            }
            catch (InvalidVectorFormatException ex)
            {
                Console.WriteLine($"✓ Parse(\"(a, b)\"): Ошибка парсинга компонентов");
            }

            // Индексация за пределами
            try
            {
                Vector<RationalNumber> v = new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(2, 1));
                var element = v[5];
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"✓ Индексация за пределами: {ex.Message}");
            }

            // Деление на нулевой скаляр
            try
            {
                Vector<RationalNumber> v = new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(2, 1));
                var result = v / RationalNumber.Zero;
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"✓ Деление на нулевой скаляр: {ex.Message}");
            }

            // ToDouble для комплексного с мнимой частью
            try
            {
                ComplexNumber c = new ComplexNumber(2, 3);
                double d = c.ToDouble();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"✓ ToDouble для комплексного с мнимой частью: {ex.Message}");
            }

            // Пустой вектор
            try
            {
                Vector<RationalNumber> v = new Vector<RationalNumber>(0);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✓ Создание вектора нулевой длины: {ex.Message}");
            }

            // Ортогональность
            Console.WriteLine($"\nДополнительно:");
            Vector<RationalNumber> v1 = new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(0, 1));
            Vector<RationalNumber> v2 = new Vector<RationalNumber>(new RationalNumber(0, 1), new RationalNumber(1, 1));
            Console.WriteLine($"v1 = {v1}, v2 = {v2}");
            Console.WriteLine($"Ортогональны: {v1.IsOrthogonalTo(v2)}");
        }
    }
}
