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
               /* try
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

                //---------------------------------------------------------------------(DO WYKRESU)
                //System.IO.StreamWriter filestream_list = new System.IO.StreamWriter("timelist.txt", false);
                for(int n=0; n<50;n++)
                {
                    string str = n.ToString() + ".txt";
                System.IO.StreamWriter filestream_heap = new System.IO.StreamWriter(str, false);
                Stopwatch stopwatch = new Stopwatch();
                UnorderedList<Element> unordered_list = new UnorderedList<Element>();
                Heap<Element> heap = new Heap<Element>();
                           
                while(A<=10000)
                {               
                    /*if(A==1)
                    {
                        for(int n=0; n<2; n++)
                        {
                            stopwatch.Restart();
                            Test<UnorderedList<Element>>(unordered_list);
                            stopwatch.Stop();
                            TimeSpan a = stopwatch.Elapsed


                            stopwatch.Restart();
                            Test<Heap<Element>>(heap);
                            stopwatch.Stop();
                            TimeSpan b = stopwatch.Elapsed;


                           // string timelist1 = a.TotalMilliseconds.ToString();
                            string timeheap1 = b.TotalMilliseconds.ToString();

                            //filestream_list.WriteLine(timelist1);
                            filestream_heap.WriteLine(timeheap1);
                        }
                    }*/

                    /*stopwatch.Restart();
                    Test<UnorderedList<Element>>(unordered_list);
                    stopwatch.Stop();
                    TimeSpan x = stopwatch.Elapsed;*/


                    stopwatch.Restart();
                    Test<Heap<Element>>(heap);
                    stopwatch.Stop();
                    TimeSpan y = stopwatch.Elapsed;

                    
                    //string timelist = x.TotalMilliseconds.ToString();
                    string timeheap = y.TotalMilliseconds.ToString();

                   // filestream_list.WriteLine(timelist);
                    filestream_heap.WriteLine(timeheap);
                   
                    A=A+10;
                }
                //filestream_list.Close();
                filestream_heap.Close();
                A = 1;
            }
                
                //---------------------------------------------------------------------------------

                /*Stopwatch stopwatch = new Stopwatch();
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
                            "\r\nUnordered List: " + elapsedTimeList + "\r\nHeap: " + elapsedTimeHeap);*/

               
            }

            else if(option==2)
            {
                Router router = new Router(2000, 4, 4);
                router.packetToBuffer(0);
                router.packetToBuffer(0);
                router.packetToBuffer(1);
                router.packetToBuffer(2);
                router.packetToBuffer(3);
                router.SendIni(router.heap.first().time);
                router.send();
                router.heap.WriteOut();
                Console.WriteLine();
                int p = 0, s = 0;
                for (int i = 0; i < 400; i++)
                {
                    int n = router.heap.first().action;
                    if(n == (int)Router.actions.packet_to_buffer)
                    {
                        router.packetToBuffer(router.heap.first().stream_id);
                        p++;
                    }
                    else if(n == (int)Router.actions.send)
                    {
                        router.send();
                        s++;
                    }          

                }
                router.heap.WriteOut();
                Console.WriteLine();
                Console.WriteLine(p);
                Console.WriteLine(s);

                    Console.Read();

                

            }
        

            
        }

        
    }
}
