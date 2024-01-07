using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatConverter
{
    public class Rebar
    {
        public Rebar() { }        
        /// <summary>
        /// Number of rebars deflection points
        /// </summary>
        public Point3D[] Deflections { get; set; };
        /// <summary>
        /// Cross section area of a rebar, sq. cm.
        /// </summary>
        public double CrossSectionArea { get; set; }
        
    }
}
