using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath
{
    public class Node
    {
        private string name;
        private Node a;
        private int weight;
        private bool visited;

        public Node(string name)
        {
            this.name = name;
            this.weight = int.MaxValue;
        }
        public Node A
        {
            get { return a; }
            set { a = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        public bool Visited
        {
            get { return visited; }
            set { visited = value; }
        }
        public override string ToString()
        {
            return name.ToString();
        }
    }
}
