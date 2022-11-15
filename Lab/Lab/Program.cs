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
            Result res;
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

                        res = maze.IDS();
                        maze.draw_maze();

                        Console.WriteLine("State - " + maze.GetStartCorde()[0] + ", " + maze.GetStartCorde()[1] + "; " + maze.GetFinishCorde()[0] + ", " + maze.GetFinishCorde()[1]);
                        Console.WriteLine("Iterations - " + res.iterations);
                        Console.WriteLine("Dead end - " + res.corner);
                        Console.WriteLine("States count - " + res.cState);
                        Console.WriteLine("States in memory - " + res.cStateInMemory);
                        Console.WriteLine("Time - " + res.time);
                    }
                    break;
                case "rbfs":
                    for (int i = 0; i < count; i++)
                    {
                        num = i + 1;
                        Console.WriteLine();
                        Console.WriteLine("Maze №" + num);

                        Maze maze = new Maze(size);
                        maze.generate_maze();

                        res = maze.RDFS();
                        maze.draw_maze();

                        Console.WriteLine("State - " + maze.GetStartCorde()[0] + ", " + maze.GetStartCorde()[1] + "; " + maze.GetFinishCorde()[0] + ", " + maze.GetFinishCorde()[1]);
                        Console.WriteLine("Iterations - " + res.iterations);
                        Console.WriteLine("Dead end - " + res.corner);
                        Console.WriteLine("States count - " + res.cState);
                        Console.WriteLine("States in memory - " + res.cStateInMemory);
                        Console.WriteLine("Time - " + res.time);
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
