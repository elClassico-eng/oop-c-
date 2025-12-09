using System;
using System.Text.RegularExpressions;

namespace FieldAlgebra.Lab4
{
    public class InvalidComplexFormatException : Exception
    {
        public InvalidComplexFormatException(string message) : base(message) { }
    }

    public class ComplexDivisionByZeroException : Exception
    {
        public ComplexDivisionByZeroException(string message) : base(message) { }
    }

    public class ComplexNumber : IField<ComplexNumber>, IEquatable<ComplexNumber>
    {
        private double real;
        private double imaginary;

        public double Real => real;
        public double Imaginary => imaginary;

        public ComplexNumber(double real, double imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }

        public ComplexNumber(double real) : this(real, 0) { }

        public ComplexNumber() : this(0, 0) { }

        public static ComplexNumber Parse(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new InvalidComplexFormatException("Строка не может быть пустой");

            str = str.Replace(" ", "").Replace("j", "i");

            Match match = Regex.Match(str, @"^([+-]?\d*\.?\d*)([+-]?\d*\.?\d*)i$");
            if (match.Success)
            {
                string realPart = match.Groups[1].Value;
                string imaginaryPart = match.Groups[2].Value;

                if (string.IsNullOrEmpty(realPart)) realPart = "0";
                if (string.IsNullOrEmpty(imaginaryPart) || imaginaryPart == "+") imaginaryPart = "1";
                if (imaginaryPart == "-") imaginaryPart = "-1";

                if (!double.TryParse(realPart, out double realValue))
                    throw new InvalidComplexFormatException($"Неверная вещественная часть: {realPart}");

                if (!double.TryParse(imaginaryPart, out double imaginaryValue))
                    throw new InvalidComplexFormatException($"Неверная мнимая часть: {imaginaryPart}");

                return new ComplexNumber(realValue, imaginaryValue);
            }

            match = Regex.Match(str, @"^([+-]?\d*\.?\d*)i$");
            if (match.Success)
            {
                string imaginaryPart = match.Groups[1].Value;
                if (string.IsNullOrEmpty(imaginaryPart) || imaginaryPart == "+") imaginaryPart = "1";
                if (imaginaryPart == "-") imaginaryPart = "-1";

                if (!double.TryParse(imaginaryPart, out double imaginaryValue))
                    throw new InvalidComplexFormatException($"Неверная мнимая часть: {imaginaryPart}");

                return new ComplexNumber(0, imaginaryValue);
            }

            if (double.TryParse(str, out double onlyReal))
            {
                return new ComplexNumber(onlyReal, 0);
            }

            throw new InvalidComplexFormatException($"Неверный формат строки: {str}. Ожидается формат '2+4i', '-4i' или '5'");
        }

        public static ComplexNumber Random()
        {
            Random random = new Random();
            double realPart = (random.NextDouble() - 0.5) * 20;
            double imaginaryPart = (random.NextDouble() - 0.5) * 20;
            return new ComplexNumber(realPart, imaginaryPart);
        }

        public static ComplexNumber GenerateRandom(ComplexNumber min, ComplexNumber max)
        {
            if (min == null || max == null)
                throw new ArgumentNullException("Границы интервала не могут быть null");

            Random random = new Random();
            double realValue = min.real + (max.real - min.real) * random.NextDouble();
            double imagValue = min.imaginary + (max.imaginary - min.imaginary) * random.NextDouble();
            return new ComplexNumber(realValue, imagValue);
        }

        public static ComplexNumber FromDouble(double value)
        {
            return new ComplexNumber(value, 0);
        }

        public double ToDouble()
        {
            if (Math.Abs(imaginary) > 1e-10)
                throw new InvalidOperationException("Невозможно преобразовать комплексное число с ненулевой мнимой частью в double");
            return real;
        }

        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Операнды не могут быть null");

            return new ComplexNumber(a.real + b.real, a.imaginary + b.imaginary);
        }

        public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Операнды не могут быть null");

            return new ComplexNumber(a.real - b.real, a.imaginary - b.imaginary);
        }

        public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Операнды не могут быть null");

            double newReal = a.real * b.real - a.imaginary * b.imaginary;
            double newImaginary = a.real * b.imaginary + a.imaginary * b.real;
            return new ComplexNumber(newReal, newImaginary);
        }

        public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Операнды не могут быть null");

            double denominator = b.real * b.real + b.imaginary * b.imaginary;
            if (Math.Abs(denominator) < 1e-10)
                throw new ComplexDivisionByZeroException("Деление на ноль");

            double newReal = (a.real * b.real + a.imaginary * b.imaginary) / denominator;
            double newImaginary = (a.imaginary * b.real - a.real * b.imaginary) / denominator;
            return new ComplexNumber(newReal, newImaginary);
        }

        public static ComplexNumber operator -(ComplexNumber a)
        {
            if (a == null)
                throw new ArgumentNullException("Операнд не может быть null");

            return new ComplexNumber(-a.real, -a.imaginary);
        }

        public static bool operator ==(ComplexNumber a, ComplexNumber b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return Math.Abs(a.real - b.real) < 1e-10 && Math.Abs(a.imaginary - b.imaginary) < 1e-10;
        }

        public static bool operator !=(ComplexNumber a, ComplexNumber b)
        {
            return !(a == b);
        }

        public static ComplexNumber Zero => new ComplexNumber(0, 0);
        public static ComplexNumber One => new ComplexNumber(1, 0);

        public ComplexNumber Inverse
        {
            get
            {
                double denominator = real * real + imaginary * imaginary;
                if (Math.Abs(denominator) < 1e-10)
                    throw new ComplexDivisionByZeroException("Невозможно найти обратный элемент для нуля");

                return new ComplexNumber(real / denominator, -imaginary / denominator);
            }
        }

        public bool IsZero => Math.Abs(real) < 1e-10 && Math.Abs(imaginary) < 1e-10;

        public override string ToString()
        {
            if (Math.Abs(imaginary) < 1e-10)
                return real.ToString("F2");

            if (Math.Abs(real) < 1e-10)
                return imaginary >= 0 ? $"{imaginary:F2}i" : $"{imaginary:F2}i";

            string imaginaryStr = imaginary >= 0 ? $"+{imaginary:F2}i" : $"{imaginary:F2}i";
            return $"{real:F2}{imaginaryStr}";
        }

        public bool Equals(ComplexNumber other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ComplexNumber);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(real, imaginary);
        }
    }
}
