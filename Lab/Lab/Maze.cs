using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;



namespace Lab
{
    class Maze
    {
        private long _byteInGig = 1024 * 1024 * 1024;
        private long _msIn30Min = 1800000;
        private long _used_mem = sizeof(int) * 2 + sizeof(double) + sizeof(bool) * 6;
        private class Node
        {
            public bool wached = false;
            public bool isPath = false;

            public bool top = true;
            public bool right = true;
            public bool bottom = true;
            public bool left = true;

            public int[] corde;

            public double f = double.MaxValue;
        }

        private Node _start = new Node();
        private Node _finish = new Node();
        private Node[,] _maze;
        private int _size;

        private Stopwatch time;

        public int[] GetFinishCorde()
        {
            return _finish.corde;
        }

        public int[] GetStartCorde()
        {
            return _start.corde;
        }

        //-----------Maze-----------//
        private List<Node> _get_neighbours(int y_cur, int x_cur)
        {
            List<Node> neighbours = new List<Node>();
            int[] cord_t = new int[2] { y_cur - 1, x_cur };
            int[] cord_l = new int[2] { y_cur, x_cur - 1 };
            int[] cord_b = new int[2] { y_cur + 1, x_cur };
            int[] cord_r = new int[2] { y_cur, x_cur + 1 };

            List<int[]> temp = new List<int[]>() { cord_t, cord_l, cord_b, cord_r };

            for (int i = 0; i < 4; i++)
            {
                if (temp[i][0] >= 0 && temp[i][0] < _size && temp[i][1] >= 0 && temp[i][1] < _size)
                {
                    if (_maze[temp[i][0], temp[i][1]].wached == false)
                    {
                        neighbours.Add(_maze[temp[i][0], temp[i][1]]);
                    }
                }
            }
            return neighbours;
        }

        private void _dell_wall(Node fir, Node sec)
        {
            if (fir.corde[0] < sec.corde[0])
            {
                _maze[fir.corde[0], fir.corde[1]].bottom = false;
                _maze[sec.corde[0], sec.corde[1]].top = false;
                return;
            }
            if (fir.corde[0] > sec.corde[0])
            {
                _maze[fir.corde[0], fir.corde[1]].top = false;
                _maze[sec.corde[0], sec.corde[1]].bottom = false;
                return;
            }
            if (fir.corde[1] < sec.corde[1])
            {
                _maze[fir.corde[0], fir.corde[1]].right = false;
                _maze[sec.corde[0], sec.corde[1]].left = false;
                return;
            }
            if (fir.corde[1] > sec.corde[1])
            {
                _maze[fir.corde[0], fir.corde[1]].left = false;
                _maze[sec.corde[0], sec.corde[1]].right = false;
                return;
            }
        }

