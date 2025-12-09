using System;

namespace FieldAlgebra.Lab5
{
    class Program5
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== СИСТЕМЫ ЛИНЕЙНЫХ УРАВНЕНИЙ ===\n");

            // 1. Система с единственным решением
            Console.WriteLine("1. СИСТЕМА С ЕДИНСТВЕННЫМ РЕШЕНИЕМ");
            TestUniqueSolution();

            // 2. Несовместная система
            Console.WriteLine("\n2. НЕСОВМЕСТНАЯ СИСТЕМА");
            TestNoSolution();

            // 3. Система с бесконечным числом решений
            Console.WriteLine("\n3. СИСТЕМА С БЕСКОНЕЧНЫМ ЧИСЛОМ РЕШЕНИЙ");
            TestInfiniteSolutions();

            // 4. Случайные системы
            Console.WriteLine("\n4. СЛУЧАЙНЫЕ СИСТЕМЫ");
            TestRandomSystems();

            // 5. Граничные случаи
            Console.WriteLine("\n5. ГРАНИЧНЫЕ СЛУЧАИ");
            TestEdgeCases();

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void TestUniqueSolution()
        {
            // Создаем простую систему 2x2
            // 2x + y = 5
            // x + 2y = 4
            Vector<RationalNumber>[] matrixA = new Vector<RationalNumber>[]
            {
                new Vector<RationalNumber>(new RationalNumber(2, 1), new RationalNumber(1, 1)),
                new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(2, 1))
            };
            Vector<RationalNumber> vectorB = new Vector<RationalNumber>(
                new RationalNumber(5, 1),
                new RationalNumber(4, 1)
            );

            LinearSystem<RationalNumber> system = new LinearSystem<RationalNumber>(matrixA, vectorB);
            system.PrintSystem();

            SolutionType solutionType = system.GetSolutionType();
            Console.WriteLine($"Тип решения: {solutionType}");

