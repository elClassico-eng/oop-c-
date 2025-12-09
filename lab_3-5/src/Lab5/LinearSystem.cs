using System;
using System.Linq;

namespace FieldAlgebra.Lab5
{
    public class LinearSystemException : Exception
    {
        public LinearSystemException(string message) : base(message) { }
    }

    public class LinearSystem<T> where T : IField<T>
    {
        private Vector<T>[] matrixA;
        private Vector<T> vectorB;

        public int Rows => matrixA.Length;
        public int Columns => matrixA[0].Length;

        public LinearSystem(Vector<T>[] matrixA, Vector<T> vectorB)
        {
            ValidateSystem(matrixA, vectorB);
            this.matrixA = matrixA;
            this.vectorB = vectorB;
        }

        private void ValidateSystem(Vector<T>[] matrixA, Vector<T> vectorB)
        {
            if (matrixA == null || matrixA.Length == 0)
                throw new ArgumentException("Матрица не может быть пустой", nameof(matrixA));

            if (vectorB == null)
                throw new ArgumentException("Вектор свободных членов не может быть null", nameof(vectorB));

            int variables = matrixA[0].Length;
            for (int i = 0; i < matrixA.Length; i++)
            {
                if (matrixA[i].Length != variables)
                    throw new ArgumentException("Все строки матрицы должны иметь одинаковую размерность");
            }

            if (vectorB.Length != matrixA.Length)
                throw new ArgumentException("Размерность вектора свободных членов должна совпадать с количеством уравнений");
        }

        public SolutionType GetSolutionType()
        {
            int rankA = GetMatrixRank(matrixA);
            int rankAB = GetAugmentedMatrixRank();

            if (rankA != rankAB)
                return SolutionType.NoSolution;
            else if (rankA == Columns)
                return SolutionType.UniqueSolution;
            else
                return SolutionType.InfiniteSolutions;
        }

        public Vector<T> Solve()
        {
            SolutionType solutionType = GetSolutionType();
            if (solutionType == SolutionType.NoSolution)
                throw new LinearSystemException("Система не имеет решений");

            if (solutionType == SolutionType.InfiniteSolutions)
                throw new LinearSystemException("Система имеет бесконечное множество решений");

            // Создаем копии для преобразований
            Vector<T>[] aCopy = matrixA.Select(v => v.Clone()).ToArray();
            Vector<T> bCopy = vectorB.Clone();

            // Прямой ход метода Гаусса
            for (int i = 0; i < Math.Min(Columns, Rows); i++)
            {
                // Поиск ведущего элемента
                int pivotRow = i;
                T maxVal = aCopy[i][i];

                for (int j = i + 1; j < Rows; j++)
                {
                    T currentVal = aCopy[j][i];
                    if (CompareAbsolute(currentVal, maxVal) > 0)
                    {
                        maxVal = currentVal;
                        pivotRow = j;
                    }
                }

                if (maxVal.IsZero)
                    continue;

                // Перестановка строк
                if (pivotRow != i)
                {
                    (aCopy[i], aCopy[pivotRow]) = (aCopy[pivotRow], aCopy[i]);
                    (bCopy[i], bCopy[pivotRow]) = (bCopy[pivotRow], bCopy[i]);
                }

                // Нормализация ведущей строки
                T pivot = aCopy[i][i];
                for (int j = i; j < Columns; j++)
                {
                    aCopy[i][j] = aCopy[i][j] / pivot;
                }
                bCopy[i] = bCopy[i] / pivot;

                // Исключение переменной
                for (int j = i + 1; j < Rows; j++)
                {
                    T factor = aCopy[j][i];
                    for (int k = i; k < Columns; k++)
                    {
                        aCopy[j][k] = aCopy[j][k] - factor * aCopy[i][k];
                    }
                    bCopy[j] = bCopy[j] - factor * bCopy[i];
                }
            }

            // Обратный ход метода Гаусса
            Vector<T> solution = new Vector<T>(Columns);
            for (int i = Math.Min(Columns, Rows) - 1; i >= 0; i--)
            {
                if (aCopy[i][i].IsZero)
                    continue;

                solution[i] = bCopy[i];
                for (int j = i + 1; j < Columns; j++)
                {
                    solution[i] = solution[i] - aCopy[i][j] * solution[j];
                }
            }

            return solution;
        }

        private int CompareAbsolute(T a, T b)
        {
            double absA = Math.Abs(a.ToDouble());
            double absB = Math.Abs(b.ToDouble());
            return absA.CompareTo(absB);
        }