        private void _make_false()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _maze[i, j].wached = false;
                }
            }
        }

        public Maze(int size)
        {
            this._size = size;

            _maze = new Node[this._size, this._size];

            _create_empty_maze();

            time = new Stopwatch();
            time.Start();
        }

        private void _create_empty_maze()
        {
            Random rand = new Random();
            //bool end;
            //if (rand.Next(21) <= 5)
            //{
            //    end = false;
            //    _finish.corde = new int[2] { int.MaxValue, int.MaxValue };
            //}
            //else
            //{
            //    end = true;
            //}

            if (rand.Next(2) == 0)
            {
                _start.corde = new int[2] { rand.Next(_size), 0 };
                //if(end == true)
                _finish.corde = new int[2] { rand.Next(_size), _size - 1 };
            }
            else
            {
                _start.corde = new int[2] { 0,rand.Next(_size) };
                //if (end == true)
                _finish.corde = new int[2] { _size - 1, rand.Next(_size) };
            }

            for (int i = 0; i < this._size; i++)
            {
                for (int j = 0; j < this._size; j++)
                {
                    if (i == _start.corde[0] && j == _start.corde[1])
                    {
                        _maze[i, j] = _start;
                    }
                    else if (i == _finish.corde[0] && j == _finish.corde[1])
                    {
                        _maze[i, j] = _finish;
                    }
                    else
                    {
                        _maze[i, j] = new Node();
                        _maze[i, j].corde = new int[2] { i, j };
                    }
                }
            }
        }

        public void generate_maze()
        {
            Stack<Node> node_stack = new Stack<Node>();
            Node current = _start;
            _start.wached = true;
            Node neighbour_node;
            int unwached = _size * _size-1;
            Random rand = new Random();
            while (unwached != 0)
            { 

                List<Node> neighbours = _get_neighbours(current.corde[0], current.corde[1]);
                if (neighbours.Count != 0)
                {
                    if (current.corde[0] == _finish.corde[0] && current.corde[1] == _finish.corde[1])
                    {
                        _maze[current.corde[0], current.corde[1]].wached = true;
                        current = node_stack.Pop();
                    }
                    else
                    {
                        int index = rand.Next(neighbours.Count);
                        neighbour_node = neighbours[index];
                        _maze[neighbour_node.corde[0], neighbour_node.corde[1]].wached = true;
                        unwached -= 1;
                        _dell_wall(current, neighbour_node);

                        node_stack.Push(current);
                        current = neighbour_node;
                    }
                }
                else if (node_stack.Count > 0)
                {
                    current = node_stack.Pop();
                }
            }

            _make_false();
        }

        public void draw_maze()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < _size + _size + 1; i++)
            {
                Console.Write("--");
            }
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < _size; i++)
            {
                // side wall
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write('|');
                Console.ResetColor();
                for (int j = 0; j < _size; j++)
                {
                    if (_start.corde[0] == i && _start.corde[1] == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" S ");
                        Console.ResetColor();
                    }
                    else if (_finish.corde[0] == i && _finish.corde[1] == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" E ");
                        Console.ResetColor();
                    }
                    else if (_maze[i, j].isPath == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" P ");
                        Console.ResetColor();
                    }
                    else
                        Console.Write(" * ");
                    if (_maze[i, j].right == true)
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
                if (i != _size - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write('|');
                    Console.ResetColor();
                    for (int j = 0; j < _size; j++)
                    {
                        if (_maze[i, j].bottom == true)
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

            for (int i = 0; i < _size + _size + 1; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("--");
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        //-----------DLS-----------//
        private Result _DLS(int max_depth, Result res)
        {
            return _recursive_DLS(max_depth, 0, _start, res);
        }

        private Result _recursive_DLS(int max_depth, int depth, Node cur, Result res)
        {
            if (res.cStateInMemory * _used_mem > _byteInGig || time.ElapsedMilliseconds > _msIn30Min)
            {
                res.failure = true;
                res.cutOff = false;

                return res;
            }

            bool cutoff_occurred = false;
            if (depth >= max_depth)
            {
                res.succes = false;
                res.cutOff = true;
                res.failure = false;
                res.cStateInMemory--;
                //res.corner++;
                return res;
            }
            List<Node> children = _find_children(cur);
            if (children.Count == 0)
            {
                res.succes = false;
                res.cStateInMemory--;
                return res;
            }

            res.cState += children.Count;
            res.cStateInMemory += children.Count;

            foreach (Node childe in children)
            {
                if (childe.corde[0] == _finish.corde[0] && childe.corde[1] == _finish.corde[1])
                {
                    cur.isPath = true;
                    res.succes = true;
                    //res.cStateInMemory++;
                    return res;
                }
                else
                {
                    //depth++;
                    res.iterations++;
                    cur.isPath = true;
                    res = _recursive_DLS(max_depth, depth+1, childe, res);
                    if (res.succes)
                    {
                        return res;
                    }
                    else if (res.cutOff == true)
                    {
                        cutoff_occurred = true;
                    }

                   // cur.isPath = false;
                }
            }
            res.cStateInMemory--;
            //depth--;
            cur.isPath = false;
            if (cutoff_occurred == true)
            {
                res.succes = false;
                return res;
            }
            else
            {
                res.failure = true;
                res.succes = false;
                return res;
            }    
        }

        public Result IDS()
        {
            int max_depth = 0;
            Result result = new Result();
            while (true)
            {
                result = _DLS(max_depth, result);
                if (result.succes != false)
                {
                    time.Stop();
                    result.time = time.ElapsedMilliseconds;
                    return result;
                }
                else if(result.failure == true)
                {
                    time.Stop();
                    result.time = time.ElapsedMilliseconds;
                    result.corner++;
                    return result;
                }
                result.corner++;
                result.cutOff = false;
                result.failure = false;
                result.cStateInMemory = 0;
                max_depth++;
            }
        }

        //-----------RBFS-----------//
        private double _funk(Node cur, int g)
        {
            return _h(cur) + g;
        }

        private double _h(Node node)
        {
            if (node.corde[0] == _finish.corde[0] && node.corde[1] == _finish.corde[1])
            {
                return 0;
            }
            return Math.Sqrt(Math.Pow(2,_finish.corde[0] - node.corde[0]) + Math.Pow(2, _finish.corde[1] - node.corde[1]));
        }

        private void _find_best(List<Node> children, ref int best)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[best].f > children[i].f)
                {
                    best = i;
                }
            }
        }

        private void _find_alternative(List<Node> children, ref int best, ref int alt)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if ((children[alt].f > children[i].f && i != best) || children[alt].f == children[best].f)
                {
                    alt = i;
                }
            }
        }

        private List<Node> _find_children(Node node)
        {
            List<Node> children = new List<Node>();
            if (node.top == false && _maze[node.corde[0] - 1, node.corde[1]].isPath != true)
            {
                children.Add(_maze[node.corde[0] - 1, node.corde[1]]);
            }
            if (node.left == false && _maze[node.corde[0], node.corde[1] - 1].isPath != true)
            {
                children.Add(_maze[node.corde[0], node.corde[1] - 1]);
            }
            if (node.bottom == false && _maze[node.corde[0] + 1, node.corde[1]].isPath != true)
            {
                children.Add(_maze[node.corde[0] + 1, node.corde[1]]);
            }
            if (node.right == false && _maze[node.corde[0], node.corde[1] + 1].isPath != true)
            {
                children.Add(_maze[node.corde[0], node.corde[1] + 1]);
            }
            return children;
        }

        private Result _recursive_RDFS(Node cur,  double fLimit, int g, Result result)
        {

            result.cStateInMemory++;
            cur.isPath = true;
            List<Node> children = _find_children(cur);

            if (cur.corde[0] == _finish.corde[0] && cur.corde[1] == _finish.corde[1])
            {
                result.succes = true;
                //result.cStateInMemory++;
                return result;
            }
            if (children.Count == 0)
            {
                cur.isPath = false;
                result.cStateInMemory--;
                result.fLimit = int.MaxValue;
                return result;
            }

            if (children[0].f == double.MaxValue)
            {
                foreach (Node child in children)
                {
                    child.f = _funk(child, g + 1);
                }
            }

            result.cState+= children.Count;

            while (true)
            {
                result.iterations++;

                int best = 0;
                int alt = 0;
                _find_best(children, ref best);

                if (fLimit < children[best].f)
                {
                    cur.isPath = false;
                    result.fLimit = children[best].f;
                    result.cStateInMemory--;
                    return result;
                }

                if (children.Count != 1)
                { 
                    _find_alternative(children, ref best, ref alt);
                    fLimit = Math.Min(fLimit, children[alt].f);
                }

                result = _recursive_RDFS(children[best], fLimit, g+1, result);
                children[best].f = result.fLimit;

                if (result.succes != false)
                {
                    //result.cStateInMemory++;
                    return result;
                }
            }
        }

        public Result RDFS()
        {
            double fLimit = int.MaxValue;
            _start.f = 0;
            Result res = new Result();
            res = _recursive_RDFS(_start, fLimit, 0, res);

            if (res.succes == false)
            {
                res.corner++;
            }

            time.Stop();
            res.time = time.ElapsedMilliseconds;
            return res;
        }
    }
}
