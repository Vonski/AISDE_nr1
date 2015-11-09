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
       static int M = 0, N = 0, A = 0, B = 0;

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
            int option = 1;
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
                }

               /* System.IO.StreamWriter filestream_list = new System.IO.StreamWriter("timelist.txt", false);
                System.IO.StreamWriter filestream_heap = new System.IO.StreamWriter("timeheap.txt", false);
                Stopwatch stopwatch = new Stopwatch();
                UnorderedList<Element> unordered_list = new UnorderedList<Element>();
                Heap<Element> heap = new Heap<Element>();

                           
                while(A<=1000)
                {               
                    if(A==1)
                    {
                        for(int n=0; n<2; n++)
                        {
                            stopwatch.Restart();
                            Test<UnorderedList<Element>>(unordered_list);
                            stopwatch.Stop();
                            TimeSpan a = stopwatch.Elapsed;


                            stopwatch.Restart();
                            Test<Heap<Element>>(heap);
                            stopwatch.Stop();
                            TimeSpan b = stopwatch.Elapsed;


                            string timelist1 = a.TotalMilliseconds.ToString();
                            string timeheap1 = b.TotalMilliseconds.ToString();

                            filestream_list.WriteLine(timelist1);
                            filestream_heap.WriteLine(timeheap1);
                        }
                    }

                    stopwatch.Restart();
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
                filestream_heap.Close();*/

                Stopwatch stopwatch = new Stopwatch();
                UnorderedList<Element> unordered_list = new UnorderedList<Element>();
                Heap<Element> heap = new Heap<Element>();

                stopwatch.Restart();
                Test<UnorderedList<Element>>(unordered_list);
                stopwatch.Stop();
                TimeSpan x = stopwatch.Elapsed;


                stopwatch.Restart();
                Test<Heap<Element>>(heap);
                stopwatch.Stop();
                TimeSpan y = stopwatch.Elapsed;

                string elapsedTimeList = x.TotalMilliseconds.ToString() + "ms";
                string elapsedTimeHeap = y.TotalMilliseconds.ToString() + "ms";

                System.IO.File.WriteAllText("output.txt", "M=" + M + "\r\nN=" + N + "\r\nA=" + A + "\r\nB=" + B +
                            "\r\nUnordered List: " + elapsedTimeList + "\r\nHeap: " + elapsedTimeHeap);
                
            }
        

            
        }

        
    }
}
