using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath
{
    class Line
    {
        private static int number;
        private Node firstNode;
        private Node secondNode;
        private int weight;
        private int id = 0;


        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        public Node FirstNode
        {
            get { return firstNode; }
            set { firstNode = value; }
        }        

        public Node SecondNode
        {
            get { return secondNode; }
            set { secondNode = value; }
        }

        public int Duration
        {
            get { return weight; }
            set { weight = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public Line(Node firstNode, Node secondNode, int weight)
        {

            this.id = number;
            this.firstNode = firstNode;
            this.secondNode = secondNode;
            this.weight = weight;
            Number++;
        }
    }
}
