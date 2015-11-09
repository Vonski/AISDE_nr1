using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_nr1
{
    class Element :IComparable
    {
        private class ComparingKeysHelper :IComparer<Element>
        {
            int IComparer<Element>.Compare(Element e1, Element e2)
            {
                if (e1.Key>e2.Key)
                    return 1;
                else if (e1.Key<e2.Key)
                    return -1;
                else
                    return 0;
            }
        }

        private int Key;

        public void SetKey(int i)
        {
            Key = i;
        }

        public static IComparer<Element> ComparingKeys()
        {
            return (IComparer<Element>)new ComparingKeysHelper();
        }

        int IComparable.CompareTo(object obj)
        {
            Element e2 = (Element)obj;
            return this.Key.CompareTo(e2.Key);
            
        }

    }

}
