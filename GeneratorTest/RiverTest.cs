using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Generator;

namespace GeneratorTest
{
    public class RiverTest : IDisposable
    {
        double[,] _map = new double[,]
            {
                {0.1 , 0.2 , 0.3},
                {0.4 , 0.5 , 0.6},
                {0.7 , 0.8 , 0.9},
            };
        double[,] digitalized = new double[,]
        {
                {0 , 0 , 0},
                {0.5 , 0.5 , 0.5},
                {1 , 1 , 1},
        };
        Map test;
        public RiverTest()
        {
            test = new Map();
            test.size = 3;
            test.seed = 23123;
            test.digitalized = digitalized;
            test.map = _map;
            //test.AllRivers = new List<RiversCollection>();
        }
        public void Dispose()
        { }

        [Fact]
        public void findStartingPointTest()
        {
           // int[] result = River.findStartingPoint(test);
           // Console.Write(result[0] + "  " + result[1]);
        }
    }
}
