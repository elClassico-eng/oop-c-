using System;
using System.Text;

namespace FieldInterface
{
    /// <summary>
    /// Представляет систему линейных уравнений Ax = b над математическим полем.
    /// </summary>
    /// <typeparam name="T">Тип элементов поля, реализующий интерфейс IField&lt;T&gt;</typeparam>
    public class LinearSystem<T> where T : IField<T>
    {
        /// <summary>
        /// Получает матрицу коэффициентов A.
        /// </summary>
        public Matrix<T> CoefficientMatrix { get; }

        /// <summary>
        /// Получает вектор констант b.
        /// </summary>
        public Vector<T> ConstantVector { get; }

        /// <summary>
        /// Получает количество неизвестных (n - количество столбцов в A).
        /// </summary>
        public int VariableCount { get; }

        /// <summary>
        /// Получает количество уравнений (m - количество строк в A).
        /// </summary>
        public int EquationCount { get; }

        /// <summary>
        /// Получает тип решения системы.
        /// </summary>
        public SolutionType SolutionType { get; private set; }

        /// <summary>
        /// Получает решение системы (null, если решения нет или их бесконечно много).
        /// </summary>
        public Vector<T>? Solution { get; private set; }

        /// <summary>
        /// Создает систему линейных уравнений из матрицы коэффициентов и вектора констант.
        /// </summary>
        /// <param name="coefficients">Матрица коэффициентов A</param>
        /// <param name="constants">Вектор констант b</param>
        /// <exception cref="ArgumentNullException">Один из параметров равен null</exception>
        /// <exception cref="ArgumentException">Размерность вектора не совпадает с количеством строк матрицы</exception>
        public LinearSystem(Matrix<T> coefficients, Vector<T> constants)
        {
            if (coefficients == null)
                throw new ArgumentNullException(nameof(coefficients), "Матрица коэффициентов не может быть null");
            if (constants == null)
                throw new ArgumentNullException(nameof(constants), "Вектор констант не может быть null");

            if (coefficients.RowCount != constants.Dimension)
                throw new ArgumentException(
                    $"Количество строк матрицы ({coefficients.RowCount}) должно равняться размерности вектора констант ({constants.Dimension})");

            CoefficientMatrix = coefficients;
            ConstantVector = constants;
            VariableCount = coefficients.ColumnCount;
            EquationCount = coefficients.RowCount;
            SolutionType = SolutionType.NoSolution; // По умолчанию
            Solution = null;
        }

        /// <summary>
        /// Создает систему из массива векторов-строк уравнений и вектора констант.
        /// </summary>
        /// <param name="equations">Массив векторов-строк коэффициентов</param>
        /// <param name="constants">Вектор констант</param>
        public LinearSystem(Vector<T>[] equations, Vector<T> constants)
            : this(new Matrix<T>(equations), constants)
        {
        }

        /// <summary>
        /// Генерирует случайную систему линейных уравнений.
        /// </summary>
        /// <param name="equations">Количество уравнений</param>
        /// <param name="variables">Количество неизвестных</param>
        /// <returns>Случайная система</returns>
        public static LinearSystem<T> GenerateRandom(int equations, int variables)
        {
            if (equations <= 0)
                throw new ArgumentException("Количество уравнений должно быть положительным", nameof(equations));
            if (variables <= 0)
                throw new ArgumentException("Количество неизвестных должно быть положительным", nameof(variables));

            Vector<T>[] coefficientRows = new Vector<T>[equations];
            for (int i = 0; i < equations; i++)
            {
                coefficientRows[i] = Vector<T>.GenerateRandom(variables);
            }

            Vector<T> constants = Vector<T>.GenerateRandom(equations);

            return new LinearSystem<T>(new Matrix<T>(coefficientRows), constants);
        }

        /// <summary>
        /// Анализирует тип решения системы на основе рангов матриц.
        /// </summary>
        /// <returns>Тип решения</returns>
        public SolutionType AnalyzeSolutionType()
        {
            int rankA = CoefficientMatrix.Rank();
            Matrix<T> augmented = CoefficientMatrix.AugmentWith(ConstantVector);
            int rankAugmented = augmented.Rank();

            if (rankA < rankAugmented)
                return SolutionType.NoSolution;

            if (rankA == rankAugmented && rankA == VariableCount)
                return SolutionType.UniqueSolution;

            return SolutionType.InfiniteSolutions;
        }

        /// <summary>
        /// Решает систему линейных уравнений методом Гаусса с обратной подстановкой.
        /// </summary>
        public void Solve()
        {
            // Анализируем тип решения
            SolutionType = AnalyzeSolutionType();

            if (SolutionType == SolutionType.NoSolution)
            {
                Solution = null;
                return;
            }

            if (SolutionType == SolutionType.InfiniteSolutions)
            {
                Solution = null;
                return;
            }

            // Решаем систему с единственным решением
            Solution = SolveUniqueSystem();
        }

