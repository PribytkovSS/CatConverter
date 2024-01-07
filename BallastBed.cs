using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatConverter
{
    public class BallastBed
    {
        public BallastBed(IEnumerable<double> rawCoordinates)
        {
            ShapePoints = new Point3D[rawCoordinates.Count() / 2];
            for (int i = 0; i <  ShapePoints.Length; i++) 
            {
                ShapePoints[i] = new Point3D() { Z = rawCoordinates.ElementAt(i * 2), Y = rawCoordinates.ElementAt(i * 2 + 1) };
            }
        }

        public Point3D[] ShapePoints { get; set; } = Array.Empty<Point3D>();
    }
}