            if (solutionType == SolutionType.UniqueSolution)
            {
                Vector<RationalNumber> solution = system.Solve();
                Console.WriteLine($"Решение: {solution}");
                Console.WriteLine($"Проверка решения: {(system.VerifySolution(solution) ? "✓ Верно" : "✗ Неверно")}");
            }
        }

        static void TestNoSolution()
        {
            // Несовместная система 2x2
            // x + y = 1
            // x + y = 2
            Vector<RationalNumber>[] matrixA = new Vector<RationalNumber>[]
            {
                new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(1, 1)),
                new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(1, 1))
            };
            Vector<RationalNumber> vectorB = new Vector<RationalNumber>(
                new RationalNumber(1, 1),
                new RationalNumber(2, 1)
            );

            LinearSystem<RationalNumber> system = new LinearSystem<RationalNumber>(matrixA, vectorB);
            system.PrintSystem();

            SolutionType solutionType = system.GetSolutionType();
            Console.WriteLine($"Тип решения: {solutionType}");

            try
            {
                Vector<RationalNumber> solution = system.Solve();
            }
            catch (LinearSystemException ex)
            {
                Console.WriteLine($"✓ {ex.Message}");
            }
        }

        static void TestInfiniteSolutions()
        {
            // Система с бесконечным числом решений (недоопределенная)
            // x + y = 2
            // 2x + 2y = 4
            Vector<RationalNumber>[] matrixA = new Vector<RationalNumber>[]
            {
                new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(1, 1)),
                new Vector<RationalNumber>(new RationalNumber(2, 1), new RationalNumber(2, 1))
            };
            Vector<RationalNumber> vectorB = new Vector<RationalNumber>(
                new RationalNumber(2, 1),
                new RationalNumber(4, 1)
            );

            LinearSystem<RationalNumber> system = new LinearSystem<RationalNumber>(matrixA, vectorB);
            system.PrintSystem();

            SolutionType solutionType = system.GetSolutionType();
            Console.WriteLine($"Тип решения: {solutionType}");

            try
            {
                Vector<RationalNumber> solution = system.Solve();
            }
            catch (LinearSystemException ex)
            {
                Console.WriteLine($"✓ {ex.Message}");
            }
        }

        static void TestRandomSystems()
        {
            // Случайная система с рациональными числами
            Console.WriteLine("Случайная система 3x3 с рациональными числами:");
            LinearSystem<RationalNumber> rationalSystem = LinearSystem<RationalNumber>.CreateWithUniqueSolution(3);
            rationalSystem.PrintSystem();
            Console.WriteLine($"Тип решения: {rationalSystem.GetSolutionType()}");

            try
            {
                Vector<RationalNumber> solution = rationalSystem.Solve();
                Console.WriteLine($"Решение: {solution}");
                Console.WriteLine($"Проверка: {(rationalSystem.VerifySolution(solution) ? "✓ Верно" : "✗ Неверно")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine();

            // Случайная система с комплексными числами
            Console.WriteLine("Случайная система 2x2 с комплексными числами:");
            LinearSystem<ComplexNumber> complexSystem = LinearSystem<ComplexNumber>.CreateWithUniqueSolution(2);
            complexSystem.PrintSystem();
            Console.WriteLine($"Тип решения: {complexSystem.GetSolutionType()}");

            try
            {
                Vector<ComplexNumber> solution = complexSystem.Solve();
                Console.WriteLine($"Решение: {solution}");
                Console.WriteLine($"Проверка: {(complexSystem.VerifySolution(solution) ? "✓ Верно" : "✗ Неверно")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void TestEdgeCases()
        {
            // Система 1x1
            Console.WriteLine("Система 1x1:");
            Vector<RationalNumber>[] matrix1x1 = new Vector<RationalNumber>[]
            {
                new Vector<RationalNumber>(new RationalNumber(2, 1))
            };
            Vector<RationalNumber> vector1x1 = new Vector<RationalNumber>(new RationalNumber(6, 1));

            LinearSystem<RationalNumber> system1x1 = new LinearSystem<RationalNumber>(matrix1x1, vector1x1);
            system1x1.PrintSystem();
            Console.WriteLine($"Тип решения: {system1x1.GetSolutionType()}");

            try
            {
                Vector<RationalNumber> solution = system1x1.Solve();
                Console.WriteLine($"Решение: {solution}");
                Console.WriteLine($"Проверка: {(system1x1.VerifySolution(solution) ? "✓ Верно" : "✗ Неверно")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine();

            // Большая система 5x5
            Console.WriteLine("Большая система 5x5:");
            LinearSystem<RationalNumber> largeSystem = LinearSystem<RationalNumber>.CreateWithUniqueSolution(5);
            largeSystem.PrintSystem();
            Console.WriteLine($"Тип решения: {largeSystem.GetSolutionType()}");

            try
            {
                Vector<RationalNumber> solution = largeSystem.Solve();
                Console.WriteLine($"Решение найдено, длина: {solution.Length}");
                Console.WriteLine($"Проверка: {(largeSystem.VerifySolution(solution) ? "✓ Верно" : "✗ Неверно")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine();

            // Переопределенная система (больше уравнений, чем неизвестных)
            Console.WriteLine("Переопределенная система 3x2:");
            Vector<RationalNumber>[] matrixOverdetermined = new Vector<RationalNumber>[]
            {
                new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(2, 1)),
                new Vector<RationalNumber>(new RationalNumber(2, 1), new RationalNumber(3, 1)),
                new Vector<RationalNumber>(new RationalNumber(3, 1), new RationalNumber(4, 1))
            };
            Vector<RationalNumber> vectorOverdetermined = new Vector<RationalNumber>(
                new RationalNumber(5, 1),
                new RationalNumber(8, 1),
                new RationalNumber(11, 1)
            );

            LinearSystem<RationalNumber> overdeterminedSystem = new LinearSystem<RationalNumber>(matrixOverdetermined, vectorOverdetermined);
            overdeterminedSystem.PrintSystem();
            Console.WriteLine($"Тип решения: {overdeterminedSystem.GetSolutionType()}");

            try
            {
                Vector<RationalNumber> solution = overdeterminedSystem.Solve();
                Console.WriteLine($"Решение: {solution}");
                Console.WriteLine($"Проверка: {(overdeterminedSystem.VerifySolution(solution) ? "✓ Верно" : "✗ Неверно")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✓ {ex.Message}");
            }

            Console.WriteLine();

            // Недоопределенная система (меньше уравнений, чем неизвестных)
            Console.WriteLine("Недоопределенная система 2x3:");
            Vector<RationalNumber>[] matrixUnderdetermined = new Vector<RationalNumber>[]
            {
                new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(2, 1), new RationalNumber(3, 1)),
                new Vector<RationalNumber>(new RationalNumber(4, 1), new RationalNumber(5, 1), new RationalNumber(6, 1))
            };
            Vector<RationalNumber> vectorUnderdetermined = new Vector<RationalNumber>(
                new RationalNumber(14, 1),
                new RationalNumber(32, 1)
            );

            LinearSystem<RationalNumber> underdeterminedSystem = new LinearSystem<RationalNumber>(matrixUnderdetermined, vectorUnderdetermined);
            underdeterminedSystem.PrintSystem();
            Console.WriteLine($"Тип решения: {underdeterminedSystem.GetSolutionType()}");

            try
            {
                Vector<RationalNumber> solution = underdeterminedSystem.Solve();
            }
            catch (LinearSystemException ex)
            {
                Console.WriteLine($"✓ {ex.Message}");
            }

            Console.WriteLine();

            // Система с нулевой правой частью
            Console.WriteLine("Система с нулевой правой частью:");
            Vector<RationalNumber>[] matrixZero = new Vector<RationalNumber>[]
            {
                new Vector<RationalNumber>(new RationalNumber(1, 1), new RationalNumber(2, 1)),
                new Vector<RationalNumber>(new RationalNumber(3, 1), new RationalNumber(4, 1))
            };
            Vector<RationalNumber> vectorZero = new Vector<RationalNumber>(
                RationalNumber.Zero,
                RationalNumber.Zero
            );

            LinearSystem<RationalNumber> zeroSystem = new LinearSystem<RationalNumber>(matrixZero, vectorZero);
            zeroSystem.PrintSystem();
            Console.WriteLine($"Тип решения: {zeroSystem.GetSolutionType()}");

            try
            {
                Vector<RationalNumber> solution = zeroSystem.Solve();
                Console.WriteLine($"Решение: {solution}");
                Console.WriteLine($"Проверка: {(zeroSystem.VerifySolution(solution) ? "✓ Верно" : "✗ Неверно")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
