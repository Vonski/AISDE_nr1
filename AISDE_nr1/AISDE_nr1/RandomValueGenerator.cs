using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_nr1
{
    class RandomValueGenerator
    {
        static Random seed = new Random();

        double value;

        public double Exp_dist(double lambda)
        {
            value = -Math.Log((seed.NextDouble())) / lambda;

            return value;
        }
    }
}
