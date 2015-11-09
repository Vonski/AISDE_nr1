using C_sharp;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_nr1
{
    class Program
    {
       static int M = 100, N = 100, A = 1, B = 10;

        static void Test<T>(T queue)
            where T: IPriorityQueue<Element>
        {
            Element element = new Element();
            Random random = new Random();

            for (int i = 0; i < A; i++)
            {
                element.SetKey(random.Next(1, M));
                queue.Add(element);
            }

            for (int i=0; i<B; i++)
            {
                queue.Delete();
                element.SetKey(random.Next(1, N));
                queue.Add(element);
            }

        }

        static void Main(string[] args)
        {
           /* int option = 1;
            if (option == 1)
            {
                try
                {
                    using (StreamReader sr = new StreamReader("input.txt"))
                    {
                        M = int.Parse(sr.ReadLine());
                        N = int.Parse(sr.ReadLine());
                        A = int.Parse(sr.ReadLine());
                        B = int.Parse(sr.ReadLine());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }*/

                System.IO.StreamWriter filestream_list = new System.IO.StreamWriter("timelist.txt", true);
                System.IO.StreamWriter filestream_heap = new System.IO.StreamWriter("timeheap.txt", true);
                Stopwatch stopwatch = new Stopwatch();
                UnorderedList<Element> unordered_list = new UnorderedList<Element>();
                Heap<Element> heap = new Heap<Element>();

                while(A<=500)
                {               
                    
                    stopwatch.Start();
                    Test<UnorderedList<Element>>(unordered_list);
                    stopwatch.Stop();
                    TimeSpan x = stopwatch.Elapsed;


                    stopwatch.Restart();
                    Test<Heap<Element>>(heap);
                    stopwatch.Stop();
                    TimeSpan y = stopwatch.Elapsed;

                    
                    string timelist = x.TotalMilliseconds.ToString();
                    string timeheap = y.TotalMilliseconds.ToString();

                    filestream_list.WriteLine(timelist);
                    filestream_heap.WriteLine(timeheap);
                   
                    A++;
                }
                filestream_list.Close();
                filestream_heap.Close();
                

                /*string elapsedTimeList = String.Format("{0:00}min {1:00}s {2:00}ms", x.Minutes, x.Seconds, x.Milliseconds);
                string elapsedTimeHeap = String.Format("{0:00}min {1:00}s {2:00}ms", y.Minutes, y.Seconds, y.Milliseconds);

                try
                {
                    string path = "output.txt";
                    using (FileStream fs = File.Create(path))
                    {
                        Byte[] output = new UTF8Encoding(true).GetBytes(
                            "M=" + M + "\r\nN=" + N + "\r\nA=" + A + "\r\nB=" + B +
                            "\r\nUnordered List: " + elapsedTimeList + "\r\nHeap: " + elapsedTimeHeap);
                        fs.Write(output, 0, output.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        */

            
        }

        
    }
}
