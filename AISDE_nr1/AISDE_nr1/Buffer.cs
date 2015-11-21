using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_nr1
{
    class Buffer
    {
        public Packet[] table = new Packet[1000];
        public int first;
        public int last;
        private int counter = 0;
        public int buffer_size;
        public int data_size;

        public Buffer()
        {
            first = 0;
            last = -1;
            buffer_size = 0;
            data_size = 0;
            for (int i = 0; i < table.Length; i++)
                table[i] = new Packet();
        }

        public void SetSize(int size)
        {
            buffer_size = size;
        }

        public bool Add(Packet new_packet)
        {
            if (counter+1 == table.Length)
                return false;

            if (data_size + new_packet.size > buffer_size)
            {
                Console.WriteLine("Za mala pojemnosc bufora");
                return false;
            }

            if (last==table.Length-1)
                last = 0;
            else
                last++;

            
            table[last] = new_packet;
            counter++;
            data_size += new_packet.size;

            return true;
        }

        public Packet PullOut()
        {
            Packet z = new Packet();
            if (first == last || last==-1)
            {
                if (counter == 1)
                {
                    counter--;
                    data_size -= table[last].size;
                    first = 0;
                    last = -1;
                }
                return z;
            }
                

            counter--;
            int tmp = first;
            if (first < table.Length - 1)
                first++;
            else if (first < table.Length)
                first = 0;

            data_size -= table[tmp].size;

            return table[tmp];
        }

        public void WriteOut()
        {
            if (last == -1)
                Console.WriteLine("Kolejka pusta");
            else if (first > last)
            {
                for (int i = first; i < table.Length; i++)
                    Console.WriteLine(table[i].size);
                for (int i = 0; i < last; i++)
                    Console.WriteLine(table[i].size);
            }
            else if(first==last)
                    Console.WriteLine(table[last].size);
            else
                for (int i = first; i <= last; i++)
                    Console.WriteLine(table[i].size);
            Console.WriteLine(" ");
        }
    }
}
