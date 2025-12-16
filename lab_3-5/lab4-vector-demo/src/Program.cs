using System;
using RationalNumbers;
using ComplexNumbers;
using FieldInterface;

namespace VectorDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("   ДЕМОНСТРАЦИЯ КЛАССА Vector<T> НАД МАТЕМАТИЧЕСКИМ ПОЛЕМ");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            // ЧАСТЬ 1: Векторы рациональных чисел
            DemonstrateRationalVectors();

            // ЧАСТЬ 2: Векторы комплексных чисел
            DemonstrateComplexVectors();

            // ЧАСТЬ 3: Векторное произведение (3D)
            DemonstrateCrossProduct();

            // ЧАСТЬ 4: Обработка ошибок
            DemonstrateErrorHandling();

            Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("                  ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void DemonstrateRationalVectors()
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("ЧАСТЬ 1: ВЕКТОРЫ РАЦИОНАЛЬНЫХ ЧИСЕЛ");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            // 1. Создание векторов
            Console.WriteLine("1. СОЗДАНИЕ ВЕКТОРОВ:\n");

            var v1 = new Vector<RationalNumber>(
                new RationalNumber(1, 2),
                new RationalNumber(3, 4)
            );

            var v2 = new Vector<RationalNumber>(
                new RationalNumber(1, 3),
                new RationalNumber(1, 4)
            );

            Console.WriteLine($"v1 = {v1}");
            Console.WriteLine($"v2 = {v2}");
            Console.WriteLine($"Размерность v1: {v1.Dimension}");
            Console.WriteLine($"Доступ к компоненте v1[0] = {v1[0]}");
            Console.WriteLine($"Доступ к компоненте v1[1] = {v1[1]}\n");

            // 2. Нулевой вектор
            Console.WriteLine("2. НУЛЕВОЙ ВЕКТОР:\n");
            var zero = new Vector<RationalNumber>(3);
            Console.WriteLine($"Нулевой вектор размерности 3: {zero}");
            Console.WriteLine($"Проверка IsZero(): {zero.IsZero()}\n");

            // 3. Операции сложения и вычитания
            Console.WriteLine("3. СЛОЖЕНИЕ И ВЫЧИТАНИЕ:\n");
            var sum = v1 + v2;
            var diff = v1 - v2;
            Console.WriteLine($"v1 + v2 = {sum}");
            Console.WriteLine($"v1 - v2 = {diff}");
            Console.WriteLine($"Проверка: (v1 + v2) - v2 = {sum - v2}");
            Console.WriteLine($"Ожидается v1: {v1}\n");

            // 4. Умножение на скаляр
            Console.WriteLine("4. УМНОЖЕНИЕ НА СКАЛЯР:\n");
            var scalar = new RationalNumber(2, 1);
            var scaled1 = scalar * v1;
            var scaled2 = v1 * scalar;
            Console.WriteLine($"Скаляр: {scalar}");
            Console.WriteLine($"scalar * v1 = {scaled1}");
            Console.WriteLine($"v1 * scalar = {scaled2}");
            Console.WriteLine($"Коммутативность: {scaled1 == scaled2}\n");

            // 5. Скалярное произведение
            Console.WriteLine("5. СКАЛЯРНОЕ ПРОИЗВЕДЕНИЕ:\n");
            var dot = v1.Dot(v2);
            Console.WriteLine($"v1 · v2 = {dot}");

            // Создадим ортогональные векторы для проверки
            var i = new Vector<RationalNumber>(
                new RationalNumber(1, 1),
                new RationalNumber(0, 1)
            );
            var j = new Vector<RationalNumber>(
                new RationalNumber(0, 1),
                new RationalNumber(1, 1)
            );
            Console.WriteLine($"\ni = {i}, j = {j}");
            Console.WriteLine($"i · j = {i.Dot(j)}  (ортогональны)");
            Console.WriteLine($"i · i = {i.Dot(i)}  (квадрат нормы)\n");

            // 6. Квадрат нормы
            Console.WriteLine("6. КВАДРАТ НОРМЫ:\n");
            var normSq = v1.NormSquared();
            Console.WriteLine($"||v1||² = v1 · v1 = {normSq}");
            Console.WriteLine($"Проверка: v1.Dot(v1) = {v1.Dot(v1)}\n");

            // 7. Парсинг векторов
            Console.WriteLine("7. ПАРСИНГ ВЕКТОРОВ:\n");
            var parsed = Vector<RationalNumber>.Parse("(1/2, 3/4, 5/6)");
            Console.WriteLine($"Parse('(1/2, 3/4, 5/6)') = {parsed}");
            Console.WriteLine($"Размерность: {parsed.Dimension}\n");

            // 8. Генерация случайного вектора
            Console.WriteLine("8. ГЕНЕРАЦИЯ СЛУЧАЙНЫХ ВЕКТОРОВ:\n");
            for (int i = 0; i < 3; i++)
            {
                var random = Vector<RationalNumber>.GenerateRandom(4);
                Console.WriteLine($"Случайный вектор {i + 1}: {random}");
            }

            // 9. Сравнение векторов
            Console.WriteLine("\n9. СРАВНЕНИЕ ВЕКТОРОВ:\n");
            var v3 = new Vector<RationalNumber>(
                new RationalNumber(2, 4),  // = 1/2
                new RationalNumber(3, 4)
            );
            Console.WriteLine($"v1 = {v1}");
            Console.WriteLine($"v3 = {v3}");
            Console.WriteLine($"v1 == v3: {v1 == v3}  (компоненты сокращаются!)");
            Console.WriteLine($"v1 != v2: {v1 != v2}");

            // 10. Преобразование в массив
            Console.WriteLine("\n10. ПРЕОБРАЗОВАНИЕ В МАССИВ:\n");
            var array = v1.ToArray();
            Console.WriteLine($"v1 как массив: [{string.Join(", ", array)}]");
        }

        static void DemonstrateComplexVectors()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("ЧАСТЬ 2: ВЕКТОРЫ КОМПЛЕКСНЫХ ЧИСЕЛ");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            // 1. Создание комплексных векторов
            Console.WriteLine("1. СОЗДАНИЕ КОМПЛЕКСНЫХ ВЕКТОРОВ:\n");

            var v1 = new Vector<ComplexNumber>(
                new ComplexNumber(1, 2),
                new ComplexNumber(3, -1)
            );

            var v2 = new Vector<ComplexNumber>(
                new ComplexNumber(2, 1),
                new ComplexNumber(0, 1)
            );

            Console.WriteLine($"v1 = {v1}");
            Console.WriteLine($"v2 = {v2}\n");

            // 2. Операции над комплексными векторами
            Console.WriteLine("2. АРИФМЕТИЧЕСКИЕ ОПЕРАЦИИ:\n");
            Console.WriteLine($"v1 + v2 = {v1 + v2}");
            Console.WriteLine($"v1 - v2 = {v1 - v2}\n");

            // 3. Умножение на мнимую единицу
            Console.WriteLine("3. УМНОЖЕНИЕ НА МНИМУЮ ЕДИНИЦУ:\n");
            var i = new ComplexNumber(0, 1);
            var rotated = i * v1;
            Console.WriteLine($"Мнимая единица: i = {i}");
            Console.WriteLine($"i * v1 = {rotated}");
            Console.WriteLine("(Поворот вектора на 90° в комплексной плоскости для каждой компоненты)\n");

            // 4. Скалярное произведение комплексных векторов
            Console.WriteLine("4. СКАЛЯРНОЕ ПРОИЗВЕДЕНИЕ:\n");
            var dot = v1.Dot(v2);
            Console.WriteLine($"v1 · v2 = {dot}");
            Console.WriteLine($"Это комплексное число!\n");

            // 5. Квадрат нормы
            Console.WriteLine("5. КВАДРАТ НОРМЫ:\n");
            var normSq = v1.NormSquared();
            Console.WriteLine($"||v1||² = {normSq}");
            Console.WriteLine("Для комплексного вектора это сумма квадратов модулей компонент\n");

            // 6. Парсинг комплексных векторов
            Console.WriteLine("6. ПАРСИНГ КОМПЛЕКСНЫХ ВЕКТОРОВ:\n");
            var parsed = Vector<ComplexNumber>.Parse("(3+4i, -2i, 5.5)");
            Console.WriteLine($"Parse('(3+4i, -2i, 5.5)') = {parsed}\n");

            // 7. Случайные комплексные векторы
            Console.WriteLine("7. СЛУЧАЙНЫЕ КОМПЛЕКСНЫЕ ВЕКТОРЫ:\n");
            for (int k = 0; k < 3; k++)
            {
                var random = Vector<ComplexNumber>.GenerateRandom(3);
                Console.WriteLine($"Случайный вектор {k + 1}: {random}");
            }

            // 8. Демонстрация универсальности
            Console.WriteLine("\n8. УНИВЕРСАЛЬНОСТЬ Vector<T>:\n");
            Console.WriteLine("Один и тот же класс Vector<T> работает с:");
            Console.WriteLine("  - Рациональными числами (RationalNumber)");
            Console.WriteLine("  - Комплексными числами (ComplexNumber)");
            Console.WriteLine("  - Любым типом, реализующим IField<T>!");
        }

        static void DemonstrateCrossProduct()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("ЧАСТЬ 3: ВЕКТОРНОЕ ПРОИЗВЕДЕНИЕ (3D)");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            // 1. Базисные векторы
            Console.WriteLine("1. БАЗИСНЫЕ ВЕКТОРЫ:\n");

            var i = new Vector<RationalNumber>(
                new RationalNumber(1), new RationalNumber(0), new RationalNumber(0)
            );
            var j = new Vector<RationalNumber>(
                new RationalNumber(0), new RationalNumber(1), new RationalNumber(0)
            );
            var k = new Vector<RationalNumber>(
                new RationalNumber(0), new RationalNumber(0), new RationalNumber(1)
            );

            Console.WriteLine($"i = {i}");
            Console.WriteLine($"j = {j}");
            Console.WriteLine($"k = {k}\n");

            // 2. Свойства векторного произведения
            Console.WriteLine("2. ВЕКТОРНОЕ ПРОИЗВЕДЕНИЕ БАЗИСНЫХ ВЕКТОРОВ:\n");
            Console.WriteLine($"i × j = {i.Cross(j)}  (ожидается k)");
            Console.WriteLine($"j × k = {j.Cross(k)}  (ожидается i)");
            Console.WriteLine($"k × i = {k.Cross(i)}  (ожидается j)\n");

            // 3. Антикоммутативность
            Console.WriteLine("3. АНТИКОММУТАТИВНОСТЬ a × b = -(b × a):\n");
            var cross1 = i.Cross(j);
            var cross2 = j.Cross(i);
            var minusK = new RationalNumber(-1) * k;
            Console.WriteLine($"i × j = {cross1}");
            Console.WriteLine($"j × i = {cross2}");
            Console.WriteLine($"-(i × j) = {new RationalNumber(-1) * cross1}");
            Console.WriteLine($"Проверка: j × i == -(i × j): {cross2 == minusK}\n");

            // 4. Перпендикулярность
            Console.WriteLine("4. ПЕРПЕНДИКУЛЯРНОСТЬ (a × b) ⊥ a и (a × b) ⊥ b:\n");
            var a = new Vector<RationalNumber>(
                new RationalNumber(1), new RationalNumber(2), new RationalNumber(3)
            );
            var b = new Vector<RationalNumber>(
                new RationalNumber(4), new RationalNumber(5), new RationalNumber(6)
            );
            var cross = a.Cross(b);

            Console.WriteLine($"a = {a}");
            Console.WriteLine($"b = {b}");
            Console.WriteLine($"a × b = {cross}");
            Console.WriteLine($"(a × b) · a = {cross.Dot(a)}  (должно быть 0)");
            Console.WriteLine($"(a × b) · b = {cross.Dot(b)}  (должно быть 0)\n");

            // 5. Площадь параллелограмма
            Console.WriteLine("5. ПЛОЩАДЬ ПАРАЛЛЕЛОГРАММА (||a × b||²):\n");
            var v1 = new Vector<RationalNumber>(
                new RationalNumber(3), new RationalNumber(0), new RationalNumber(0)
            );
            var v2 = new Vector<RationalNumber>(
                new RationalNumber(0), new RationalNumber(4), new RationalNumber(0)
            );
            var crossProduct = v1.Cross(v2);
            var areaSq = crossProduct.NormSquared();

            Console.WriteLine($"v1 = {v1}  (длина 3)");
            Console.WriteLine($"v2 = {v2}  (длина 4)");
            Console.WriteLine($"v1 × v2 = {crossProduct}");
            Console.WriteLine($"||v1 × v2||² = {areaSq}");
            Console.WriteLine($"Площадь параллелограмма = sqrt({areaSq}) = {new RationalNumber(12)} (3 * 4)\n");

            // 6. Комплексные векторы в 3D
            Console.WriteLine("6. ВЕКТОРНОЕ ПРОИЗВЕДЕНИЕ КОМПЛЕКСНЫХ ВЕКТОРОВ:\n");

            var c1 = new Vector<ComplexNumber>(
                new ComplexNumber(1, 0),
                new ComplexNumber(0, 1),
                new ComplexNumber(0, 0)
            );
            var c2 = new Vector<ComplexNumber>(
                new ComplexNumber(0, 0),
                new ComplexNumber(1, 0),
                new ComplexNumber(0, 1)
            );

            Console.WriteLine($"c1 = {c1}");
            Console.WriteLine($"c2 = {c2}");
            Console.WriteLine($"c1 × c2 = {c1.Cross(c2)}");
            Console.WriteLine("Векторное произведение работает и для комплексных чисел!\n");

            // 7. Самопересечение (a × a = 0)
            Console.WriteLine("7. САМОПЕРЕСЕЧЕНИЕ a × a = 0:\n");
            var selfCross = a.Cross(a);
            Console.WriteLine($"a = {a}");
            Console.WriteLine($"a × a = {selfCross}");
            Console.WriteLine($"Проверка IsZero(): {selfCross.IsZero()}");
        }

        static void DemonstrateErrorHandling()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("ЧАСТЬ 4: ОБРАБОТКА ОШИБОК");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            // 1. Векторное произведение для не-3D вектора
            Console.WriteLine("1. ВЕКТОРНОЕ ПРОИЗВЕДЕНИЕ ДЛЯ НЕ-3D ВЕКТОРА:\n");
            try
            {
                var v1 = new Vector<RationalNumber>(
                    new RationalNumber(1), new RationalNumber(2)
                );
                var v2 = new Vector<RationalNumber>(
                    new RationalNumber(3), new RationalNumber(4)
                );
                Console.WriteLine($"Попытка: {v1} × {v2}");
                var cross = v1.Cross(v2);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"✓ Ожидаемая ошибка: {ex.Message}");
            }

            // 2. Сложение векторов разных размерностей
            Console.WriteLine("\n2. СЛОЖЕНИЕ ВЕКТОРОВ РАЗНЫХ РАЗМЕРНОСТЕЙ:\n");
            try
            {
                var v1 = new Vector<RationalNumber>(
                    new RationalNumber(1), new RationalNumber(2)
                );
                var v2 = new Vector<RationalNumber>(
                    new RationalNumber(1), new RationalNumber(2), new RationalNumber(3)
                );
                Console.WriteLine($"Попытка: {v1} + {v2}");
                var sum = v1 + v2;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✓ Ожидаемая ошибка: {ex.Message}");
            }

            // 3. Скалярное произведение векторов разных размерностей
            Console.WriteLine("\n3. СКАЛЯРНОЕ ПРОИЗВЕДЕНИЕ РАЗНЫХ РАЗМЕРНОСТЕЙ:\n");
            try
            {
                var v1 = new Vector<RationalNumber>(
                    new RationalNumber(1), new RationalNumber(2)
                );
                var v2 = new Vector<RationalNumber>(
                    new RationalNumber(1), new RationalNumber(2), new RationalNumber(3)
                );
                Console.WriteLine($"Попытка: {v1} · {v2}");
                var dot = v1.Dot(v2);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✓ Ожидаемая ошибка: {ex.Message}");
            }

            // 4. Неверный формат парсинга
            Console.WriteLine("\n4. НЕВЕРНЫЙ ФОРМАТ ПАРСИНГА:\n");
            try
            {
                Console.WriteLine("Попытка: Parse('1/2, 3/4')  (нет скобок)");
                var parsed = Vector<RationalNumber>.Parse("1/2, 3/4");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"✓ Ожидаемая ошибка: {ex.Message}");
            }

            // 5. Пустой вектор
            Console.WriteLine("\n5. СОЗДАНИЕ ПУСТОГО ВЕКТОРА:\n");
            try
            {
                Console.WriteLine("Попытка: new Vector<RationalNumber>()  (без компонент)");
                var empty = new Vector<RationalNumber>();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✓ Ожидаемая ошибка: {ex.Message}");
            }

            // 6. Неверная компонента при парсинге
            Console.WriteLine("\n6. НЕВЕРНАЯ КОМПОНЕНТА ПРИ ПАРСИНГЕ:\n");
            try
            {
                Console.WriteLine("Попытка: Parse('(1/2, invalid, 3/4)')");
                var parsed = Vector<RationalNumber>.Parse("(1/2, invalid, 3/4)");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"✓ Ожидаемая ошибка:");
                Console.WriteLine($"   {ex.Message}");
            }

            // 7. Индекс вне диапазона
            Console.WriteLine("\n7. ДОСТУП К НЕСУЩЕСТВУЮЩЕМУ ИНДЕКСУ:\n");
            try
            {
                var v = new Vector<RationalNumber>(
                    new RationalNumber(1), new RationalNumber(2)
                );
                Console.WriteLine($"Вектор: {v}  (размерность {v.Dimension})");
                Console.WriteLine("Попытка: v[5]");
                var component = v[5];
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"✓ Ожидаемая ошибка: {ex.Message}");
            }

            // 8. Нулевая размерность
            Console.WriteLine("\n8. СОЗДАНИЕ ВЕКТОРА НУЛЕВОЙ РАЗМЕРНОСТИ:\n");
            try
            {
                Console.WriteLine("Попытка: new Vector<RationalNumber>(0)");
                var zero = new Vector<RationalNumber>(0);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✓ Ожидаемая ошибка: {ex.Message}");
            }

            Console.WriteLine("\n✓ Все ошибки обработаны корректно!");
        }
    }
}
