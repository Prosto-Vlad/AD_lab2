using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab
{
    class Result
    {
        public bool succes = false;
        public double fLimit;
        public bool cutOff = false;
        public bool failure = false;

        public int corner = 0;
        public int iterations = 0;
        public int cState = 0;
        public int cStateInMemory = 0;

        public float time;
    }
}
