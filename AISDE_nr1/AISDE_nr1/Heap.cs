using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp
{
    class Heap<ElementType> : IPriorityQueue<ElementType>
    {
        //Fields:
        public ElementType[] table = new ElementType[1];
        int counter = 0;


        //Methods:
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
            PushUp(counter - 1);

        }

        public void Delete()
        {
            int last = --counter;
            table[0] = table[last];
            table[last] = default(ElementType);
            PushDown(0);
        }

        public void WriteOut()
        {
            foreach (ElementType current in table)
            {
                System.Console.WriteLine(current);
            }
        }

        private void PushUp(int i)
        {
            var result = Comparer<ElementType>.Default.Compare(table[i], table[(i - 1) / 2]);

            while (i >= 1 && result > 0)
            {
                ElementType tmp = table[i];
                table[i] = table[(i - 1) / 2];
                table[(i - 1) / 2] = tmp;
                i = (i - 1) / 2;
                result = Comparer<ElementType>.Default.Compare(table[i], table[(i - 1) / 2]);
            }

        }

        private void PushDown(int i)
        {

            while (2 * i + 2 <= counter - 1)
            {
                var result = Comparer<ElementType>.Default.Compare(table[2 * i + 1], table[2 * i + 2]);
                if (result > 0)
                {
                    var result1 = Comparer<ElementType>.Default.Compare(table[i], table[2 * i + 1]);
                    if (result1 < 0)
                    {
                        ElementType tmp = table[i];
                        table[i] = table[2 * i + 1];
                        table[2 * i + 1] = tmp;
                        i = 2 * i + 1;
                    }
                    else
                        break;
                }
                else
                {
                    var result2 = Comparer<ElementType>.Default.Compare(table[i], table[2 * i + 2]);
                    if (result2 < 0)
                    {
                        ElementType tmp = table[i];
                        table[i] = table[2 * i + 2];
                        table[2 * i + 2] = tmp;
                        i = 2 * i + 2;
                    }
                    else
                        break;
                }

            }

            if (2 * i + 1 == counter - 1)
            {
                var result3 = Comparer<ElementType>.Default.Compare(table[i], table[2 * i + 1]);
                if (result3 < 0)
                {
                    ElementType tmp = table[i];
                    table[i] = table[2 * i + 1];
                    table[2 * i + 1] = tmp;
                }
            }

        }

        public int length()
        {
            return counter;
        }
    }
    
}

