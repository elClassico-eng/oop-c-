using System;

namespace FieldAlgebra.Lab5
{
    public interface IField<T> where T : IField<T>
    {
        // Парсинг строки
        static abstract T Parse(string s);

        // Генерация случайного элемента
        static abstract T Random();

        // Генерация случайного элемента в диапазоне
        static abstract T GenerateRandom(T min, T max);

        // Создание элемента из double
        static abstract T FromDouble(double value);

        // Преобразование в double
        double ToDouble();

        // Нулевой элемент поля
        static abstract T Zero { get; }

        // Единичный элемент поля
        static abstract T One { get; }

        // Обратный элемент (мультипликативно обратный)
        T Inverse { get; }

        // Проверка на нулевой элемент
        bool IsZero { get; }

        // Арифметические операторы
        static abstract T operator +(T a, T b);
        static abstract T operator -(T a, T b);
        static abstract T operator *(T a, T b);
        static abstract T operator /(T a, T b);
        static abstract T operator -(T a);

        // Операторы сравнения
        static abstract bool operator ==(T a, T b);
        static abstract bool operator !=(T a, T b);
    }
}
