using System;

namespace FieldInterface
{
    /// <summary>
    /// Интерфейс математического поля.
    /// Определяет операции, необходимые для типа, чтобы образовывать алгебраическое поле.
    /// </summary>
    /// <typeparam name="T">Тип элемента поля, реализующий сам этот интерфейс</typeparam>
    public interface IField<T> where T : IField<T>
    {
        // ============================================
        // СТАТИЧЕСКИЕ АБСТРАКТНЫЕ СВОЙСТВА
        // ============================================

        /// <summary>
        /// Нулевой элемент поля (аддитивная идентичность).
        /// Для любого элемента a: a + Zero = a
        /// </summary>
        static abstract T Zero { get; }

        /// <summary>
        /// Единичный элемент поля (мультипликативная идентичность).
        /// Для любого элемента a: a * One = a
        /// </summary>
        static abstract T One { get; }

        // ============================================
        // СТАТИЧЕСКИЕ АБСТРАКТНЫЕ МЕТОДЫ
        // ============================================

        /// <summary>
        /// Парсинг элемента поля из строки.
        /// </summary>
        /// <param name="str">Строковое представление элемента</param>
        /// <returns>Элемент поля</returns>
        /// <exception cref="FormatException">Если строка имеет неверный формат</exception>
        static abstract T Parse(string str);

        /// <summary>
        /// Генерация случайного элемента поля.
        /// </summary>
        /// <returns>Случайный элемент поля</returns>
        static abstract T GenerateRandom();

        // ============================================
        // СТАТИЧЕСКИЕ АБСТРАКТНЫЕ ОПЕРАТОРЫ
        // ============================================

        /// <summary>
        /// Сложение двух элементов поля.
        /// </summary>
        /// <param name="left">Левый операнд</param>
        /// <param name="right">Правый операнд</param>
        /// <returns>Сумма элементов</returns>
        static abstract T operator +(T left, T right);

        /// <summary>
        /// Вычитание двух элементов поля.
        /// </summary>
        /// <param name="left">Левый операнд (уменьшаемое)</param>
        /// <param name="right">Правый операнд (вычитаемое)</param>
        /// <returns>Разность элементов</returns>
        static abstract T operator -(T left, T right);

        /// <summary>
        /// Умножение двух элементов поля.
        /// </summary>
        /// <param name="left">Левый операнд</param>
        /// <param name="right">Правый операнд</param>
        /// <returns>Произведение элементов</returns>
        static abstract T operator *(T left, T right);

        /// <summary>
        /// Деление двух элементов поля.
        /// </summary>
        /// <param name="left">Левый операнд (делимое)</param>
        /// <param name="right">Правый операнд (делитель)</param>
        /// <returns>Частное элементов</returns>
        /// <exception cref="DivideByZeroException">Если делитель равен нулю</exception>
        static abstract T operator /(T left, T right);

        /// <summary>
        /// Проверка равенства двух элементов поля.
        /// </summary>
        /// <param name="left">Левый операнд</param>
        /// <param name="right">Правый операнд</param>
        /// <returns>true, если элементы равны; иначе false</returns>
        static abstract bool operator ==(T left, T right);

        /// <summary>
        /// Проверка неравенства двух элементов поля.
        /// </summary>
        /// <param name="left">Левый операнд</param>
        /// <param name="right">Правый операнд</param>
        /// <returns>true, если элементы не равны; иначе false</returns>
        static abstract bool operator !=(T left, T right);

        // ============================================
        // СВОЙСТВА ЭКЗЕМПЛЯРА
        // ============================================

        /// <summary>
        /// Мультипликативный обратный элемент (1/x).
        /// Для ненулевого элемента a: a * a.Inverse = One
        /// </summary>
        /// <exception cref="DivideByZeroException">Если элемент равен нулю</exception>
        T Inverse { get; }
    }
}
