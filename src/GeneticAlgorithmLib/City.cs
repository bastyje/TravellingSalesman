using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class City
    {
        public int X { get; }
        public int Y { get; }
        public string Name { get; }
        public City(string name, int x, int y)
        {
            X = x;
            Y = y;
            Name = name;
        }
    }
}
