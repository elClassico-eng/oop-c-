using System;
using RationalNumbers;
using ComplexNumbers;
using FieldInterface;

namespace LinearSystemsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("   ДЕМОНСТРАЦИЯ РЕШЕНИЯ СИСТЕМ ЛИНЕЙНЫХ УРАВНЕНИЙ");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            // Часть 1: Решение систем
            DemonstrateSolving();

            // Часть 2: Проверка существования и единственности
            DemonstrateExistenceCheck();

            // Часть 3: Верификация решений
            DemonstrateVerification();

            Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("                  ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void DemonstrateSolving()
        {
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("ЧАСТЬ 1: РЕШЕНИЕ СИСТЕМ ЛИНЕЙНЫХ УРАВНЕНИЙ");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            // Пример 1: Простая 2x2 система над рациональными числами
            Console.WriteLine("1. СИСТЕМА 2x2 НАД РАЦИОНАЛЬНЫМИ ЧИСЛАМИ:\n");

            var A1 = new Matrix<RationalNumber>(
                new Vector<RationalNumber>(new RationalNumber(2), new RationalNumber(1)),
                new Vector<RationalNumber>(new RationalNumber(1), new RationalNumber(3))
            );
            var b1 = new Vector<RationalNumber>(new RationalNumber(5), new RationalNumber(6));

            var system1 = new LinearSystem<RationalNumber>(A1, b1);
            system1.Solve();
            Console.WriteLine(system1.ToDetailedString());

            // Пример 2: Система 3x3 над комплексными числами
            Console.WriteLine("\n2. СИСТЕМА 3x3 НАД КОМПЛЕКСНЫМИ ЧИСЛАМИ:\n");

            var A2 = new Matrix<ComplexNumber>(
                new Vector<ComplexNumber>(new ComplexNumber(1, 0), new ComplexNumber(2, 0), new ComplexNumber(1, 0)),
                new Vector<ComplexNumber>(new ComplexNumber(0, 1), new ComplexNumber(1, 1), new ComplexNumber(2, 0)),
                new Vector<ComplexNumber>(new ComplexNumber(1, 1), new ComplexNumber(0, -1), new ComplexNumber(3, 0))
            );
            var b2 = new Vector<ComplexNumber>(
                new ComplexNumber(4, 0),
                new ComplexNumber(3, 2),
                new ComplexNumber(5, 1)
            );

            var system2 = new LinearSystem<ComplexNumber>(A2, b2);
            system2.Solve();
            Console.WriteLine(system2.ToDetailedString());
        }

        static void DemonstrateExistenceCheck()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("ЧАСТЬ 2: ПРОВЕРКА СУЩЕСТВОВАНИЯ И ЕДИНСТВЕННОСТИ РЕШЕНИЯ");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            // Случай 1: Единственное решение
            Console.WriteLine("1. ЕДИНСТВЕННОЕ РЕШЕНИЕ (rank(A) = rank([A|b]) = n):\n");

            var A_unique = new Matrix<RationalNumber>(
                new Vector<RationalNumber>(new RationalNumber(3), new RationalNumber(2)),
                new Vector<RationalNumber>(new RationalNumber(1), new RationalNumber(4))
            );
            var b_unique = new Vector<RationalNumber>(new RationalNumber(7), new RationalNumber(9));

            var system_unique = new LinearSystem<RationalNumber>(A_unique, b_unique);
            system_unique.Solve();
            Console.WriteLine(system_unique.ToDetailedString());

            // Случай 2: Бесконечно много решений
            Console.WriteLine("\n2. БЕСКОНЕЧНО МНОГО РЕШЕНИЙ (rank(A) = rank([A|b]) < n):\n");

            var A_infinite = new Matrix<RationalNumber>(
                new Vector<RationalNumber>(new RationalNumber(1), new RationalNumber(2), new RationalNumber(3)),
                new Vector<RationalNumber>(new RationalNumber(2), new RationalNumber(4), new RationalNumber(6))
            );
            var b_infinite = new Vector<RationalNumber>(new RationalNumber(5), new RationalNumber(10));

            var system_infinite = new LinearSystem<RationalNumber>(A_infinite, b_infinite);
            system_infinite.Solve();
            Console.WriteLine(system_infinite.ToDetailedString());

            // Случай 3: Нет решения
            Console.WriteLine("\n3. НЕТ РЕШЕНИЯ (rank(A) < rank([A|b])):\n");

            var A_none = new Matrix<RationalNumber>(
                new Vector<RationalNumber>(new RationalNumber(1), new RationalNumber(2)),
                new Vector<RationalNumber>(new RationalNumber(1), new RationalNumber(2))
            );
            var b_none = new Vector<RationalNumber>(new RationalNumber(3), new RationalNumber(5));

            var system_none = new LinearSystem<RationalNumber>(A_none, b_none);
            system_none.Solve();
            Console.WriteLine(system_none.ToDetailedString());
        }

        static void DemonstrateVerification()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("ЧАСТЬ 3: ВЕРИФИКАЦИЯ РЕШЕНИЙ");
            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

            Console.WriteLine("1. АВТОМАТИЧЕСКАЯ ВЕРИФИКАЦИЯ РЕШЕНИЯ:\n");

            // Создаем систему с известным решением
            var A = new Matrix<RationalNumber>(
                new Vector<RationalNumber>(new RationalNumber(1), new RationalNumber(1)),
                new Vector<RationalNumber>(new RationalNumber(2), new RationalNumber(-1))
            );
            var b = new Vector<RationalNumber>(new RationalNumber(3), new RationalNumber(0));

            var system = new LinearSystem<RationalNumber>(A, b);
            system.Solve();

            Console.WriteLine($"Система:");
            Console.WriteLine($"  x + y = 3");
            Console.WriteLine($"  2x - y = 0");
            Console.WriteLine();
            Console.WriteLine($"Решение: x = {system.Solution}");
            Console.WriteLine();

            // Верификация с правильным решением
            if (system.Solution != null)
            {
                bool isValid = system.VerifySolution(system.Solution);
                Console.WriteLine($"Проверка решения через VerifySolution(): {isValid}");

                // Ручная проверка A*x
                var Ax = A * system.Solution;
                Console.WriteLine($"A * x = {Ax}");
                Console.WriteLine($"b     = {b}");
                Console.WriteLine($"A * x == b: {Ax == b}");
            }

            Console.WriteLine("\n2. ВЕРИФИКАЦИЯ НЕПРАВИЛЬНОГО РЕШЕНИЯ:\n");

            // Проверяем неправильный вектор
            var wrongSolution = new Vector<RationalNumber>(new RationalNumber(5), new RationalNumber(5));
            bool isWrongValid = system.VerifySolution(wrongSolution);
            Console.WriteLine($"Неправильное решение: x = {wrongSolution}");
            Console.WriteLine($"Проверка через VerifySolution(): {isWrongValid}");

            var Ax_wrong = A * wrongSolution;
            Console.WriteLine($"A * x_wrong = {Ax_wrong}");
            Console.WriteLine($"b           = {b}");
            Console.WriteLine($"A * x_wrong == b: {Ax_wrong == b}");
        }
    }
}
