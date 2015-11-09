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
        Packet packet;
        RandomValueGenerator random_value_generator;

        public Packet GeneratePacket(double lambda)
        {
            Packet packet = new Packet();
            RandomValueGenerator random_value_generator = new RandomValueGenerator();
            double size = random_value_generator.Exp_dist(lambda);
            packet.size = (int)Math.Ceiling(size);
            packet.priority = priority;
            return packet;
        }
        public int GenerateTime(double lambda)
        {
            RandomValueGenerator random_value_generator = new RandomValueGenerator();
            double t = random_value_generator.Exp_dist(lambda);
            int time = (int)Math.Ceiling(t);
            return time;
        }
    }
}
