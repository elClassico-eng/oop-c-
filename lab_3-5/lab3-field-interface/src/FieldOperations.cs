using System;

namespace FieldInterface
{
    /// <summary>
    /// Утилитарный класс с обобщенными операциями над элементами поля.
    /// Демонстрирует мощь интерфейса IField - универсальные алгоритмы для любых полей.
    /// </summary>
    public static class FieldOperations
    {
        /// <summary>
        /// Возведение элемента поля в целую степень.
        /// </summary>
        /// <typeparam name="T">Тип элемента поля</typeparam>
        /// <param name="value">Основание степени</param>
        /// <param name="exponent">Показатель степени (целое число)</param>
        /// <returns>Результат возведения в степень</returns>
        /// <exception cref="DivideByZeroException">Если value = 0 и exponent &lt; 0</exception>
        public static T Power<T>(T value, int exponent) where T : IField<T>
        {
            // 0^0 = 1 по определению (математическое соглашение)
            if (exponent == 0)
                return T.One;

            // Для отрицательных степеней: a^(-n) = (1/a)^n = (a.Inverse)^n
            if (exponent < 0)
                return Power(value.Inverse, -exponent);

            // Для положительных степеней: последовательное умножение
            T result = T.One;
            for (int i = 0; i < exponent; i++)
            {
                result = result * value;
            }
            return result;
        }

        /// <summary>
        /// Вычисление суммы массива элементов поля.
        /// </summary>
        /// <typeparam name="T">Тип элемента поля</typeparam>
        /// <param name="elements">Массив элементов для суммирования</param>
        /// <returns>Сумма всех элементов (или Zero, если массив пустой)</returns>
        public static T Sum<T>(params T[] elements) where T : IField<T>
        {
            T result = T.Zero;
            foreach (var element in elements)
            {
                result = result + element;
            }
            return result;
        }

        /// <summary>
        /// Вычисление произведения массива элементов поля.
        /// </summary>
        /// <typeparam name="T">Тип элемента поля</typeparam>
        /// <param name="elements">Массив элементов для перемножения</param>
        /// <returns>Произведение всех элементов (или One, если массив пустой)</returns>
        public static T Product<T>(params T[] elements) where T : IField<T>
        {
            T result = T.One;
            foreach (var element in elements)
            {
                result = result * element;
            }
            return result;
        }

        /// <summary>
        /// Вычисление среднего арифметического массива элементов поля.
        /// Среднее = (сумма элементов) / (количество элементов)
        /// </summary>
        /// <typeparam name="T">Тип элемента поля</typeparam>
        /// <param name="elements">Массив элементов</param>
        /// <returns>Среднее арифметическое</returns>
        /// <exception cref="ArgumentException">Если массив пустой</exception>
        public static T Average<T>(params T[] elements) where T : IField<T>
        {
            if (elements.Length == 0)
                throw new ArgumentException("Невозможно вычислить среднее для пустого массива");

            // Вычисляем сумму
            T sum = Sum(elements);

            // Создаем элемент поля, представляющий количество элементов
            // Делаем это через последовательное сложение единиц
            T count = T.One;
            for (int i = 1; i < elements.Length; i++)
            {
                count = count + T.One;
            }

            // Среднее = сумма / количество
            return sum / count;
        }

        /// <summary>
        /// Печать таблицы операций для набора элементов.
        /// Создает двумерную таблицу, где каждая ячейка - результат операции.
        /// </summary>
        /// <typeparam name="T">Тип элемента поля</typeparam>
        /// <param name="operation">Название операции (для заголовка)</param>
        /// <param name="op">Функция операции (например, (a, b) => a + b)</param>
        /// <param name="elements">Массив элементов для таблицы</param>
        public static void PrintOperationTable<T>(string operation, Func<T, T, T> op, params T[] elements)
            where T : IField<T>
        {
            Console.WriteLine($"\nТаблица операции '{operation}':");

            // Печать заголовка (элементы по горизонтали)
            Console.Write("      ");
            foreach (var elem in elements)
            {
                Console.Write($"{elem,10}");
            }
            Console.WriteLine();

            // Печать строк таблицы
            foreach (var row in elements)
            {
                // Элемент по вертикали
                Console.Write($"{row,6}");

                // Результаты операций
                foreach (var col in elements)
                {
                    try
                    {
                        var result = op(row, col);
                        Console.Write($"{result,10}");
                    }
                    catch (Exception ex)
                    {
                        // Если операция невозможна (например, деление на ноль)
                        Console.Write($"{"ERROR",10}");
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Вычисление линейной комбинации элементов поля.
        /// Результат = c1*e1 + c2*e2 + ... + cn*en
        /// </summary>
        /// <typeparam name="T">Тип элемента поля</typeparam>
        /// <param name="coefficients">Массив коэффициентов</param>
        /// <param name="elements">Массив элементов</param>
        /// <returns>Линейная комбинация</returns>
        /// <exception cref="ArgumentException">Если длины массивов не совпадают</exception>
        public static T LinearCombination<T>(T[] coefficients, T[] elements) where T : IField<T>
        {
            if (coefficients.Length != elements.Length)
                throw new ArgumentException("Длины массивов коэффициентов и элементов должны совпадать");

            T result = T.Zero;
            for (int i = 0; i < coefficients.Length; i++)
            {
                result = result + (coefficients[i] * elements[i]);
            }
            return result;
        }

        /// <summary>
        /// Проверка, является ли элемент нулевым элементом поля.
        /// </summary>
        /// <typeparam name="T">Тип элемента поля</typeparam>
        /// <param name="value">Элемент для проверки</param>
        /// <returns>true, если элемент равен Zero; иначе false</returns>
        public static bool IsZero<T>(T value) where T : IField<T>
        {
            return value == T.Zero;
        }

        /// <summary>
        /// Проверка, является ли элемент единичным элементом поля.
        /// </summary>
        /// <typeparam name="T">Тип элемента поля</typeparam>
        /// <param name="value">Элемент для проверки</param>
        /// <returns>true, если элемент равен One; иначе false</returns>
        public static bool IsOne<T>(T value) where T : IField<T>
        {
            return value == T.One;
        }
    }
}
