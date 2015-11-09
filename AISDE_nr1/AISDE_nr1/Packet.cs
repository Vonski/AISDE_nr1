using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_nr1
{
    class Packet : IComparable
    {
        public int size;
        public int priority;

        public Packet()
        {
            size = 1;
            priority = 1;
        }

        private class sortSizeHelper : IComparer<Packet>
        {
            int IComparer<Packet>.Compare(Packet a, Packet b)
            {
                Packet c1 = new Packet();
                Packet c2 = new Packet();

                if (c1.size > c2.size)
                    return 1;

                if (c1.size < c2.size)
                    return -1;

                else
                    return 0;
            }
        }

        public static IComparer <Packet> sortSize()
        {
            return (IComparer<Packet>)new sortSizeHelper();
        }

        int IComparable.CompareTo(object obj)
        {
            Packet p = (Packet)obj;
            return this.size.CompareTo(p.size);
        }

        

    }
}
