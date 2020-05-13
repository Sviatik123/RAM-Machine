using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RAMMachine
{
    partial class Program
    {
        static void Main(string[] args)
        {
            // task 1

            Console.WriteLine("Task 1\n");
            Machine m1 = new Machine();
            m1.ParseCommands("../../Commands/task1.txt");
            m1.Run("5 6 3 1 6 8 3 45 64 7 23 0");
            m1.PrintResult();

            // task 2 Виводить 1, якщо рядок відповідає шаблону, 0 якщо ні

            Console.WriteLine("Task 2\n");
            Machine m2 = new Machine();
            m2.ParseCommands("../../Commands/task2.txt");
            m2.Run("1 1 2 2 2 2 0");         
            m2.PrintResult();
            m2.Run("1 1 2 2 0");
            m2.PrintResult();


            // task 3

            Console.WriteLine("Task 3\n");
            Machine m3 = new Machine();
            m3.ParseCommands("../../Commands/task3.txt");
            m3.Run("2");
            m3.PrintResult();
            m3.Run("3");
            m3.PrintResult();

            // task 4 Вводить масив (виводжу регістри для демонстрації роботи)

            Console.WriteLine("Task 4\n");
            Machine m4 = new Machine();
            m4.ParseCommands("../../Commands/task4.txt");
            m4.Run("2 6 2 5 2 5 67 8 2 3 0");
            m4.PrintRegisters();
        }
    }
}
