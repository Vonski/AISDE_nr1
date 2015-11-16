using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_nr1
{
    class Stream
    { 
        public int priority;
        public double lambda;
        public int buffer_number;
        RandomValueGenerator random_value_generator;

        public Stream()
        {
            random_value_generator = new RandomValueGenerator();
            priority = 1;
            lambda = 1;
            buffer_number = 1;
        }

        public Stream(int number)
        {
            random_value_generator = new RandomValueGenerator();
            priority = 1;
            lambda = 1;
            buffer_number = number;
        }

        /*public Stream(int priority)
        {   
           random_value_generator = new RandomValueGenerator();
           this.priority = priority;
        }*/

        public Packet GeneratePacket(double lambda)
        {
            Packet packet = new Packet();
            double size = random_value_generator.Exp_dist(lambda);
            packet.size = (int)Math.Ceiling(size);
            packet.priority = priority;
            return packet;
        }
        public int GenerateTime(double lambda)
        {
            double t = random_value_generator.Exp_dist(lambda);
            int time = (int)Math.Ceiling(t);
            return time;
        }
    }
}
