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
            Fifo<Packet> boom = new Fifo<Packet>();

            boom.setSize(10);
            for(int i = 0; i < 6; i++)
            {
                Packet z = new Packet();
                z.size = i;
                boom.Add(z);
            }

            boom.WriteOut();
            Console.WriteLine(" ");

            for (int i = 0; i < 3; i++)
            {
                boom.Delete();
            }

            Console.WriteLine(" ");
            boom.WriteOut();
            Console.WriteLine(" ");

            for (int i = 6; i < 16; i++)
            {
                Packet z = new Packet();
                z.size = i;
                boom.Add(z);
            }

            Console.WriteLine(" ");
            boom.WriteOut();
            Console.WriteLine(" ");

            for (int i = 0; i < 11; i++)
            {
                boom.Delete();
            }

            Console.Read();
        }
    }
}
