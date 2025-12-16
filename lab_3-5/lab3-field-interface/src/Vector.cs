using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FieldInterface
{
    /// <summary>
    /// Представляет вектор над математическим полем с компонентами типа T.
    /// </summary>
    /// <typeparam name="T">Тип элементов поля, реализующий интерфейс IField&lt;T&gt;</typeparam>
    public class Vector<T> : IEquatable<Vector<T>> where T : IField<T>
    {
        private readonly T[] components;

        /// <summary>
        /// Получает размерность вектора.
        /// </summary>
        public int Dimension => components.Length;

        /// <summary>
        /// Получает компоненту вектора по индексу (только для чтения).
        /// </summary>
        /// <param name="index">Индекс компоненты (от 0 до Dimension-1)</param>
        /// <returns>Компонента вектора</returns>
        /// <exception cref="IndexOutOfRangeException">Индекс выходит за пределы массива</exception>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= components.Length)
                    throw new IndexOutOfRangeException($"Индекс {index} выходит за пределы вектора размерности {Dimension}");
                return components[index];
            }
        }

        /// <summary>
        /// Создает вектор из массива компонент.
        /// </summary>
        /// <param name="components">Массив компонент вектора</param>
        /// <exception cref="ArgumentNullException">Массив компонент равен null</exception>
        /// <exception cref="ArgumentException">Массив пуст или содержит null компоненты</exception>
        public Vector(params T[] components)
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components), "Массив компонент не может быть null");

            if (components.Length == 0)
                throw new ArgumentException("Вектор не может быть пустым", nameof(components));

            // Проверка на null компоненты
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                    throw new ArgumentException($"Компонента {i} не может быть null", nameof(components));
            }

            // Defensive copy
            this.components = new T[components.Length];
            Array.Copy(components, this.components, components.Length);
        }

        /// <summary>
        /// Создает нулевой вектор заданной размерности.
        /// </summary>
        /// <param name="dimension">Размерность вектора</param>
        /// <exception cref="ArgumentException">Размерность меньше или равна нулю</exception>
        public Vector(int dimension)
        {
            if (dimension <= 0)
                throw new ArgumentException("Размерность должна быть положительной", nameof(dimension));

            components = new T[dimension];
            for (int i = 0; i < dimension; i++)
            {
                components[i] = T.Zero;
            }
        }

        /// <summary>
        /// Оператор сложения векторов.
        /// </summary>
        /// <param name="left">Первый вектор</param>
        /// <param name="right">Второй вектор</param>
        /// <returns>Сумма векторов</returns>
        /// <exception cref="ArgumentNullException">Один из векторов равен null</exception>
        /// <exception cref="ArgumentException">Размерности векторов не совпадают</exception>
        public static Vector<T> operator +(Vector<T> left, Vector<T> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left), "Левый вектор не может быть null");
            if (right == null)
                throw new ArgumentNullException(nameof(right), "Правый вектор не может быть null");

            if (left.Dimension != right.Dimension)
                throw new ArgumentException($"Размерности не совпадают: {left.Dimension} != {right.Dimension}");

            T[] result = new T[left.Dimension];
            for (int i = 0; i < left.Dimension; i++)
            {
                result[i] = left.components[i] + right.components[i];
            }

            return new Vector<T>(result);
        }

        /// <summary>
        /// Оператор вычитания векторов.
        /// </summary>
        /// <param name="left">Первый вектор</param>
        /// <param name="right">Второй вектор</param>
        /// <returns>Разность векторов</returns>
        /// <exception cref="ArgumentNullException">Один из векторов равен null</exception>
        /// <exception cref="ArgumentException">Размерности векторов не совпадают</exception>
        public static Vector<T> operator -(Vector<T> left, Vector<T> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left), "Левый вектор не может быть null");
            if (right == null)
                throw new ArgumentNullException(nameof(right), "Правый вектор не может быть null");

            if (left.Dimension != right.Dimension)
                throw new ArgumentException($"Размерности не совпадают: {left.Dimension} != {right.Dimension}");

            T[] result = new T[left.Dimension];
            for (int i = 0; i < left.Dimension; i++)
            {
                result[i] = left.components[i] - right.components[i];
            }

            return new Vector<T>(result);
        }

        /// <summary>
        /// Оператор умножения скаляра на вектор.
        /// </summary>
        /// <param name="scalar">Скаляр (элемент поля)</param>
        /// <param name="vector">Вектор</param>
        /// <returns>Произведение скаляра на вектор</returns>
        /// <exception cref="ArgumentNullException">Скаляр или вектор равны null</exception>
        public static Vector<T> operator *(T scalar, Vector<T> vector)
        {
            if (scalar == null)
                throw new ArgumentNullException(nameof(scalar), "Скаляр не может быть null");
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), "Вектор не может быть null");

            T[] result = new T[vector.Dimension];
            for (int i = 0; i < vector.Dimension; i++)
            {
                result[i] = scalar * vector.components[i];
            }

            return new Vector<T>(result);
        }

        /// <summary>
        /// Оператор умножения вектора на скаляр.
        /// </summary>
        /// <param name="vector">Вектор</param>
        /// <param name="scalar">Скаляр (элемент поля)</param>
        /// <returns>Произведение вектора на скаляр</returns>
        /// <exception cref="ArgumentNullException">Вектор или скаляр равны null</exception>
        public static Vector<T> operator *(Vector<T> vector, T scalar)
        {
            return scalar * vector;  // Используем коммутативность
        }

        /// <summary>
        /// Оператор равенства векторов.
        /// </summary>
        /// <param name="left">Первый вектор</param>
        /// <param name="right">Второй вектор</param>
        /// <returns>true, если векторы равны; иначе false</returns>
        public static bool operator ==(Vector<T> left, Vector<T> right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;

            if (left.Dimension != right.Dimension) return false;

            for (int i = 0; i < left.Dimension; i++)
            {
                if (left.components[i] != right.components[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Оператор неравенства векторов.
        /// </summary>
        /// <param name="left">Первый вектор</param>
        /// <param name="right">Второй вектор</param>
        /// <returns>true, если векторы не равны; иначе false</returns>
        public static bool operator !=(Vector<T> left, Vector<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Вычисляет скалярное произведение двух векторов.
        /// </summary>
        /// <param name="other">Второй вектор</param>
        /// <returns>Скалярное произведение (элемент поля)</returns>
        /// <exception cref="ArgumentNullException">Вектор равен null</exception>
        /// <exception cref="ArgumentException">Размерности векторов не совпадают</exception>
        public T Dot(Vector<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "Вектор не может быть null");

            if (Dimension != other.Dimension)
                throw new ArgumentException($"Размерности не совпадают: {Dimension} != {other.Dimension}");

            T result = T.Zero;
            for (int i = 0; i < Dimension; i++)
            {
                result = result + (components[i] * other.components[i]);
            }

            return result;
        }

        /// <summary>
        /// Вычисляет векторное произведение двух векторов (только для размерности 3).
        /// </summary>
        /// <param name="other">Второй вектор</param>
        /// <returns>Векторное произведение</returns>
        /// <exception cref="ArgumentNullException">Вектор равен null</exception>
        /// <exception cref="InvalidOperationException">Векторное произведение определено только для размерности 3</exception>
        public Vector<T> Cross(Vector<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "Вектор не может быть null");

            if (Dimension != 3 || other.Dimension != 3)
                throw new InvalidOperationException(
                    $"Векторное произведение определено только для размерности 3. Текущие размерности: {Dimension}, {other.Dimension}");

            // a × b = (a2*b3 - a3*b2, a3*b1 - a1*b3, a1*b2 - a2*b1)
            T[] result = new T[3];
            result[0] = components[1] * other.components[2] - components[2] * other.components[1];
            result[1] = components[2] * other.components[0] - components[0] * other.components[2];
            result[2] = components[0] * other.components[1] - components[1] * other.components[0];

            return new Vector<T>(result);
        }

        /// <summary>
        /// Парсит строку в вектор. Формат: "(a1, a2, ..., an)".
        /// </summary>
        /// <param name="str">Строка для парсинга</param>
        /// <returns>Вектор</returns>
        /// <exception cref="ArgumentNullException">Строка равна null</exception>
        /// <exception cref="FormatException">Неверный формат строки</exception>
        public static Vector<T> Parse(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str), "Строка не может быть null");

            str = str.Trim();

            // Проверка формата со скобками
            if (!str.StartsWith("(") || !str.EndsWith(")"))
                throw new FormatException($"Неверный формат: '{str}'. Ожидается формат '(a1, a2, ..., an)'");

            // Удаляем скобки
            string content = str.Substring(1, str.Length - 2).Trim();

            if (string.IsNullOrWhiteSpace(content))
                throw new FormatException($"Неверный формат: '{str}'. Вектор не может быть пустым");

            // Разделяем по запятым
            string[] parts = content.Split(',');

            T[] components = new T[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i].Trim();
                try
                {
                    components[i] = T.Parse(part);
                }
                catch (Exception ex)
                {
                    throw new FormatException(
                        $"Неверный формат компоненты {i} ('{part}') в строке '{str}': {ex.Message}", ex);
                }
            }

            return new Vector<T>(components);
        }

        /// <summary>
        /// Генерирует случайный вектор заданной размерности.
        /// </summary>
        /// <param name="dimension">Размерность вектора</param>
        /// <returns>Случайный вектор</returns>
        /// <exception cref="ArgumentException">Размерность меньше или равна нулю</exception>
        public static Vector<T> GenerateRandom(int dimension)
        {
            if (dimension <= 0)
                throw new ArgumentException("Размерность должна быть положительной", nameof(dimension));

            T[] components = new T[dimension];
            for (int i = 0; i < dimension; i++)
            {
                components[i] = T.GenerateRandom();
            }

            return new Vector<T>(components);
        }

        /// <summary>
        /// Вычисляет квадрат нормы вектора (скалярное произведение вектора на самого себя).
        /// </summary>
        /// <returns>Квадрат нормы (элемент поля)</returns>
        public T NormSquared()
        {
            return Dot(this);
        }

        /// <summary>
        /// Проверяет, является ли вектор нулевым.
        /// </summary>
        /// <returns>true, если все компоненты равны нулю; иначе false</returns>
        public bool IsZero()
        {
            for (int i = 0; i < Dimension; i++)
            {
                if (components[i] != T.Zero)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Возвращает копию массива компонент вектора.
        /// </summary>
        /// <returns>Массив компонент</returns>
        public T[] ToArray()
        {
            T[] result = new T[Dimension];
            Array.Copy(components, result, Dimension);
            return result;
        }

        /// <summary>
        /// Возвращает строковое представление вектора в формате "(a1, a2, ..., an)".
        /// </summary>
        /// <returns>Строковое представление вектора</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");

            for (int i = 0; i < Dimension; i++)
            {
                sb.Append(components[i].ToString());
                if (i < Dimension - 1)
                    sb.Append(", ");
            }

            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// Проверяет равенство с другим вектором.
        /// </summary>
        /// <param name="other">Другой вектор</param>
        /// <returns>true, если векторы равны; иначе false</returns>
        public bool Equals(Vector<T> other)
        {
            return this == other;
        }

        /// <summary>
        /// Проверяет равенство с объектом.
        /// </summary>
        /// <param name="obj">Объект для сравнения</param>
        /// <returns>true, если объекты равны; иначе false</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Vector<T>);
        }

        /// <summary>
        /// Вычисляет хэш-код вектора.
        /// </summary>
        /// <returns>Хэш-код</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + Dimension.GetHashCode();
                foreach (var component in components)
                {
                    if (component != null)
                        hash = hash * 31 + component.GetHashCode();
                }
                return hash;
            }
        }
    }
}
