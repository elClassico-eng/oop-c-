using System;
using System.Linq;
using System.Text;

namespace FieldInterface
{
    /// <summary>
    /// Представляет матрицу над математическим полем с элементами типа T.
    /// </summary>
    /// <typeparam name="T">Тип элементов поля, реализующий интерфейс IField&lt;T&gt;</typeparam>
    public class Matrix<T> : IEquatable<Matrix<T>> where T : IField<T>
    {
        private readonly Vector<T>[] rows;

        /// <summary>
        /// Получает количество строк матрицы.
        /// </summary>
        public int RowCount => rows.Length;

        /// <summary>
        /// Получает количество столбцов матрицы.
        /// </summary>
        public int ColumnCount { get; }

        /// <summary>
        /// Получает элемент матрицы по индексам строки и столбца (только для чтения).
        /// </summary>
        /// <param name="row">Индекс строки (от 0 до RowCount-1)</param>
        /// <param name="col">Индекс столбца (от 0 до ColumnCount-1)</param>
        /// <returns>Элемент матрицы</returns>
        /// <exception cref="IndexOutOfRangeException">Индекс выходит за пределы матрицы</exception>
        public T this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= RowCount)
                    throw new IndexOutOfRangeException($"Индекс строки {row} выходит за пределы матрицы {RowCount}×{ColumnCount}");
                if (col < 0 || col >= ColumnCount)
                    throw new IndexOutOfRangeException($"Индекс столбца {col} выходит за пределы матрицы {RowCount}×{ColumnCount}");

