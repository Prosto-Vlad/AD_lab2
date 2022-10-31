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
            Maze maze = new Maze(20);

            maze.generate_maze();
            maze.draw_maze();

            maze.IDS();
            maze.draw_maze();

            Console.ReadLine();
        }
    }
}
