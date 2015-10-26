using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp
{
    class UnorderedList<ElementType> : IPriorityQueue<ElementType>
    {
        // Fields:
        public ElementType[] table = new ElementType[1];
        int counter = 0;

        // Methods:
        public void Add(ElementType element)
        {
            if (table.Length == counter)
            {
                ElementType[] table_tmp = new ElementType[table.Length * 10];
                for (int i = 0; i < table.Length; i++)
                {
                    table_tmp[i] = table[i];
                }
                table = table_tmp;
            }
            table[counter++] = element;
        }

        public void Delete()
        {
            ElementType tmp = table[0];
            int min = 0;
            for (int i = 1; i < table.Length; i++)
            {
                tmp = (Comparer<ElementType>.Default.Compare(table[i], tmp) < 0) ? table[i] : tmp;
                if (((Comparer<ElementType>.Default.Compare(table[i], tmp) == 0) ? true : false) && ((Comparer<ElementType>.Default.Compare(tmp, default(ElementType)) != 0) ? true : false))
                {
                    min = i;
                }
            }
            for (int i = min; i < table.Length - 1; i++)
            {
                table[i] = table[i + 1];
            }
            table[table.Length - 1] = default(ElementType);
        }

        public void WriteOut()
        {
            foreach (ElementType current in table)
            {
                System.Console.WriteLine(current);
            }
        }

    }
}
