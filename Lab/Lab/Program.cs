using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            string answ;
            int num, count, size;

            Console.WriteLine("Which algoritm you want? ids/rbfs");
            answ = Console.ReadLine();

            Console.WriteLine("How much maze you want generate?");
            count = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Maze size?");
            size = Convert.ToInt32(Console.ReadLine());
            switch (answ)
            {
                case "ids":
                    for (int i = 0; i < count; i++)
                    {
                        num = i + 1;
                        Console.WriteLine();
                        Console.WriteLine("Maze №" + num);

                        Maze maze = new Maze(size);
                        maze.generate_maze();

                        maze.IDS();
                        maze.draw_maze();
                    }
                    break;
                case "rbfs":
                    for (int i = 0; i < count; i++)
                    {
                        num = i + 1;
                        Console.WriteLine();
                        Console.WriteLine("Maze №" + num);

                        Maze maze2 = new Maze(size);
                        maze2.generate_maze();

                        maze2.RDFS();
                        maze2.draw_maze();
                    }
                    break;
                default:
                    Console.WriteLine("Error");
                    break;
            }
            
            Console.ReadLine();
        }
    }
}
