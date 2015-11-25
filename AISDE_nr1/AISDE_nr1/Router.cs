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
        // ROUTER DATA FIELDS
        int channel_capacity;  //przepustowosc lacza
        int number_of_buffers;  //ilosc buforow
        int number_of_streams;  //ilosc strumieni
        bool flag;   //0, jezeli send zwrocil false
        enum actions { packet_to_buffer, send };

        Buffer[] buffers;       // zbiór kolejek
        string[] names_of_streams;      // zbiór nazw strumieni
        Stream[] streams;       // zbiór strumieni
        Heap2<Times> heap;      // kolejka zdarzeń

        Times new_send;         // zmienna sterujaca operacja wysylania
        double wyslane = 0;

        // STATISTICS FIELDS
        Stopwatch simulation_timer;
        int[] packets_counter;
        int[] losted_packets_counter;
        int sended_packet_counter;
        TimeSpan simulation;
        double packet_processing_mil;
        double simulation_mil;
        double last_link_modify_time;
        double[] last_buffer_modify_time;
        double free_link2;
        double[] free_buffers2;
        double simulation_time;
        
        

        // EVENT DATA MEDIUM
        private struct Times : IComparable
        {
            public double time;
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

        // ROUTER INITIALIZATION
        public Router()
        {
            ReadFromFile();
        }

        // ROUTER SIMULATION INITIALIZATION
        public void Simulation(double sim_time)
        {
            // Symulacja
            new_send.time = sim_time;
            simulation_time = sim_time;
            simulation_timer.Start();
            while (heap.first().time < sim_time)
            {
                int n = heap.first().action;
                if (new_send.time <= heap.first().time)
                {
                    heap.Add(new_send);
                    new_send.time = sim_time;
                    continue;
                }
                if (n == (int)Router.actions.packet_to_buffer)
                {
                    packetToBuffer(heap.first().stream_id);
                }
                else if (n == (int)Router.actions.send)
                {
                    send();
                }

            }
            for(int i = 0; i < number_of_buffers; i++)
            {
                // Statistics collecting

                while(true)
                {
                    if (buffers[i].table[buffers[i].first].adding_time < simulation_time && (buffers[i].last!=-1))
                    {
                        free_buffers2[i] += (simulation_time - buffers[i].table[buffers[i].first].adding_time) * (double)buffers[i].table[buffers[i].first].size;
                        buffers[i].PullOut();
                    }
                    else
                        break;
                }
                    
            }
            simulation_timer.Stop();
            simulation = simulation_timer.Elapsed;
            simulation_mil = simulation.TotalMilliseconds;

            // Wypisywanie danych wyjściowych na ekran
            Console.WriteLine("WYNIKI SYMULACJI ROUTERA");
            Console.Write("Całkowity czas symulacji: ");
            Console.WriteLine(simulation_mil + " ms");
            Console.WriteLine();

            Console.Write("Średnia zajętość łącza: ");
            if (free_link2 / simulation_time * 100 < 100)
                Console.WriteLine(100 - free_link2 / simulation_time * 100 + "%");
            else
                Console.WriteLine("0% (Przy dużych przepustowościach błędy jakimi obarczone są operacje arytmetyczne w systemie dyskretnym uniemożliwiają dokładne wyliczenie wartości, jednak różni się ona pomijalnie od zera, więc przyjmujemy wartość zero)");
            Console.WriteLine();

            Console.WriteLine("Średnie zajętości kolejek (jeżeli 0%, to wartość na tyle zbliżona do zera, że jest to pomijalne)");
            for (int i = 0; i < number_of_buffers; i++)
            {
                Console.Write("Średnia zajętość kolejki KOL{0}: ", i);
                Console.WriteLine(free_buffers2[i] / simulation_time + " bitow srednio ");
            }
            Console.WriteLine();

            Console.WriteLine("Prawdopodobieństwo odrzucenia pakietu dla poszczególnych strumieni");
            for (int i = 0; i < number_of_streams; i++)
            {
                Console.Write("Prawdopodobieństwo odrzucenia pakietu dla strumienia {0}: ", names_of_streams[i]);
                Console.WriteLine((double)losted_packets_counter[i] / packets_counter[i] * 100 + "%");
            }
            Console.WriteLine();

            Console.Write("Średni czas przetwarzania pakietu w systemie(tylko wysłane): ");
            Console.WriteLine((packet_processing_mil/sended_packet_counter) + " ms");
            Console.WriteLine();
            Console.WriteLine(heap.zdarzenia);
            Console.WriteLine(wyslane);
            Console.WriteLine(heap.zdarzenia - wyslane);
            Console.WriteLine(buffers[0].counter);

            

            // Generowanie pliku wyjściowego
            WriteToFile();
        }

        // PACKETS QUEUING HANDLER
        private void packetToBuffer(int stream_id)
        {
            // Local variables initialization
            Packet packet = streams[stream_id].GeneratePacket();
            Times times = new Times();
            double t = heap.first().time;
            double pom = streams[stream_id].GenerateTime();

            // Here you can test router logic using const values by uncomment next to lines
            //packet.size = 4000;
            //pom = 0.5;

            // Events queue handling
            packet.adding_time = t + pom;
            times.time = pom + t;
            times.action = (int)actions.packet_to_buffer;
            times.stream_id = stream_id;
            if (heap.counter!=0 && heap.first().stream_id==stream_id)
                heap.Delete();
            heap.Add(times);

            int size = buffers[streams[stream_id].buffer_number].data_size;
            // Statistics collecting
            packets_counter[stream_id]++;

            if (buffers[streams[stream_id].buffer_number].Add(packet))      // Adding packet to buffer
            {
                // Send event handling
                if (flag == false)
                {
                    int id = streams[stream_id].buffer_number;
                    if (new_send.time <= t || new_send.time==simulation_time)
                    {
                        Times times1 = new Times();
                        times1.action = 1;
                        times1.time = buffers[id].table[buffers[id].first].adding_time + 0.000001;
                        new_send = times1;
                    }
                    else if (new_send.time > times.time + ((double)buffers[id].table[buffers[id].first].size) * 1000 / channel_capacity)
                    {
                        new_send.time = buffers[id].table[buffers[id].first].adding_time + 0.000001;
                    }
                }
                
            }
            else
            {
                losted_packets_counter[stream_id]++;        // Statistics of losted packets
            }
        }

        // PACKETS SENDING HANDLER
        private bool send()
        {
            Times times = new Times();
            for (int i = 0; i < number_of_buffers; i++)
            {
                if (buffers[i].data_size > 0)       // Looking for not empty buffers in priority order
                {
                    // Local variables intialization
                    double t = heap.first().time;
                    double tmp = ((double)buffers[i].table[buffers[i].first].size) * 1000 / channel_capacity;

                    // Checking if the packet has already arrived
                    if (t < buffers[i].table[buffers[i].first].adding_time)
                        continue;

                    // Events queue handling
                    times.time = tmp + t;
                    times.action = (int)actions.send;
                    if (heap.counter!=0 && heap.first().action==1)
                        heap.Delete();
                    heap.Add(times);

                    // Statistics collecting
                    if (!flag)
                        free_link2 = free_link2 + t - last_link_modify_time;
                    packet_processing_mil += tmp + t - buffers[i].table[buffers[i].first].adding_time;
                    if(tmp != 0)
                        sended_packet_counter++;

                    // Statistics collecting
                    free_buffers2[i] = free_buffers2[i] + (t + tmp - buffers[i].table[buffers[i].first].adding_time) * (double)buffers[i].table[buffers[i].first].size;

                    // Sending packet out of router
                    buffers[i].PullOut();

                    // Send event handling
                    flag = true;
                    wyslane++;
                    return true;
                }
            }
            // Statistics collecting
            last_link_modify_time = heap.first().time;

            // Events queue handling
            if (heap.counter != 0 && heap.first().action == 1)
                heap.Delete();

            // Send event handling
            flag = false;
            return false; 
        }                 
        
        // LOADING INPUT DATA FROM FILE
        private void ReadFromFile()
        {
            Console.WriteLine("Przeciagnij plik inicjalizacyjny i wcisnij enter:");
            string str = Console.ReadLine();
            if (str[0] == '"')
               str = str.Substring(1,str.Length-2);
            //string str = "F:\\Multimedia\\Dokumenty\\Studia\\PW Notatki\\Semestr 3\\AISDE\\Projekty\\Projekt1\\AISDE_nr1\\AISDE_nr1\\bin\\Debug\\Download\\Input\\router.txt";
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
            string[] buffers_names = new string[number_of_buffers];
            for(int i=0;i< number_of_buffers; i++)
            {
                tmp = filestream.ReadLine();
                words = tmp.Split(' ');
                lbuffers[i]= int.Parse(words[5]);
                buffers_names[i] = words[2];
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
            // End of file reading

            // Setting up initial values
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
                    if("WYK"+(j+1)==time_lambdas[i])
                        streams[i].time_lambda = lambdas[j];
                for (int j = 0; j < R; j++)
                    if ("WYK" + (j+1) == size_lambdas[i])
                        streams[i].size_lambda = lambdas[j];
                for (int j = 0; j < number_of_buffers; j++)
                    if ("KOL" + (j + 1) == names_of_buffers[i])
                        streams[i].buffer_number = j;
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
            last_buffer_modify_time = new double[number_of_buffers];
            free_buffers2 = new double[number_of_buffers];
            for (int i = 0; i < number_of_buffers; i++)
            {
                last_buffer_modify_time[i] = new double();
                free_buffers2[i] = new double();
            }
            losted_packets_counter = new int[number_of_streams];
            packets_counter = new int[number_of_streams];
            for (int i = 0; i < number_of_streams; i++)
            {
                packets_counter[i] = new int();
                losted_packets_counter[i] = new int();
            }
            
            packet_processing_mil = new double();

            last_link_modify_time = new double();
            free_link2 = new double();
        }

        // WRITING OUTPUT DATA INTO FILE
        private void WriteToFile()
        {
            System.IO.StreamWriter filestream = new System.IO.StreamWriter("wyniki.txt", false);

            filestream.WriteLine("WYNIKI SYMULACJI ROUTERA");
            filestream.Write("Całkowity czas symulacji: ");
            filestream.WriteLine(simulation_mil + " ms");
            filestream.WriteLine();

            filestream.Write("Średnia zajętość łącza: ");
            if(free_link2 / simulation_time * 100<100)
                filestream.WriteLine(100 - free_link2 / simulation_time * 100 + "%");
            else
                filestream.WriteLine("0% (Przy dużych przepustowościach błędy jakimi obarczone są operacje arytmetyczne w systemie dyskretnym uniemożliwiają dokładne wyliczenie wartości, jednak różni się ona pomijalnie od zera, więc przyjmujemy wartość zero)");
            filestream.WriteLine();

            filestream.WriteLine("Średnie zajętości kolejek (jeżeli 0%, to wartość na tyle zbliżona do zera, że jest to pomijalne)");
            for (int i = 0; i < number_of_buffers; i++)
            {
                filestream.Write("Średnia zajętość kolejki KOL{0}: ",i);
                filestream.WriteLine(free_buffers2[i] / simulation_time + " bitów znajdowalo sie srednio w kolejce ");
            }
            filestream.WriteLine();

            filestream.WriteLine("Prawdopodobieństwo odrzucenia pakietu dla poszczególnych strumieni");
            for (int i = 0; i < number_of_streams; i++)
            {
                filestream.Write("Prawdopodobieństwo odrzucenia pakietu dla strumienia {0}: ", names_of_streams[i]);
                filestream.WriteLine((double)losted_packets_counter[i] / packets_counter[i] * 100 + "%");
            }
            filestream.WriteLine();

            filestream.Write("Średni czas przetwarzania pakietu w systemie(tylko wysłane): ");
            filestream.WriteLine((packet_processing_mil / sended_packet_counter) + " ms");

            filestream.Close();

        }
    }
}
