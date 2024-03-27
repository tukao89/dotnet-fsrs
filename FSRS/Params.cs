using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSRS
{
    public class Params
    {
        public double RequestRetention { get; set; }
        public int MaximumInterval { get; set; }
        public double[] W { get; set; }

        public Params()
        {
            RequestRetention = 0.9;
            MaximumInterval = 36500;
            W = new double[]
            {
            0.4, 0.6, 2.4, 5.8, 4.93, 0.94, 0.86, 0.01, 1.49, 0.14, 0.94, 2.18, 0.05,
            0.34, 1.26, 0.29, 2.61,
            };
        }
    }

}
