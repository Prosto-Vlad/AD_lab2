﻿using System;
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

            maze.draw_maze();

            Console.ReadLine();
        }
    }
}