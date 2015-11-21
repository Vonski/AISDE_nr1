using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_nr1
{
    
    class Router
    {
        int channel_capacity;  //przepustowosc lacza
        int number_of_buffers;  //ilosc buforow
        int number_of_streams;  //ilosc strumieni
        bool flag;   //0, jezeli send zwrocil false
        public enum actions { packet_to_buffer, send };

        Buffer[] buffers;
        public string[] names_of_streams;
        public Stream[] streams;
        public Heap2<Times> heap;

        // STATISTICS FIELDS
        Stopwatch simulation_timer;
        Stopwatch free_link_timer;
        Stopwatch[] free_buffers_timer;
        int[] packets_counter;
        int[] losted_packets_counter;
        Stopwatch packet_processing_timer;
        TimeSpan simulation;
        TimeSpan free_link;
        TimeSpan[] free_buffers;
        TimeSpan packet_processing;
        double free_link_mil;
        double[] free_buffers_mil;
        double[] free_buffers_size;
        double packet_processing_mil;


        public struct Times : IComparable
        {
            public int time;
            public int action;
            public int stream_id;
            int IComparable.CompareTo(object obj)
            {
                Times times = (Times)obj;
                return this.time.CompareTo(times.time);
            }
            override public string ToString()
            {
                return time.ToString();
            }
            
        }

        public Router()
        {
            ReadFromFile();
        }

        public void Simulation(int sim_time)
        {
            int p = 0, s = 0;
            simulation_timer.Start();
            for (int i = 0; i < number_of_buffers; i++)
            {
                free_buffers_timer[i].Restart();
            }
            while (heap.first().time < sim_time)
            {
                int n = heap.first().action;
                if (n == (int)Router.actions.packet_to_buffer)
                {
                    packetToBuffer(heap.first().stream_id);
                    p++;
                }
                else if (n == (int)Router.actions.send)
                {
                    send();
                    s++;
                }

            }
            for(int i = 0; i < number_of_buffers; i++)
            {
                free_buffers_timer[i].Stop();
                free_buffers[i] = free_buffers_timer[i].Elapsed;
                free_buffers_mil[i] = free_buffers[i].TotalMilliseconds;
                free_buffers_size[i] += free_buffers_mil[i] * buffers[i].buffer_size;
            }
            simulation_timer.Stop();
            heap.WriteOut();
            simulation = simulation_timer.Elapsed;
            double z = simulation.TotalMilliseconds;
            Console.WriteLine();
            Console.WriteLine(z);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(free_link_mil/z*100);
            Console.WriteLine(p);
            Console.WriteLine(s);
            Console.WriteLine();
            for (int i = 0; i < number_of_buffers; i++)
            {
                Console.WriteLine(free_buffers_size[i]/buffers[i].buffer_size/z*100);
            }
            Console.WriteLine();
            for (int i = 0; i < number_of_streams; i++)
            {
                Console.WriteLine((double)losted_packets_counter[i]/packets_counter[i] * 100);
            }
            Console.WriteLine();
            Console.WriteLine(packet_processing_mil);
        }

        public void packetToBuffer(int stream_id)
        {
            Packet packet = streams[stream_id].GeneratePacket();
            Times times = new Times();
            int t = heap.first().time;
            int pom = streams[stream_id].GenerateTime();
            times.time = pom + t;
            times.action = (int)actions.packet_to_buffer;
            times.stream_id = stream_id;
            if (heap.counter!=0 && heap.first().stream_id==stream_id)
                heap.Delete();
            heap.Add(times);
            int size = buffers[streams[stream_id].buffer_number].data_size;
            packets_counter[stream_id]++;
            if (buffers[streams[stream_id].buffer_number].Add(packet))
            {
                free_buffers_timer[streams[stream_id].buffer_number].Stop();
                free_buffers[streams[stream_id].buffer_number] = free_buffers_timer[streams[stream_id].buffer_number].Elapsed;
                free_buffers_mil[streams[stream_id].buffer_number] = free_buffers[streams[stream_id].buffer_number].TotalMilliseconds;
                free_buffers_size[streams[stream_id].buffer_number] += free_buffers_mil[streams[stream_id].buffer_number] * size;
                free_buffers_timer[streams[stream_id].buffer_number].Restart();
                if (packet_processing_mil != 0)
                    packet_processing_mil = (packet_processing_mil + packet.size) / 2;
                else
                    packet_processing_mil = (packet_processing_mil + packet.size);
            }
            else
            {
                losted_packets_counter[stream_id]++;
            }
            if (flag == false)
            {
                send();
                free_link_timer.Stop();
                free_link = free_link_timer.Elapsed;
                free_link_mil += free_link.TotalMilliseconds;
            }
        }

        public bool send()
        {
            Times times = new Times();
            for (int i = 0; i < number_of_buffers; i++)
            {
                if (buffers[i].data_size > 0)
                {
                    int t = heap.first().time;
                    times.time = ((buffers[i].table[buffers[i].last].size)*1000 / channel_capacity) + t;
                    times.action = (int)actions.send;
                    if (heap.counter!=0 && heap.first().action==1)
                        heap.Delete();
                    heap.Add(times);
                    free_buffers_timer[i].Stop();
                    free_buffers[i] = free_buffers_timer[i].Elapsed;
                    free_buffers_mil[i] = free_buffers[i].TotalMilliseconds;
                    free_buffers_size[i] += free_buffers_mil[i] * buffers[i].buffer_size;
                    buffers[i].PullOut();
                    free_buffers_timer[i].Restart();
                    flag = true;
                    return true;
                }
            }
            free_link_timer.Restart();
            if (heap.counter != 0 && heap.first().action == 1)
                heap.Delete();
            flag = false;
            return false; 
        }                 
        
        public void ReadFromFile()
        {
            Console.WriteLine("Przeciagnij plik inicjalizacyjny i wcisnij enter:");
            //string str = Console.ReadLine();
            //if (str[0] == '"')
            //    str = str.Substring(1,str.Length-2);
            string str = "F:\\Multimedia\\Dokumenty\\Studia\\PW Notatki\\Semestr 3\\AISDE\\Projekty\\Projekt1\\AISDE_nr1\\AISDE_nr1\\bin\\Debug\\Download\\Input\\router.txt";
            FileStream fs = new FileStream(str, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            System.IO.StreamReader filestream = new System.IO.StreamReader(fs);

            string tmp = "";
            string[] words;

            bool logic = true;
            while (logic)
            {
                tmp = filestream.ReadLine();
                if(!string.IsNullOrEmpty(tmp))
                    if (tmp[0] != '#')
                    {
                        logic = false;
                    }
            }
            logic = true;
            while (logic)
            {
                tmp = filestream.ReadLine();
                if (!string.IsNullOrEmpty(tmp))
                    if (tmp[0] != '#')
                    {
                        logic = false;
                    }
            }
            words = tmp.Split(' ');
            channel_capacity = int.Parse(words[2]);
            logic = true;
            while (logic)
            {
                tmp = filestream.ReadLine();
                if (!string.IsNullOrEmpty(tmp))
                    if (tmp[0] != '#')
                    {
                        logic = false;
                    }
            }
            words = tmp.Split(' ');
            number_of_buffers = int.Parse(words[2]);
            int[] lbuffers = new int[number_of_buffers];
            for(int i=0;i< number_of_buffers; i++)
            {
                tmp = filestream.ReadLine();
                words = tmp.Split(' ');
                lbuffers[i]= int.Parse(words[5]);
            }
            logic = true;
            while (logic)
            {
                tmp = filestream.ReadLine();
                if (!string.IsNullOrEmpty(tmp))
                    if (tmp[0] != '#')
                    {
                        logic = false;
                    }
            }
            words = tmp.Split(' ');
            int R = int.Parse(words[2]);
            float[] lambdas = new float[R];
            for (int i = 0; i < R; i++)
            {
                tmp = filestream.ReadLine();
                tmp = filestream.ReadLine();
                words = tmp.Split(' ');
                words[2] = words[2].Replace(".",",");
                lambdas[i] = float.Parse(words[2]);
            }
            logic = true;
            while (logic)
            {
                tmp = filestream.ReadLine();
                if (!string.IsNullOrEmpty(tmp))
                    if (tmp[0] != '#')
                    {
                        logic = false;
                    }
            }
            words = tmp.Split(' ');
            number_of_streams = int.Parse(words[2]);
            names_of_streams = new string[number_of_streams];
            string[] names_of_buffers = new string[number_of_streams];
            string[] time_lambdas = new string[number_of_streams];
            string[] size_lambdas = new string[number_of_streams];
            for (int i = 0; i < number_of_streams; i++)
            {
                tmp = filestream.ReadLine();
                words = tmp.Split(' ');
                names_of_streams[i] = words[2];
                names_of_buffers[i] = words[5];
                time_lambdas[i] = words[8];
                size_lambdas[i] = words[11];
            }
            filestream.Close();
            // Koniec pobierania z pliku

            flag = true;
            buffers = new Buffer[number_of_buffers];
            streams = new Stream[number_of_streams];
            for (int i = 0; i < number_of_buffers; i++)
            {
                buffers[i] = new Buffer();
                buffers[i].SetSize(lbuffers[i]);
            }
            for (int i = 0; i < number_of_streams; i++)
            {
                streams[i] = new Stream();
                for (int j = 0; j < R; j++)
                    if("WYK"+j.ToString()==time_lambdas[j])
                        streams[i].time_lambda = lambdas[j];
                for (int j = 0; j < R; j++)
                    if ("WYK" + j.ToString() == time_lambdas[j])
                        streams[i].size_lambda = lambdas[j];
                streams[i].buffer_number = i;
            }
            heap = new Heap2<Times>();
            Times times1 = new Times();
            times1.action = 1;
            times1.time = 0;
            heap.Add(times1);
            for (int i = 0; i < number_of_streams; i++)
            {
                Times times = new Times();
                times.action = 0;
                times.time = 0;
                times.stream_id = i;
                heap.Add(times);
            }

            // Statistics init
            simulation_timer = new Stopwatch();
            free_link_timer = new Stopwatch();
            free_buffers_timer = new Stopwatch[number_of_buffers];
            free_buffers = new TimeSpan[number_of_buffers];
            free_buffers_mil = new double[number_of_buffers];
            free_buffers_size = new double[number_of_buffers];
            for (int i = 0; i < number_of_buffers; i++)
            {
                free_buffers_timer[i] = new Stopwatch();
                free_buffers_timer[i].Start();
                free_buffers_timer[i].Stop();
                free_buffers[i] = new TimeSpan();
                free_buffers_mil[i] = new double();
                free_buffers_size[i] = new double();
            }
            losted_packets_counter = new int[number_of_streams];
            packets_counter = new int[number_of_streams];
            for (int i = 0; i < number_of_streams; i++)
            {
                packets_counter[i] = new int();
                losted_packets_counter[i] = new int();
            }
            packet_processing_timer = new Stopwatch();

            free_link_timer.Start();
            free_link_timer.Stop();
            packet_processing_timer.Start();
            packet_processing_timer.Stop();

            free_link_mil = new double();
            packet_processing_mil = new double();
        }

    }
}
