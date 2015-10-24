using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp
{
    interface IPriorityQueue<ElementType>
    {
        void Add(ElementType element);
        void Delete();
        void WriteOut();
    }
}
