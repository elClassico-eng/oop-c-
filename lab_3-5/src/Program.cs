using System;

namespace FieldAlgebra
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ЛАБОРАТОРНЫЕ РАБОТЫ 3-5 ===");
                Console.WriteLine();
                Console.WriteLine("1. Лаба 3: Интерфейс IField");
                Console.WriteLine("2. Лаба 4: Векторы");
                Console.WriteLine("3. Лаба 5: Системы линейных уравнений");
                Console.WriteLine("0. Выход");
                Console.WriteLine();
                Console.Write("Выберите лабу: ");

                string choice = Console.ReadLine();

                Console.Clear();

                switch (choice)
                {
                    case "1":
                        Lab3.Program3.Main(args);
                        break;
                    case "2":
                        Lab4.Program4.Main(args);
                        break;
                    case "3":
                        Lab5.Program5.Main(args);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
