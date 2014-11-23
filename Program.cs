using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShortestPath
{
    class Program
    {
        static List<Node> Nodes = new List<Node>();
        static List<Line> Lines = new List<Line>();
        public static List<List<Node>> PointerArray;
        public static List<Node> Tree;
        public static Stack<Node> Result;
        static List<string> paths = new List<string>();
        static string path="";
        static StringBuilder sb = new StringBuilder();

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the name of the flights file :");
            string path1 = Console.ReadLine();
            Parser(path1);
            Console.WriteLine("Enter the name of the tasks file :");
            string path2 = Console.ReadLine();
            ParseTaskFile(path2);
            WriteFile(sb);
            Console.Write("\nPress Any Key to Exit.");
            Console.ReadLine();
        }


        //Task 2
        public static void FindPath(Node node, int length, ref List<string> paths, string preds)
        {
            preds += node.Name + ",";
            
            if (length == 0)
            {
                preds = preds.Remove(preds.Length - 1);
                paths.Add(preds);
                return;
            }
            node.Visited = true;
            List<Node> childs = PointerArray[GetIndexOfNode(node.Name)];
            foreach (Node child in childs)
            {               
                    FindPath(child, length-1, ref paths, preds);
            }            
        }

        //Task 3
        public static void FindLongestCycle(Node node, Node source, ref string path, string preds)
        {
            preds += node.Name + ",";
            if (source.Name == node.Name && preds.Length>2)
            {
                preds = preds.Remove(preds.Length - 1);
                if (path.Length < preds.Length)
                    path = preds;
                return;
            }
            node.Visited = true;
            List<Node> childs = PointerArray[GetIndexOfNode(node.Name)];
            foreach (Node child in childs)
            {
                if (!child.Visited) 
                FindLongestCycle(child, source, ref path, preds);
            }
        }

        //Finding the shortest path from an airport and saves all the possiblile flight list in a tree
        public static List<Node> dijkstra(Node start)
        {
            List<Node> S = new List<Node>();
            foreach (Node node in Nodes)
            {
                node.Weight = int.MaxValue;
            }
            //CalcPointerArray();
            start.A = null;
            start.Weight = 0;
            Node xPilot = start;
            S.Add(xPilot);

            while ((S.Count != Nodes.Count) && (xPilot.Weight != int.MaxValue))
            {
                List<Node> klew = PointerArray[GetIndexOfNode(xPilot.Name)];
                for (int j = 0; j < klew.Count; j++)
                {
                    if(!nodeExists(klew[j].Name,S))                    
                    {
                        int i = GetIndexOfLine(xPilot, klew[j]);
                        if (klew[j].Weight > xPilot.Weight + Lines.ElementAt(i).Duration)
                        {
                            Nodes.Find(p => p.Name == klew[j].Name).Weight = xPilot.Weight + Lines.ElementAt(i).Duration;
                            Nodes.Find(p => p.Name == klew[j].Name).A = xPilot;
                        }
                    }
                }
                int min = int.MaxValue;
                foreach (Node nodex in Nodes)
                {
                    if ((!S.Contains(nodex)) && (nodex.Weight < min))
                    {
                        min = nodex.Weight;
                        xPilot = nodex;
                    }
                }
                S.Add(xPilot);
            }
            return S;
        }

        //Calculates the array that has all flights flying from each Airport
        public static void CalcPointerArray()
        {
            PointerArray = new List<List<Node>>(Nodes.Count);//[Nodes.Count];
            for (int i = 0; i < Nodes.Count; i++)
            {
                PointerArray.Add(new List<Node>());
                for (int j = 0; j < Lines.Count; j++)
                {
                    if (Lines.ElementAt(j).FirstNode.Name == Nodes.ElementAt(i).Name)
                    {
                        PointerArray[i].Add(Lines.ElementAt(j).SecondNode);
                    }
                }
            }
        }

        //Method that gets the index number of an airport from the Airports list
        public static int GetIndexOfNode(String nodename)
        {
            foreach(Node node in Nodes)
            {
                if (node.Name == nodename)
                {
                    return Nodes.IndexOf(node);
                }
            }
            return -1;
        }

        //Method that gets the number of the corrosponding flight between two airports
        public static int GetIndexOfLine(Node source, Node destination)
        {
            foreach(Line line in Lines)
            {
                if ((line.FirstNode.Name == source.Name) && (line.SecondNode.Name == destination.Name))
                    return line.Id;
            }
            return -1;
        }
        public static void ParseTaskFile(string path)
        {
            //Reading the essential lines found in the Tasks.txt file
            StreamReader reader = new StreamReader(path);
            string line1 = reader.ReadLine();
            string line2 = reader.ReadLine();
            string line3 = reader.ReadLine();
            string[] Str1 = line1.Split(',');
            string[] Str2 = line2.Split(',');

            //Calculating the array that contains each airport's destinations list
            CalcPointerArray();

            Console.WriteLine("Task 1\n");
            //Task 1 Finding all the possible shortest paths between two airports using Dijekstra algorithm
            Node startNode = Nodes.ElementAt(GetIndexOfNode(Str1[0]));
            Node endNode = Nodes.ElementAt(GetIndexOfNode(Str1[1]));
            Tree = dijkstra(startNode);
            int overAllDuration = endNode.Weight;
            int index = Tree.IndexOf(endNode);

            //Printing the airpirts in the shortest path from the stack 
            if (nodeExists(endNode.Name, Tree))
            {
                Result = new Stack<Node>();

                Result.Push(endNode);
                while (endNode != startNode)
                {
                    endNode = endNode.A;
                    Result.Push(endNode);
                }
            }
            string shortestpath="";
            while (Result.Count != 0)
            {
                shortestpath = Result.Pop().Name;
                sb.Append(shortestpath + " ");
                Console.Write(shortestpath + " ");
            }
            sb.AppendLine("\n");
            //Console.WriteLine("\nDuration: " + overAllDuration);

            Console.WriteLine("\nTask 2\n");
            //Find a flight from an airport with a specific length
            startNode = Nodes.ElementAt(GetIndexOfNode(Str2[0]));
            int length = Convert.ToInt32(Str2[1]);            
            FindPath(startNode, length, ref paths, "");
            foreach (string pathx in paths)
            {
                Console.WriteLine(pathx);
                sb.AppendLine(pathx);
            }


            Console.WriteLine("Task 3\n");
            //Find the Longest Cycle or roundtrip that starts from an airport and returns to the same airport.
            startNode = Nodes.ElementAt(GetIndexOfNode(line3));
            UnvisitNodes();
            FindLongestCycle(startNode, startNode, ref path, "");
            Console.WriteLine(path);
            sb.AppendLine(path);
        }

        //Write the results to the Solutions file in the same folder
        static void WriteFile (StringBuilder sb)
        {
            using (StreamWriter outfile = new StreamWriter("Solutions.txt"))
            {
                outfile.Write(sb.ToString());
            }
        }

        //Parser to read the flights.txt and construct the graph with airports and flights
        static void Parser(string path)
        {
            StreamReader reader = new StreamReader(path);
            string stringLine;
            int i = 0;


            while ((stringLine  = reader.ReadLine()) != null)
            {
                string[] Str1 = new string[3];
                Str1 = stringLine.Split(',');                

                Node node1 = new Node(Str1[0]);
                Node node2 = new Node(Str1[1]);
                int lineWeight = Convert.ToInt32(Str1[2]);
                Line line = new Line(node1, node2, lineWeight);
                
                if (!nodeExists(Str1[0], Nodes))
                {
                    Nodes.Add(node1);
                }
                if (!nodeExists(Str1[1], Nodes))
                {
                    Nodes.Add(node2);
                }
                Lines.Add(line);
                i++;
            }
        }

        static bool nodeExists(string name, List<Node> list)
        {
            foreach (Node node in list)
            {
                if (node.ToString() == name)
                {
                    return true;
                }
            }
            return false;
        }

        static void UnvisitNodes()
        {
            for (int i = 0; i < PointerArray.Count; i++)
            {
                foreach (Node node in PointerArray[i])
                {
                    node.Visited = false;
                }
            }
        }
    }
}
