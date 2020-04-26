using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace icris.SolarEdgeDownloader
{
    public class Measurement
    {
        public DateTime Timestamp { get; set; }
        public double? Power { get; set; }
        public double? Energy { get; set; }
    }
}