        /// <summary>
        /// Проверяет, является ли заданный вектор решением системы.
        /// </summary>
        /// <param name="candidate">Вектор-кандидат</param>
        /// <returns>true, если A*x = b; иначе false</returns>
        public bool VerifySolution(Vector<T> candidate)
        {
            if (candidate == null)
                return false;

            if (candidate.Dimension != VariableCount)
                return false;

            Vector<T> result = CoefficientMatrix * candidate;
            return result == ConstantVector;
        }

        // ============================================
        // ПРИВАТНЫЕ МЕТОДЫ РЕШЕНИЯ
        // ============================================

        private Vector<T> SolveUniqueSystem()
        {
            // Создаем расширенную матрицу [A|b]
            Matrix<T> augmented = CoefficientMatrix.AugmentWith(ConstantVector);

            // Прямой ход Гаусса (приведение к треугольному виду)
            augmented = PerformForwardElimination(augmented);

            // Обратная подстановка
            return PerformBackSubstitution(augmented);
        }

        private Matrix<T> PerformForwardElimination(Matrix<T> matrix)
        {
            Matrix<T> working = matrix.Clone();

            for (int col = 0; col < VariableCount; col++)
            {
                // Найти ведущий элемент (partial pivoting)
                int pivotRow = FindPivotRow(working, col, col);

                if (working[pivotRow, col] == T.Zero)
                    continue; // Не должно произойти для системы с единственным решением

                // Поменять строки местами, если нужно
                if (pivotRow != col)
                    working.SwapRows(col, pivotRow);

                // Нормализовать ведущую строку (сделать ведущий элемент равным 1)
                T pivot = working[col, col];
                working.ScaleRow(col, pivot.Inverse);

                // Обнулить элементы ниже ведущего
                for (int row = col + 1; row < EquationCount; row++)
                {
                    T factor = working[row, col];
                    if (factor != T.Zero)
                    {
                        working.AddScaledRow(col, row, -factor);
                    }
                }
            }

            return working;
        }

        private Vector<T> PerformBackSubstitution(Matrix<T> echelonForm)
        {
            T[] solution = new T[VariableCount];

            // Инициализация нулями
            for (int i = 0; i < VariableCount; i++)
            {
                solution[i] = T.Zero;
            }

            // Обратная подстановка снизу вверх
            for (int i = VariableCount - 1; i >= 0; i--)
            {
                // Начинаем со свободного члена из расширенной части
                T sum = echelonForm[i, VariableCount];

                // Вычитаем вклад уже найденных переменных
                for (int j = i + 1; j < VariableCount; j++)
                {
                    sum = sum - echelonForm[i, j] * solution[j];
                }

                // Делим на коэффициент при текущей переменной
                // (должен быть 1 после нормализации, но на всякий случай делим)
                T coefficient = echelonForm[i, i];
                if (coefficient != T.Zero)
                {
                    solution[i] = sum / coefficient;
                }
                else
                {
                    solution[i] = sum; // Если уже нормализовано
                }
            }

            return new Vector<T>(solution);
        }

        private int FindPivotRow(Matrix<T> matrix, int startRow, int column)
        {
            // Ищем первый ненулевой элемент в столбце, начиная со startRow
            for (int i = startRow; i < matrix.RowCount; i++)
            {
                if (matrix[i, column] != T.Zero)
                    return i;
            }
            return startRow;
        }

        // ============================================
        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ДЛЯ Matrix
        // (используем приватные методы через расширения)
        // ============================================

