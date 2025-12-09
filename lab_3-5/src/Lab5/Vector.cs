using System;
using System.Linq;
using System.Text;

namespace FieldAlgebra.Lab5
{
    public class InvalidVectorFormatException : Exception
    {
        public InvalidVectorFormatException(string message) : base(message) { }
    }

    public class VectorDimensionMismatchException : Exception
    {
        public VectorDimensionMismatchException(string message) : base(message) { }
    }

    public class VectorOperationException : Exception
    {
        public VectorOperationException(string message) : base(message) { }
    }

    public class Vector<T> where T : IField<T>
    {
        private T[] components;

        public int Length => components.Length;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= components.Length)
                    throw new IndexOutOfRangeException("Индекс выходит за границы вектора");
                return components[index];
            }
            set
            {
                if (index < 0 || index >= components.Length)
                    throw new IndexOutOfRangeException("Индекс выходит за границы вектора");
                components[index] = value;
            }
        }

        public Vector(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Размерность вектора должна быть положительной", nameof(length));

            components = new T[length];
            for (int i = 0; i < length; i++)
            {
                components[i] = T.Zero;
            }
        }

        public Vector(params T[] components)
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));
            if (components.Length == 0)
                throw new ArgumentException("Вектор не может быть пустым", nameof(components));

            this.components = (T[])components.Clone();
        }

        public static Vector<T> Zero(int length)
        {
            return new Vector<T>(length);
        }

        public static Vector<T> BasisVector(int length, int direction)
        {
            if (direction < 0 || direction >= length)
                throw new ArgumentException("Направление должно быть в пределах размерности", nameof(direction));

            var vector = new Vector<T>(length);
            vector.components[direction] = T.One;
            return vector;
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Векторы не могут быть null");
            if (a.Length != b.Length)
                throw new VectorDimensionMismatchException("Векторы должны иметь одинаковую размерность");

            T[] resultComponents = new T[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                resultComponents[i] = a.components[i] + b.components[i];
            }
            return new Vector<T>(resultComponents);
        }

        public static Vector<T> operator -(Vector<T> a, Vector<T> b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Векторы не могут быть null");
            if (a.Length != b.Length)
                throw new VectorDimensionMismatchException("Векторы должны иметь одинаковую размерность");

            T[] resultComponents = new T[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                resultComponents[i] = a.components[i] - b.components[i];
            }
            return new Vector<T>(resultComponents);
        }

        public static Vector<T> operator *(Vector<T> vector, T scalar)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector));

            T[] resultComponents = new T[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                resultComponents[i] = vector.components[i] * scalar;
            }
            return new Vector<T>(resultComponents);
        }

        public static Vector<T> operator *(T scalar, Vector<T> vector)
        {
            return vector * scalar;
        }

        public static Vector<T> operator /(Vector<T> vector, T scalar)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector));
            if (scalar.IsZero)
                throw new DivideByZeroException("Деление на нулевой скаляр невозможно");

            T[] resultComponents = new T[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                resultComponents[i] = vector.components[i] / scalar;
            }
            return new Vector<T>(resultComponents);
        }

        public static Vector<T> operator -(Vector<T> vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector));

            return vector * (-T.One);
        }

        public static bool operator ==(Vector<T> a, Vector<T> b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if ((object)a == null || (object)b == null) return false;
            if (a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a.components[i] != b.components[i])
                    return false;
            }
            return true;
        }

        public static bool operator !=(Vector<T> a, Vector<T> b)
        {
            return !(a == b);
        }

        public T DotProduct(Vector<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (Length != other.Length)
                throw new VectorDimensionMismatchException("Векторы должны иметь одинаковую размерность");

            T result = T.Zero;
            for (int i = 0; i < Length; i++)
            {
                result = result + (components[i] * other.components[i]);
            }
            return result;
        }

        public Vector<T> CrossProduct(Vector<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (Length != 3 || other.Length != 3)
                throw new VectorOperationException("Векторное произведение определено только для 3D векторов");

            T x = components[1] * other.components[2] - components[2] * other.components[1];
            T y = components[2] * other.components[0] - components[0] * other.components[2];
            T z = components[0] * other.components[1] - components[1] * other.components[0];

            return new Vector<T>(x, y, z);
        }

        public double Magnitude()
        {
            double sumOfSquares = 0;
            for (int i = 0; i < Length; i++)
            {
                double value = components[i].ToDouble();
                sumOfSquares += value * value;
            }
            return Math.Sqrt(sumOfSquares);
        }

        public T MagnitudeSquared()
        {
            T sumOfSquares = T.Zero;
            for (int i = 0; i < Length; i++)
            {
                sumOfSquares = sumOfSquares + (components[i] * components[i]);
            }
            return sumOfSquares;
        }

        public Vector<T> Normalize()
        {
            double magnitude = Magnitude();
            if (magnitude == 0)
                throw new VectorOperationException("Невозможно нормализовать нулевой вектор");

            T scalar = T.FromDouble(1.0 / magnitude);
            return this * scalar;
        }

        public static Vector<T> Random(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Размерность должна быть положительной", nameof(length));

            T[] randomComponents = new T[length];
            for (int i = 0; i < length; i++)
            {
                randomComponents[i] = T.Random();
            }
            return new Vector<T>(randomComponents);
        }

        public static Vector<T> GenerateRandom(int length, T minComponent, T maxComponent)
        {
            if (length <= 0)
                throw new ArgumentException("Размерность должна быть положительной", nameof(length));

            T[] randomComponents = new T[length];
            for (int i = 0; i < length; i++)
            {
                randomComponents[i] = T.GenerateRandom(minComponent, maxComponent);
            }
            return new Vector<T>(randomComponents);
        }

        public static Vector<T> Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new InvalidVectorFormatException("Строка не может быть пустой или null");

            s = s.Trim();

            if (!s.StartsWith("(") || !s.EndsWith(")"))
                throw new InvalidVectorFormatException("Строка должна быть в формате '(a1, a2, ..., an)'");

            string inner = s.Substring(1, s.Length - 2).Trim();
            if (string.IsNullOrEmpty(inner))
                throw new InvalidVectorFormatException("Вектор не может быть пустым");

            string[] componentStrings = inner.Split(',');
            T[] components = new T[componentStrings.Length];

            for (int i = 0; i < componentStrings.Length; i++)
            {
                string componentStr = componentStrings[i].Trim();
                try
                {
                    components[i] = T.Parse(componentStr);
                }
                catch (Exception ex)
                {
                    throw new InvalidVectorFormatException($"Ошибка парсинга компоненты {i + 1}: '{componentStr}'", ex);
                }
            }

            return new Vector<T>(components);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < components.Length; i++)
            {
                sb.Append(components[i].ToString());
                if (i < components.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(")");
            return sb.ToString();
        }

        public bool IsZero()
        {
            foreach (var component in components)
            {
                if (!component.IsZero)
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vector<T>);
        }

        public bool Equals(Vector<T> other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var component in components)
                {
                    hash = hash * 23 + component.GetHashCode();
                }
                return hash;
            }
        }

        public Vector<T> Clone()
        {
            return new Vector<T>((T[])components.Clone());
        }

        public Vector<T> ProjectOnto(Vector<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (Length != other.Length)
                throw new VectorDimensionMismatchException("Векторы должны иметь одинаковую размерность");

            T scalar = this.DotProduct(other) / other.DotProduct(other);
            return other * scalar;
        }

        public bool IsOrthogonalTo(Vector<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (Length != other.Length)
                throw new VectorDimensionMismatchException("Векторы должны иметь одинаковую размерность");

            return this.DotProduct(other).IsZero;
        }
    }
}
