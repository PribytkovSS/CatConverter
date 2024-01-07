using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatConverter
{
    public class PointLoad
    {
        public PointLoad() { }
        /// <summary>
        /// Point of load application, cm
        /// </summary>
        public Point3D Point { get; set; }
        /// <summary>
        /// Load value, kN
        /// </summary>
        public double Value { get; set; }
    }
}