        // Эти методы используют приватные методы Matrix через публичный Clone
        private static class MatrixHelper
        {
            public static void SwapRows(Matrix<T> matrix, int row1, int row2)
            {
                // Используем рефлексию для вызова приватного метода
                var method = typeof(Matrix<T>).GetMethod("SwapRows",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                method?.Invoke(matrix, new object[] { row1, row2 });
            }

            public static void ScaleRow(Matrix<T> matrix, int rowIndex, T scalar)
            {
                var method = typeof(Matrix<T>).GetMethod("ScaleRow",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                method?.Invoke(matrix, new object[] { rowIndex, scalar });
            }

            public static void AddScaledRow(Matrix<T> matrix, int sourceRow, int targetRow, T scalar)
            {
                var method = typeof(Matrix<T>).GetMethod("AddScaledRow",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                method?.Invoke(matrix, new object[] { sourceRow, targetRow, scalar });
            }
        }

        // ============================================
        // МЕТОДЫ ОТОБРАЖЕНИЯ
        // ============================================

        /// <summary>
        /// Возвращает строковое представление системы уравнений.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Система линейных уравнений:");

            for (int i = 0; i < EquationCount; i++)
            {
                for (int j = 0; j < VariableCount; j++)
                {
                    T coef = CoefficientMatrix[i, j];
                    if (j > 0)
                    {
                        sb.Append(" + ");
                    }
                    sb.Append($"({coef})*x{j + 1}");
                }
                sb.Append($" = {ConstantVector[i]}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Возвращает подробное строковое представление с анализом решения.
        /// </summary>
        public string ToDetailedString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("═══════════════════════════════════════════════════════════");
            sb.AppendLine("              СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ");
            sb.AppendLine("═══════════════════════════════════════════════════════════");
            sb.AppendLine();

            // Вывод системы
            for (int i = 0; i < EquationCount; i++)
            {
                sb.Append("  ");
                for (int j = 0; j < VariableCount; j++)
                {
                    T coef = CoefficientMatrix[i, j];
                    if (j > 0)
                    {
                        sb.Append(" + ");
                    }
                    sb.Append($"({coef})*x{j + 1}");
                }
                sb.Append($" = {ConstantVector[i]}");
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine("───────────────────────────────────────────────────────────");
            sb.AppendLine("                    МАТРИЦЫ И РАНГИ");
            sb.AppendLine("───────────────────────────────────────────────────────────");
            sb.AppendLine();

            sb.AppendLine($"Матрица коэффициентов A ({EquationCount}×{VariableCount}):");
            sb.AppendLine(CoefficientMatrix.ToString());
            sb.AppendLine();

            sb.AppendLine("Вектор констант b:");
            sb.AppendLine($"  {ConstantVector}");
            sb.AppendLine();

            int rankA = CoefficientMatrix.Rank();
            Matrix<T> augmented = CoefficientMatrix.AugmentWith(ConstantVector);
            int rankAugmented = augmented.Rank();

            sb.AppendLine($"Ранг матрицы A:               rank(A)     = {rankA}");
            sb.AppendLine($"Ранг расширенной матрицы:     rank([A|b]) = {rankAugmented}");
            sb.AppendLine($"Количество неизвестных:       n           = {VariableCount}");
            sb.AppendLine();

            sb.AppendLine("───────────────────────────────────────────────────────────");
            sb.AppendLine("                      ТИП РЕШЕНИЯ");
            sb.AppendLine("───────────────────────────────────────────────────────────");
            sb.AppendLine();

            string solutionTypeStr = SolutionType switch
            {
                SolutionType.UniqueSolution => "ЕДИНСТВЕННОЕ РЕШЕНИЕ",
                SolutionType.InfiniteSolutions => "БЕСКОНЕЧНО МНОГО РЕШЕНИЙ",
                SolutionType.NoSolution => "НЕТ РЕШЕНИЙ (система несовместна)",
                _ => "НЕИЗВЕСТНО"
            };

            sb.AppendLine($"  {solutionTypeStr}");
            sb.AppendLine();

            string condition = SolutionType switch
            {
                SolutionType.UniqueSolution => $"rank(A) = rank([A|b]) = n  ({rankA} = {rankAugmented} = {VariableCount})",
                SolutionType.InfiniteSolutions => $"rank(A) = rank([A|b]) < n  ({rankA} = {rankAugmented} < {VariableCount})",
                SolutionType.NoSolution => $"rank(A) < rank([A|b])  ({rankA} < {rankAugmented})",
                _ => "???"
            };

            sb.AppendLine($"  Условие: {condition}");

            if (Solution != null)
            {
                sb.AppendLine();
                sb.AppendLine("───────────────────────────────────────────────────────────");
                sb.AppendLine("                        РЕШЕНИЕ");
                sb.AppendLine("───────────────────────────────────────────────────────────");
                sb.AppendLine();
                sb.AppendLine($"  x = {Solution}");
                sb.AppendLine();

                // Проверка решения
                if (VerifySolution(Solution))
                {
                    sb.AppendLine("  ✓ Проверка: A*x = b");
                    Vector<T> Ax = CoefficientMatrix * Solution;
                    sb.AppendLine($"    A*x = {Ax}");
                    sb.AppendLine($"    b   = {ConstantVector}");
                }
            }

            sb.AppendLine("═══════════════════════════════════════════════════════════");

            return sb.ToString();
        }
    }

    // Расширения для Matrix для доступа к приватным методам
    internal static class MatrixExtensions
    {
        public static void SwapRows<T>(this Matrix<T> matrix, int row1, int row2) where T : IField<T>
        {
            var method = typeof(Matrix<T>).GetMethod("SwapRows",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(matrix, new object[] { row1, row2 });
        }

        public static void ScaleRow<T>(this Matrix<T> matrix, int rowIndex, T scalar) where T : IField<T>
        {
            var method = typeof(Matrix<T>).GetMethod("ScaleRow",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(matrix, new object[] { rowIndex, scalar });
        }

        public static void AddScaledRow<T>(this Matrix<T> matrix, int sourceRow, int targetRow, T scalar) where T : IField<T>
        {
            var method = typeof(Matrix<T>).GetMethod("AddScaledRow",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(matrix, new object[] { sourceRow, targetRow, scalar });
        }
    }
}
