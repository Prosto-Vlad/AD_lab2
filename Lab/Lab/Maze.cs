using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;



namespace Lab
{
    class Maze
    {
        public class Node
        {
            public bool wached = false;
            public bool isPath = false;

            public bool top = true;
            public bool right = true;
            public bool bottom = true;
            public bool left = true;

            public int[] corde;



            public Node parent;
            public List<Node> children = new List<Node>();
            public double f = double.MaxValue;
            //public bool isStart = false;
            //public bool isEnd = false;

            public int _unwatched_childrens()
            {
                int count = 0;
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i].wached == false)
                    {
                        count += 1;
                    }
                }
                return count;
            }
        }

        //private List<Node> path = new List<Node>();
        private Node start = new Node();
        private Node finish = new Node();
        private Node[,] maze;
        private int size;

        private List<Node> get_neighbours(int y_cur, int x_cur)
        {
            List<Node> neighbours = new List<Node>();
            int[] cord_t = new int[2] { y_cur - 1, x_cur };
            int[] cord_l = new int[2] { y_cur, x_cur - 1 };
            int[] cord_b = new int[2] { y_cur + 1, x_cur };
            int[] cord_r = new int[2] { y_cur, x_cur + 1 };

            List<int[]> temp = new List<int[]>() { cord_t, cord_l, cord_b, cord_r };

            for (int i = 0; i < 4; i++)
            {
                if (temp[i][0] >= 0 && temp[i][0] < size && temp[i][1] >= 0 && temp[i][1] < size)
                {
                    if (maze[temp[i][0], temp[i][1]].wached == false)
                    {
                        //maze[temp[i][0], temp[i][1]].wached = true;
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
            if (fir.corde[1] > sec.corde[1])
            {
                maze[fir.corde[0], fir.corde[1]].left = false;
                maze[sec.corde[0], sec.corde[1]].right = false;
                return;
            }
        }

        private void make_false()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    maze[i, j].wached = false;
                }
            }
        }

        public Maze(int size)
        {
            this.size = size;

            maze = new Node[this.size, this.size];

            create_empty_maze();
        }

        private void create_empty_maze()
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

        public void generate_maze()
        {
            Stack<Node> node_stack = new Stack<Node>();

            Node current = start;
            start.wached = true;
            Node neighbour_node;
            int unwached = size * size;
            Random rand = new Random();

            while (unwached != 0)
            {
                List<Node> neighbours = get_neighbours(current.corde[0], current.corde[1]);
                if (neighbours.Count != 0)
                {
                    if (current.corde[0] == finish.corde[0] && current.corde[1] == finish.corde[1])
                    {
                        maze[current.corde[0], current.corde[1]].wached = true;
                        unwached--;
                        current = node_stack.Pop();
                    }
                    else
                    {
                        int index = rand.Next(neighbours.Count);
                        neighbour_node = neighbours[index];
                        maze[neighbour_node.corde[0], neighbour_node.corde[1]].wached = true;
                        unwached--;

                        dell_wall(current, neighbour_node);

                        current.children.Add(neighbour_node);
                        neighbour_node.parent = current;

                        node_stack.Push(current);
                        current = neighbour_node;
                    }
                }
                else if (node_stack.Count > 0)
                {
                    current = node_stack.Pop();
                }
            }

            make_false();
        }

        public void draw_maze()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < size + size + 1; i++)
            {
                Console.Write("--");
            }
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                // side wall
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write('|');
                Console.ResetColor();
                for (int j = 0; j < size; j++)
                {
                    if (start.corde[0] == i && start.corde[1] == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" S ");
                        Console.ResetColor();
                    }
                    else if (finish.corde[0] == i && finish.corde[1] == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" E ");
                        Console.ResetColor();
                    }
                    else if (maze[i, j].isPath == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" P ");
                        Console.ResetColor();
                    }
                    else
                        Console.Write(" * ");
                    if (maze[i, j].right == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("|");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("*");
                    }
                }
                Console.WriteLine();
                // bottom wall
                if (i != size - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write('|');
                    Console.ResetColor();
                    for (int j = 0; j < size; j++)
                    {
                        if (maze[i, j].bottom == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("---");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write(" * ");
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("|");
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }
            }

            for (int i = 0; i < size + size + 1; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("--");
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        private bool _DLS(int max_depth)
        {
            return _recursive_DLS(max_depth, 0, maze[0, 0]);
        }

        private bool _recursive_DLS(int max_depth, int depth, Node cur)
        {
            if (cur == null || depth >= max_depth)
            {
                return false;
            }
            foreach (Node childe in cur.children)
            {
                if (childe.corde[0] == finish.corde[0] && childe.corde[1] == finish.corde[1])
                {
                    cur.isPath = true;
                    return true;
                }
                else
                {
                    depth++;
                    cur.isPath = true;
                    if (_recursive_DLS(max_depth, depth, childe))
                    {
                        return true;
                    }

                    cur.isPath = false;
                }
            }
            depth--;
            return false;
        }

        public bool IDS()
        {
            int depth = 0;
            bool result = false;
            while (true)
            {
                result = _DLS(depth);
                if (result != false)
                {
                    make_false();
                    return result;
                }
                depth++;
            }
        }

        private double _funk(Node cur, int g)
        {
            return h(cur) + g;
        }

        private double h(Node node)
        {
            if (node.corde[0] == finish.corde[0] && node.corde[1] == finish.corde[1])
            {
                return 0;
            }
            return Math.Sqrt(Math.Pow(2,finish.corde[0] - node.corde[0]) + Math.Pow(2, finish.corde[1] - node.corde[1]));
        }

        private Node _find_best(Node node)
        {
            //int j = 0;
            //while (node.children[j].wached == false)
            //{
            //    j += 1;
            //}
            Node best = new Node();
            for (int i = 0; i < node.children.Count; i++)
            {
                if (best.f > node.children[i].f)
                {
                    best = node.children[i];
                }
            }
            return best;
        }

        private Node _find_alternative(Node node, Node best)
        {
            Node alter = node.children[0];
            for (int i = 0; i < node.children.Count; i++)
            {
                if (alter.f > node.children[i].f && node.children[i] != best)
                {
                    alter = node.children[i];
                }
            }
            return alter;
        }

        private void _find_alt_and_best(Node node, ref int best, ref int alt)
        {
            for (int i = 0; i < node.children.Count; i++)
            {
                if (node.children[best].f >= node.children[i].f)
                {
                    alt = best;
                    best = i;
                }
            }
        }

        private bool _recursive_RDFS(Node cur, ref double fLimit, int g)
        {
            //cur.wached = true;
            //if(g == 10000)
            //{
            //    g++;
            //    g--;
            //}
            if (cur.corde[0] == finish.corde[0] && cur.corde[1] == finish.corde[1])
            {
                return true;
            }
            if (cur.children.Count == 0)
            {
                cur.isPath = false;
                //cur.wached = true;
                return false;
            }
            foreach (Node child in cur.children)
            {
                
                child.f = _funk(child, g+1);
            }

            bool result = false;

            while (result == false)
            {
                //if (cur._unwatched_childrens() == 0)
                //{
                //    cur.isPath = false;
                //    return false;
                //}
                //Node best = _find_best(cur);
                int best = 0;
                int alt = 0;
                _find_alt_and_best(cur, ref best, ref alt);

                if (fLimit < cur.children[best].f)
                {
                    //cur.wached = false;
                    cur.isPath = false;
                    fLimit = cur.children[best].f;
                    return false;
                }
                //Node alter = new Node(); 
                //if (cur.children.Count != 1)
                //{
                //    alter = _find_alternative(cur, best);
                //}    
                

                cur.isPath = true;
                Console.Clear();
                draw_maze();
                Thread.Sleep(700);

                fLimit = cur.children[alt].f;
                result = _recursive_RDFS(cur.children[best], ref fLimit, g+1);
                cur.children[best].f = fLimit;
            }
            return result;
        }

        public bool RDFS()
        {
            double fLimit = int.MaxValue;
            return _recursive_RDFS(maze[0,0], ref fLimit, 0);
        }
    }
}
