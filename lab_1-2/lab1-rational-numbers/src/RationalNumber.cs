using System;
using System.Text.RegularExpressions;

namespace RationalNumbers
{
    public class InvalidRationalFormatException : Exception
    {
        public InvalidRationalFormatException(string message) : base(message) { }
    }

    public class RationalDivisionByZeroException : Exception
    {
        public RationalDivisionByZeroException(string message) : base(message) { }
    }

    public class RationalNumber : IEquatable<RationalNumber>, IComparable<RationalNumber>
    {
        private int numerator;
        private uint denominator;

        public int Numerator => numerator;
        public uint Denominator => denominator;

        public RationalNumber(int numerator, uint denominator)
        {
            if (denominator == 0)
                throw new RationalDivisionByZeroException("Знаменатель не может быть равен нулю");

            this.numerator = numerator;
            this.denominator = denominator;
            Reduce();
        }

        public RationalNumber(int numerator) : this(numerator, 1) { }

        public RationalNumber() : this(0, 1) { }

        private void Reduce()
        {
            if (numerator == 0)
            {
                denominator = 1;
                return;
            }

            uint gcd = GCD((uint)Math.Abs(numerator), denominator);
            numerator = (int)(numerator / (int)gcd);
            denominator = denominator / gcd;
        }

        private static uint GCD(uint a, uint b)
        {
            while (b != 0)
            {
                uint temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static RationalNumber Parse(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new InvalidRationalFormatException("Строка не может быть пустой");

            str = str.Trim();

            Match match = Regex.Match(str, @"^(-?\d+)[:/](\d+)$");
            if (!match.Success)
                throw new InvalidRationalFormatException($"Неверный формат строки: {str}. Ожидается формат '4:5' или '4/5'");

            if (!int.TryParse(match.Groups[1].Value, out int num))
                throw new InvalidRationalFormatException($"Неверный числитель: {match.Groups[1].Value}");

            if (!uint.TryParse(match.Groups[2].Value, out uint den) || den == 0)
                throw new InvalidRationalFormatException($"Неверный знаменатель: {match.Groups[2].Value}");

            return new RationalNumber(num, den);
        }

        public static RationalNumber operator +(RationalNumber a, RationalNumber b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Операнды не могут быть null");

            int newNumerator = a.numerator * (int)b.denominator + b.numerator * (int)a.denominator;
            uint newDenominator = a.denominator * b.denominator;
            return new RationalNumber(newNumerator, newDenominator);
        }

        public static RationalNumber operator -(RationalNumber a, RationalNumber b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Операнды не могут быть null");

            int newNumerator = a.numerator * (int)b.denominator - b.numerator * (int)a.denominator;
            uint newDenominator = a.denominator * b.denominator;
            return new RationalNumber(newNumerator, newDenominator);
        }

        public static RationalNumber operator *(RationalNumber a, RationalNumber b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Операнды не могут быть null");

            int newNumerator = a.numerator * b.numerator;
            uint newDenominator = a.denominator * b.denominator;
            return new RationalNumber(newNumerator, newDenominator);
        }

        public static RationalNumber operator /(RationalNumber a, RationalNumber b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Операнды не могут быть null");

            if (b.numerator == 0)
                throw new RationalDivisionByZeroException("Деление на ноль");

            int newNumerator = a.numerator * (int)b.denominator;
            uint newDenominator = a.denominator * (uint)Math.Abs(b.numerator);
            
            if (b.numerator < 0)
                newNumerator = -newNumerator;

            return new RationalNumber(newNumerator, newDenominator);
        }

        public static bool operator ==(RationalNumber a, RationalNumber b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.numerator * (int)b.denominator == b.numerator * (int)a.denominator;
        }

        public static bool operator !=(RationalNumber a, RationalNumber b)
        {
            return !(a == b);
        }

        public static bool operator <(RationalNumber a, RationalNumber b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Операнды не могут быть null");

            return a.numerator * (int)b.denominator < b.numerator * (int)a.denominator;
        }

        public static bool operator >(RationalNumber a, RationalNumber b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Операнды не могут быть null");

            return a.numerator * (int)b.denominator > b.numerator * (int)a.denominator;
        }

        public static bool operator <=(RationalNumber a, RationalNumber b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(RationalNumber a, RationalNumber b)
        {
            return a > b || a == b;
        }

        public static RationalNumber Zero => new RationalNumber(0, 1);
        public static RationalNumber One => new RationalNumber(1, 1);

        public RationalNumber Inverse
        {
            get
            {
                if (numerator == 0)
                    throw new RationalDivisionByZeroException("Невозможно найти обратный элемент для нуля");

                if (numerator > 0)
                    return new RationalNumber((int)denominator, (uint)numerator);
                else
                    return new RationalNumber(-(int)denominator, (uint)(-numerator));
            }
        }

        public override string ToString()
        {
            return $"{numerator}/{denominator}";
        }

        public static RationalNumber GenerateRandom(RationalNumber min, RationalNumber max)
        {
            if (min == null || max == null)
                throw new ArgumentNullException("Границы интервала не могут быть null");

            if (min > max)
                throw new ArgumentException("Минимальное значение не может быть больше максимального");

            Random random = new Random();

            double minValue = (double)min.numerator / min.denominator;
            double maxValue = (double)max.numerator / max.denominator;
            double randomValue = minValue + (maxValue - minValue) * random.NextDouble();

            int randomNumerator = (int)(randomValue * 100);
            uint randomDenominator = 100;

            return new RationalNumber(randomNumerator, randomDenominator);
        }

        public bool Equals(RationalNumber other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RationalNumber);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(numerator, denominator);
        }

        public int CompareTo(RationalNumber other)
        {
            if (other == null) return 1;
            if (this == other) return 0;
            return this < other ? -1 : 1;
        }
    }
}