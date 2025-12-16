using System;

namespace FieldInterface
{
    /// <summary>
    /// Тип решения системы линейных уравнений.
    /// Определяется на основе рангов матрицы коэффициентов и расширенной матрицы.
    /// </summary>
    public enum SolutionType
    {
        /// <summary>
        /// Единственное решение.
        /// Условие: rank(A) = rank([A|b]) = n, где n - количество неизвестных.
        /// </summary>
        UniqueSolution,

        /// <summary>
        /// Бесконечно много решений.
        /// Условие: rank(A) = rank([A|b]) &lt; n, где n - количество неизвестных.
        /// </summary>
        InfiniteSolutions,

        /// <summary>
        /// Нет решений (система несовместна).
        /// Условие: rank(A) &lt; rank([A|b]).
        /// </summary>
        NoSolution
    }
}
