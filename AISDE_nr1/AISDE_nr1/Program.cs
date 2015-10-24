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
            System.Console.WriteLine("Hello world!");

            UnorderedList<int> boom = new UnorderedList<int>();

            boom.Add(2);
            boom.Add(1);
            boom.Add(8);
            boom.WriteOut();
            System.Console.WriteLine("Hello world!");
            boom.Delete();
            boom.WriteOut();

            
            System.Console.Read();
        }
    }
}