                return rows[row][col];
            }
        }

        /// <summary>
        /// Создает матрицу из массива векторов-строк.
        /// </summary>
        /// <param name="rows">Массив векторов-строк</param>
        /// <exception cref="ArgumentNullException">Массив строк равен null</exception>
        /// <exception cref="ArgumentException">Массив пуст, содержит null строки, или строки имеют разную размерность</exception>
        public Matrix(params Vector<T>[] rows)
        {
            if (rows == null)
                throw new ArgumentNullException(nameof(rows), "Массив строк не может быть null");

            if (rows.Length == 0)
                throw new ArgumentException("Матрица не может быть пустой", nameof(rows));

            // Проверка на null строки
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] == null)
                    throw new ArgumentException($"Строка {i} не может быть null", nameof(rows));
            }

            // Проверка одинаковой размерности всех строк
            int columnCount = rows[0].Dimension;
            for (int i = 1; i < rows.Length; i++)
            {
                if (rows[i].Dimension != columnCount)
                    throw new ArgumentException(
                        $"Все строки должны иметь одинаковую размерность. Строка 0 имеет размерность {columnCount}, строка {i} имеет размерность {rows[i].Dimension}",
                        nameof(rows));
            }

            // Defensive copy
            this.rows = new Vector<T>[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                this.rows[i] = new Vector<T>(rows[i].ToArray());
            }

            ColumnCount = columnCount;
        }

        /// <summary>
        /// Создает нулевую матрицу заданного размера.
        /// </summary>
        /// <param name="rowCount">Количество строк</param>
        /// <param name="columnCount">Количество столбцов</param>
        /// <exception cref="ArgumentException">Размеры меньше или равны нулю</exception>
        public Matrix(int rowCount, int columnCount)
        {
            if (rowCount <= 0)
                throw new ArgumentException("Количество строк должно быть положительным", nameof(rowCount));
            if (columnCount <= 0)
                throw new ArgumentException("Количество столбцов должно быть положительным", nameof(columnCount));

            rows = new Vector<T>[rowCount];
            for (int i = 0; i < rowCount; i++)
            {
                rows[i] = new Vector<T>(columnCount);
            }

            ColumnCount = columnCount;
        }

        /// <summary>
        /// Создает матрицу из двумерного массива.
        /// </summary>
        /// <param name="elements">Двумерный массив элементов</param>
        /// <exception cref="ArgumentNullException">Массив равен null</exception>
        /// <exception cref="ArgumentException">Массив пуст</exception>
        public Matrix(T[,] elements)
        {
            if (elements == null)
                throw new ArgumentNullException(nameof(elements), "Массив элементов не может быть null");

            int rowCount = elements.GetLength(0);
            int columnCount = elements.GetLength(1);

            if (rowCount == 0 || columnCount == 0)
                throw new ArgumentException("Матрица не может быть пустой", nameof(elements));

            rows = new Vector<T>[rowCount];
            for (int i = 0; i < rowCount; i++)
            {
                T[] rowElements = new T[columnCount];
                for (int j = 0; j < columnCount; j++)
                {
                    rowElements[j] = elements[i, j];
                }
                rows[i] = new Vector<T>(rowElements);
            }

            ColumnCount = columnCount;
        }

        /// <summary>
        /// Создает единичную матрицу заданного размера.
        /// </summary>
        /// <param name="size">Размер матрицы (size × size)</param>
        /// <returns>Единичная матрица</returns>
        /// <exception cref="ArgumentException">Размер меньше или равен нулю</exception>
        public static Matrix<T> Identity(int size)
        {
            if (size <= 0)
                throw new ArgumentException("Размер должен быть положительным", nameof(size));

            T[,] elements = new T[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    elements[i, j] = (i == j) ? T.One : T.Zero;
                }
            }

            return new Matrix<T>(elements);
        }

        /// <summary>
        /// Получает строку матрицы по индексу.
        /// </summary>
        /// <param name="index">Индекс строки</param>
        /// <returns>Вектор-строка</returns>
        /// <exception cref="IndexOutOfRangeException">Индекс выходит за пределы</exception>
        public Vector<T> GetRow(int index)
        {
            if (index < 0 || index >= RowCount)
                throw new IndexOutOfRangeException($"Индекс строки {index} выходит за пределы матрицы {RowCount}×{ColumnCount}");

            return new Vector<T>(rows[index].ToArray());
        }

        /// <summary>
        /// Получает столбец матрицы по индексу.
        /// </summary>
        /// <param name="index">Индекс столбца</param>
        /// <returns>Вектор-столбец</returns>
        /// <exception cref="IndexOutOfRangeException">Индекс выходит за пределы</exception>
        public Vector<T> GetColumn(int index)
        {
            if (index < 0 || index >= ColumnCount)
                throw new IndexOutOfRangeException($"Индекс столбца {index} выходит за пределы матрицы {RowCount}×{ColumnCount}");

            T[] columnElements = new T[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                columnElements[i] = rows[i][index];
            }

            return new Vector<T>(columnElements);
        }

        /// <summary>
        /// Создает копию матрицы.
        /// </summary>
        /// <returns>Копия матрицы</returns>
        public Matrix<T> Clone()
        {
            Vector<T>[] clonedRows = new Vector<T>[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                clonedRows[i] = new Vector<T>(rows[i].ToArray());
            }
            return new Matrix<T>(clonedRows);
        }

        /// <summary>
        /// Возвращает двумерный массив элементов матрицы.
        /// </summary>
        /// <returns>Двумерный массив</returns>
        public T[,] ToArray()
        {
            T[,] result = new T[RowCount, ColumnCount];
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    result[i, j] = rows[i][j];
                }
            }
            return result;
        }

        /// <summary>
        /// Транспонирует матрицу.
        /// </summary>
        /// <returns>Транспонированная матрица</returns>
        public Matrix<T> Transpose()
        {
            T[,] transposed = new T[ColumnCount, RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    transposed[j, i] = rows[i][j];
                }
            }
            return new Matrix<T>(transposed);
        }

        /// <summary>
        /// Создает расширенную матрицу [A|b], добавляя столбец справа.
        /// </summary>
        /// <param name="column">Вектор-столбец для добавления</param>
        /// <returns>Расширенная матрица</returns>
        /// <exception cref="ArgumentNullException">Столбец равен null</exception>
        /// <exception cref="ArgumentException">Размерность столбца не совпадает с количеством строк</exception>
        public Matrix<T> AugmentWith(Vector<T> column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column), "Столбец не может быть null");

            if (column.Dimension != RowCount)
                throw new ArgumentException(
                    $"Размерность столбца {column.Dimension} должна совпадать с количеством строк {RowCount}",
                    nameof(column));

            T[,] augmented = new T[RowCount, ColumnCount + 1];
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    augmented[i, j] = rows[i][j];
                }
                augmented[i, ColumnCount] = column[i];
            }

            return new Matrix<T>(augmented);
        }

        /// <summary>
        /// Вычисляет ранг матрицы методом Гаусса.
        /// </summary>
        /// <returns>Ранг матрицы</returns>
        public int Rank()
        {
            // Работаем с копией, чтобы не изменять оригинал
            Matrix<T> working = Clone();

            int rank = 0;
            int currentRow = 0;

            for (int col = 0; col < ColumnCount && currentRow < RowCount; col++)
            {
                // Найти ведущий элемент
                int pivotRow = working.FindPivotRow(currentRow, col);

                if (working[pivotRow, col] == T.Zero)
                    continue; // Нет ненулевого элемента в этом столбце

                // Поменять строки местами
                if (pivotRow != currentRow)
                    working.SwapRows(currentRow, pivotRow);

                // Обнулить элементы ниже ведущего
                for (int row = currentRow + 1; row < RowCount; row++)
                {
                    if (working[row, col] != T.Zero)
                    {
                        T factor = working[row, col] / working[currentRow, col];
                        working.AddScaledRow(currentRow, row, -factor);
                    }
                }

                rank++;
                currentRow++;
            }

            return rank;
        }

        // ============================================
        // ПРИВАТНЫЕ МЕТОДЫ ДЛЯ МЕТОДА ГАУССА
        // ============================================

        private void SwapRows(int row1, int row2)
        {
            var temp = rows[row1];
            rows[row1] = rows[row2];
            rows[row2] = temp;
        }

        private void ScaleRow(int rowIndex, T scalar)
        {
            if (scalar == null)
                throw new ArgumentNullException(nameof(scalar));

            T[] scaledElements = new T[ColumnCount];
            for (int j = 0; j < ColumnCount; j++)
            {
                scaledElements[j] = scalar * rows[rowIndex][j];
            }
            rows[rowIndex] = new Vector<T>(scaledElements);
        }

        private void AddScaledRow(int sourceRow, int targetRow, T scalar)
        {
            if (scalar == null)
                throw new ArgumentNullException(nameof(scalar));

            T[] newElements = new T[ColumnCount];
            for (int j = 0; j < ColumnCount; j++)
            {
                newElements[j] = rows[targetRow][j] + scalar * rows[sourceRow][j];
            }
            rows[targetRow] = new Vector<T>(newElements);
        }

        private int FindPivotRow(int startRow, int column)
        {
            // Ищем первый ненулевой элемент в столбце, начиная со startRow
            for (int i = startRow; i < RowCount; i++)
            {
                if (rows[i][column] != T.Zero)
                    return i;
            }
            return startRow; // Если не нашли ненулевой, возвращаем стартовую строку
        }

        // ============================================
        // ОПЕРАТОРЫ
        // ============================================

        /// <summary>
        /// Оператор сложения матриц.
        /// </summary>
        public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left), "Левая матрица не может быть null");
            if (right == null)
                throw new ArgumentNullException(nameof(right), "Правая матрица не может быть null");

            if (left.RowCount != right.RowCount || left.ColumnCount != right.ColumnCount)
                throw new ArgumentException(
                    $"Размеры матриц не совпадают: {left.RowCount}×{left.ColumnCount} != {right.RowCount}×{right.ColumnCount}");

            T[,] result = new T[left.RowCount, left.ColumnCount];
            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.ColumnCount; j++)
                {
                    result[i, j] = left[i, j] + right[i, j];
                }
            }

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Оператор вычитания матриц.
        /// </summary>
        public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left), "Левая матрица не может быть null");
            if (right == null)
                throw new ArgumentNullException(nameof(right), "Правая матрица не может быть null");

            if (left.RowCount != right.RowCount || left.ColumnCount != right.ColumnCount)
                throw new ArgumentException(
                    $"Размеры матриц не совпадают: {left.RowCount}×{left.ColumnCount} != {right.RowCount}×{right.ColumnCount}");

            T[,] result = new T[left.RowCount, left.ColumnCount];
            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.ColumnCount; j++)
                {
                    result[i, j] = left[i, j] - right[i, j];
                }
            }

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Оператор умножения матриц.
        /// </summary>
        public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left), "Левая матрица не может быть null");
            if (right == null)
                throw new ArgumentNullException(nameof(right), "Правая матрица не может быть null");

            if (left.ColumnCount != right.RowCount)
                throw new ArgumentException(
                    $"Количество столбцов левой матрицы ({left.ColumnCount}) должно равняться количеству строк правой матрицы ({right.RowCount})");

            T[,] result = new T[left.RowCount, right.ColumnCount];
            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < right.ColumnCount; j++)
                {
                    T sum = T.Zero;
                    for (int k = 0; k < left.ColumnCount; k++)
                    {
                        sum = sum + left[i, k] * right[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Оператор умножения матрицы на вектор.
        /// </summary>
        public static Vector<T> operator *(Matrix<T> matrix, Vector<T> vector)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), "Матрица не может быть null");
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), "Вектор не может быть null");

            if (matrix.ColumnCount != vector.Dimension)
                throw new ArgumentException(
                    $"Количество столбцов матрицы ({matrix.ColumnCount}) должно равняться размерности вектора ({vector.Dimension})");

            T[] result = new T[matrix.RowCount];
            for (int i = 0; i < matrix.RowCount; i++)
            {
                T sum = T.Zero;
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    sum = sum + matrix[i, j] * vector[j];
                }
                result[i] = sum;
            }

            return new Vector<T>(result);
        }

        /// <summary>
        /// Оператор равенства матриц.
        /// </summary>
        public static bool operator ==(Matrix<T> left, Matrix<T> right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;

            if (left.RowCount != right.RowCount || left.ColumnCount != right.ColumnCount)
                return false;

            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.ColumnCount; j++)
                {
                    if (left[i, j] != right[i, j])
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Оператор неравенства матриц.
        /// </summary>
        public static bool operator !=(Matrix<T> left, Matrix<T> right)
        {
            return !(left == right);
        }

        // ============================================
        // МЕТОДЫ OBJECT
        // ============================================

        /// <summary>
        /// Возвращает строковое представление матрицы.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < RowCount; i++)
            {
                sb.Append("[");
                for (int j = 0; j < ColumnCount; j++)
                {
                    sb.Append(rows[i][j].ToString());
                    if (j < ColumnCount - 1)
                        sb.Append("  ");
                }
                sb.Append("]");
                if (i < RowCount - 1)
                    sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Проверяет равенство с другой матрицей.
        /// </summary>
        public bool Equals(Matrix<T> other)
        {
            return this == other;
        }

        /// <summary>
        /// Проверяет равенство с объектом.
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as Matrix<T>);
        }

        /// <summary>
        /// Вычисляет хэш-код матрицы.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + RowCount.GetHashCode();
                hash = hash * 31 + ColumnCount.GetHashCode();

                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColumnCount; j++)
                    {
                        if (rows[i][j] != null)
                            hash = hash * 31 + rows[i][j].GetHashCode();
                    }
                }

                return hash;
            }
        }
    }
}
