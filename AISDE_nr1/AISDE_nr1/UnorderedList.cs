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
                int j = table.Length * 10;
                ElementType[] table_tmp = new ElementType[j];
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
            ElementType[] tmp = new ElementType[1];
            ElementType[] tmp2 = new ElementType[1];
            tmp[0] = table[0];
            int min = 0;
            for (int i = 1; i < counter; i++)
            {
                tmp[0] = (Comparer<ElementType>.Default.Compare(table[i], tmp[0]) < 0) ? table[i] : tmp[0];
                if (((Comparer<ElementType>.Default.Compare(table[i], tmp[0]) == 0) ? true : false) && ((Comparer<ElementType>.Default.Compare(tmp[0], tmp2[0]) != 0) ? true : false))
                {
                    min = i;
                }
            }
            for (int i = min; i < counter - 1; i++)
            {
                table[i] = table[i + 1];
            }
            table[table.Length - 1] = tmp2[0];
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
