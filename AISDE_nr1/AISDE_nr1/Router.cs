using System;
using System.Collections.Generic;
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
        public Stream[] streams;
        public Heap2<Times> heap;

        public struct Times : IComparable
        {
            public int time;
            public int action;
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

        public Router(int C, int K, int N)
        {
            channel_capacity = C;
            number_of_buffers = K;
            number_of_streams = N;
            flag = true;
            buffers = new Buffer[number_of_buffers];
            streams = new Stream[number_of_streams];
            for (int i = 0; i<number_of_buffers; i++)
            {
                buffers[i] = new Buffer();
                buffers[i].SetSize(1000);
            }
            for (int i=0; i<number_of_streams; i++)
            {
                streams[i] = new Stream();
                streams[i].priority = i;
                streams[i].lambda = 0.2;
            }
            heap = new Heap2<Times>();


        }

        public void packetToBuffer(Stream stream)
        {
            Packet packet = stream.GeneratePacket(stream.lambda);
            Times times = new Times();
            int t = heap.first().time;
            times.time = stream.GenerateTime(stream.lambda) + t;
            times.action = (int)actions.packet_to_buffer;
            if (heap.counter!=0)
                heap.Delete();
            heap.Add(times);
            buffers[stream.buffer_number].Add(packet);
            if (flag == false)
                send();
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
                    if(heap.counter!=0)
                        heap.Delete();
                    heap.Add(times);
                    buffers[i].PullOut();
                    flag = true;
                    return true;
                }
            }
            flag = false;
            return false; 
        }                 
        
    }
}
