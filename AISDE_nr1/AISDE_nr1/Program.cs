using C_sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_nr1
{
    class Program
    {
        static void Main(string[] args)
        {
/*            System.Console.WriteLine("Hello world!");

           UnorderedList<int> boom = new UnorderedList<int>();

            boom.Add(2);
            boom.Add(1);
            boom.Add(8);
            boom.WriteOut();
            System.Console.WriteLine("Hello world!");
            boom.Delete();
            boom.WriteOut();*/

            Heap<int> heap = new Heap<int>();
            heap.Add(9);
            heap.Add(8);
            heap.Add(7);
            heap.Add(6);
            heap.Add(5);
            heap.Add(4);
            heap.Add(3);
            heap.Add(2);
            heap.Add(1);
            heap.Add(0);
            heap.WriteOut();
            System.Console.WriteLine("\n");
            heap.Delete();
            heap.WriteOut();
            System.Console.WriteLine("\n");

            heap.Delete();
            heap.WriteOut();
            System.Console.WriteLine("\n");
            heap.Delete();
            heap.WriteOut();

            
            System.Console.Read();
        }
    }
}