        private int GetMatrixRank(Vector<T>[] matrix)
        {
            if (matrix.Length == 0) return 0;

            Vector<T>[] tempMatrix = matrix.Select(v => v.Clone()).ToArray();
            int rows = tempMatrix.Length;
            int cols = tempMatrix[0].Length;
            int rank = 0;

            for (int col = 0; col < cols && rank < rows; col++)
            {
                // Поиск ненулевого элемента в текущем столбце
                int pivotRow = -1;
                for (int row = rank; row < rows; row++)
                {
                    if (!tempMatrix[row][col].IsZero)
                    {
                        pivotRow = row;
                        break;
                    }
                }

                if (pivotRow == -1) continue;

                // Перестановка строк
                if (pivotRow != rank)
                {
                    (tempMatrix[rank], tempMatrix[pivotRow]) = (tempMatrix[pivotRow], tempMatrix[rank]);
                }

                // Нормализация ведущей строки
                T pivot = tempMatrix[rank][col];
                for (int j = col; j < cols; j++)
                {
                    tempMatrix[rank][j] = tempMatrix[rank][j] / pivot;
                }

                // Исключение
                for (int i = 0; i < rows; i++)
                {
                    if (i != rank && !tempMatrix[i][col].IsZero)
                    {
                        T factor = tempMatrix[i][col];
                        for (int j = col; j < cols; j++)
                        {
                            tempMatrix[i][j] = tempMatrix[i][j] - factor * tempMatrix[rank][j];
                        }
                    }
                }
                rank++;
            }

            return rank;
        }

        private int GetAugmentedMatrixRank()
        {
            Vector<T>[] augmented = new Vector<T>[Rows];
            for (int i = 0; i < Rows; i++)
            {
                T[] augmentedRow = new T[Columns + 1];
                for (int j = 0; j < Columns; j++)
                {
                    augmentedRow[j] = matrixA[i][j];
                }
                augmentedRow[Columns] = vectorB[i];
                augmented[i] = new Vector<T>(augmentedRow);
            }

            return GetMatrixRank(augmented);
        }

        public void PrintSystem()
        {
            Console.WriteLine("Система уравнений:");
            for (int i = 0; i < Rows; i++)
            {
                string equation = "";
                for (int j = 0; j < Columns; j++)
                {
                    if (j > 0 && !matrixA[i][j].IsZero)
                    {
                        double val = matrixA[i][j].ToDouble();
                        equation += val >= 0 ? " + " : " - ";
                        equation += $"{Math.Abs(val):F2}*x{j + 1}";
                    }
                    else if (j == 0)
                    {
                        equation += $"{matrixA[i][j].ToDouble():F2}*x{j + 1}";
                    }
                }
                equation += $" = {vectorB[i].ToDouble():F2}";
                Console.WriteLine($"  {equation}");
            }
        }

        public bool VerifySolution(Vector<T> solution)
        {
            if (solution.Length != Columns)
                throw new ArgumentException("Размерность решения должна совпадать с количеством переменных");

            const double tolerance = 1e-6;

            for (int i = 0; i < Rows; i++)
            {
                T leftSide = T.Zero;
                for (int j = 0; j < Columns; j++)
                {
                    leftSide = leftSide + matrixA[i][j] * solution[j];
                }
                T error = leftSide - vectorB[i];

                if (Math.Abs(error.ToDouble()) > tolerance)
                {
                    return false;
                }
            }
            return true;
        }

        public static LinearSystem<T> GenerateRandom(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Количество строк и столбцов должно быть положительным");

            Vector<T>[] matrixA = new Vector<T>[rows];
            Vector<T> vectorB = Vector<T>.Random(rows);

            for (int i = 0; i < rows; i++)
            {
                matrixA[i] = Vector<T>.Random(cols);
            }

            return new LinearSystem<T>(matrixA, vectorB);
        }

        public static LinearSystem<T> CreateWithUniqueSolution(int size)
        {
            Vector<T>[] matrixA = new Vector<T>[size];
            Vector<T> vectorB = Vector<T>.Random(size);

            // Создаем диагонально-доминантную матрицу
            for (int i = 0; i < size; i++)
            {
                T[] row = new T[size];
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        row[j] = T.FromDouble(5.0);
                    }
                    else
                    {
                        row[j] = T.FromDouble(0.5);
                    }
                }
                matrixA[i] = new Vector<T>(row);
            }

            return new LinearSystem<T>(matrixA, vectorB);
        }
    }
}
