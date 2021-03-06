﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_nr1
{
    class Stream
    { 
        public int priority;
        public double time_lambda;
        public double size_lambda;
        public int buffer_number;
        RandomValueGenerator random_value_generator;

        public Stream()
        {
            random_value_generator = new RandomValueGenerator();
            priority = 1;
            time_lambda = 1;
            size_lambda = 1;
            buffer_number = 1;
        }

        public Stream(int number)
        {
            random_value_generator = new RandomValueGenerator();
            priority = 1;
            time_lambda = 1;
            size_lambda = 1;
            buffer_number = number;
        }

        public Packet GeneratePacket()
        {
            Packet packet = new Packet();
            double size = random_value_generator.Exp_dist(size_lambda/8);
            packet.size = (int)Math.Ceiling(size);
            packet.priority = priority;
            return packet;
        }
        public double GenerateTime()
        {
            double t = random_value_generator.Exp_dist(time_lambda);
            return t*1000;
        }
    }
}
