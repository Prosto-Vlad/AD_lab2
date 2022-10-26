﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab
{
    class Maze
    {
        public class Node
        {
            public bool wached = false;

            public bool top = true;
            public bool right = true;
            public bool bottom = true;
            public bool left = true;

            public int[] corde;
        }

        Node start = new Node();

        Node finish = new Node();

        public Node[,] maze;

        int size;
        string[] side = { "t", "r", "b", "l"};

        public Maze(int size)
        {
            this.size = size;

            maze = new Node[this.size, this.size];

            create_empty_maze();
        }

        public void create_empty_maze()
        {
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        start.corde = new int[2] { 0, 0 };
                        maze[i, j] = start;
                    }
                    else if (i == size - 1 && j == size - 1)
                    {
                        finish.corde = new int[2] { size - 1, size - 1 };
                        maze[i, j] = finish;
                    }
                    else
                    {
                        maze[i, j] = new Node();
                        maze[i, j].corde = new int[2] { i, j };
                    }
                }
            }
        }

        private List<Node> get_neighbours( int y_cur, int x_cur)
        {
            List<Node> neighbours = new List<Node>();
            int[] cord_t = new int[2] { y_cur - 1, x_cur };
            int[] cord_l = new int[2] { y_cur, x_cur+1 };
            int[] cord_b = new int[2] { y_cur + 1, x_cur };
            int[] cord_r = new int[2] { y_cur, x_cur-1};

            List<int[]> temp = new List<int[]>() { cord_t, cord_l, cord_b, cord_r };

            for (int i = 0; i < 4; i++)
            {
                if (temp[i][0] >= 0 && temp[i][0] < size && temp[i][1] >= 0 && temp[i][1] < size)
                {
                    if (maze[temp[i][0], temp[i][1]].wached == false)
                    {
                        maze[temp[i][0], temp[i][1]].wached = true;
                        neighbours.Add(maze[temp[i][0], temp[i][1]]);
                    }
                }
            }

            return neighbours;
        }

        private void dell_wall(Node fir, Node sec)
        {
            if (fir.corde[0] < sec.corde[0])
            {
                maze[fir.corde[0], fir.corde[1]].bottom = false;
                maze[sec.corde[0], sec.corde[1]].top = false;
                return;
            }
            if (fir.corde[0] > sec.corde[0])
            {
                maze[fir.corde[0], fir.corde[1]].top = false;
                maze[sec.corde[0], sec.corde[1]].bottom = false;
                return;
            }
            if (fir.corde[1] < sec.corde[1])
            {
                maze[fir.corde[0], fir.corde[1]].right = false;
                maze[sec.corde[0], sec.corde[1]].left = false;
                return;
            }
            if (fir.corde[0] < sec.corde[0])
            {
                maze[fir.corde[0], fir.corde[1]].left = false;
                maze[sec.corde[0], sec.corde[1]].right = false;
                return;
            }
        }

        public void generate_maze()
        {
            Stack<Node> node_stack = new Stack<Node>();

            Node current = start;
            Node neighbour_node;
            int unwached = size;
            int x = 0, y = 0;

            while (unwached != 0)
            {
                List<Node> neighbours = get_neighbours( current.corde[0], current.corde[1]);

                if(neighbours.Count != 0)
                {
                    Random rand = new Random();
                    maze[current.corde[0], current.corde[1]].wached = true;
                    unwached--;
                    neighbour_node = neighbours[rand.Next(0, neighbours.Count-1)];

                    dell_wall(current, neighbour_node);

                    node_stack.Push(current);
                    current = neighbour_node;
                }
            }

        }

        public void draw_maze()
        {
            for (int i = 0; i < size + size + 1; i++)
            {
                Console.Write("- ");
            }
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                // side wall
                Console.Write('|');
                for (int j = 0; j < size; j++)
                {
                    if(start.corde[0] == i && start.corde[1] == j)
                        Console.Write(" S ");
                    else if (finish.corde[0] == i && finish.corde[1] == j)
                        Console.Write(" E ");
                    else
                        Console.Write(" O ");
                    if (maze[i, j].right == true)
                    {
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write("O");
                    }
                }
                Console.WriteLine();
                // bottom wall
                if (i!=size-1)
                {
                    Console.Write('|');
                    for (int j = 0; j < size; j++)
                    {
                        if (maze[i, j].bottom == true)
                        {
                            Console.Write(" - ");
                        }
                        else
                        {
                            Console.Write(" O ");
                        }
                        Console.Write("|");
                    }
                    Console.WriteLine();
                }
            }

            for (int i = 0; i < size + size + 1 ; i++)
            {
                Console.Write("- ");
            }
        }
    }
}