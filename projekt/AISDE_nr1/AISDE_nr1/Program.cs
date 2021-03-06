﻿using System;
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
       static int M = 1000, N = 1000, A = 1, B = 10000;

        static void Test<T>(T queue)
            where T: IPriorityQueue<Element>
        {
            Element element = new Element();
            Element element2 = new Element();

            Random random = new Random();

            for (int i = 0; i < A; i++)
            {
                element2 = (Element)element.Clone();
                element2.SetKey(random.Next(1, M));
                queue.Add(element2);
            }

            for (int i=0; i<B; i++)
            {
                element2 = (Element)element.Clone();
                queue.Delete();
                element2.SetKey(random.Next(1, N));
                queue.Add(element2);
            }

        }

        static void Main(string[] args)
        {
            int option = 2;
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

                //---------------------------------------------------------------------(DO WYKRESU)
                /*System.IO.StreamWriter filestream_list = new System.IO.StreamWriter("timelist.txt", false);
                System.IO.StreamWriter filestream_heap = new System.IO.StreamWriter("timeheap.txt", false);
                Stopwatch stopwatch = new Stopwatch();
                UnorderedList<Element> unordered_list = new UnorderedList<Element>();
                Heap2<Element> heap = new Heap2<Element>();
                A=1;
                           
                while(A<=10000)
                {               
                    stopwatch.Restart();
                    Test<UnorderedList<Element>>(unordered_list);
                    stopwatch.Stop();
                    TimeSpan x = stopwatch.Elapsed;


                    stopwatch.Restart();
                    Test<Heap2<Element>>(heap);
                    stopwatch.Stop();
                    TimeSpan y = stopwatch.Elapsed;

                    
                    string timelist = x.TotalMilliseconds.ToString();
                    string timeheap = y.TotalMilliseconds.ToString();

                    filestream_list.WriteLine(timelist);
                    filestream_heap.WriteLine(timeheap);
                   
                    A=A+10;
                }
                filestream_list.Close();
                filestream_heap.Close();*/
                            
                //---------------------------------------------------------------------------------

                              
            }

            else if(option==2)
            {
                Router router = new Router();
                double t;
                Console.Write("Podaj czas symulacji(w sekundach): ");
                t = double.Parse(Console.ReadLine());
                t*=1000;
                router.Simulation(t);

                Console.WriteLine(t);

                Console.ReadLine();
                Console.ReadLine();

            }
        }
    }
}
