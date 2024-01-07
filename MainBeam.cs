using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatConverter
{
    public class MainBeam
    {
        public MainBeam(double axisZ, bool FirstOrLast, bool SingleBeam, List<double> numbers, ref int numIndex)
        {
            AxisZ = axisZ;
            if (FirstOrLast)
            {
                SidewalkDeadLoad = numbers[numIndex]; numIndex++;
                SidewalkZ = numbers[numIndex]; numIndex++;
                RailingDeadLoad = numbers[numIndex]; numIndex++;
                RailingZ = numbers[numIndex]; numIndex++;
            }
            // Slab reinforcement
            var slabRebarsCount = (int)numbers[numIndex]; numIndex++;
            
            for (var i = 0; i < slabRebarsCount; i++)
            {
                var rebar = new Rebar();
                SlabRebars.Add(rebar);
                rebar.Deflections = new Point3D[(int)numbers[numIndex]];                
                numIndex++;
            }

            for (var i = 0; i < slabRebarsCount; i++)
            {
                var rebar = SlabRebars[i];
                rebar.CrossSectionArea = numbers[numIndex]; numIndex++;
            }

            for (var i = 0; i < slabRebarsCount; i++)
            {
                var rebar = SlabRebars[i];
                for (var j = 0; j < rebar.Deflections.Length; j++)
                {
                    rebar.Deflections[j].Z = numbers[numIndex]; numIndex++;
                    rebar.Deflections[j].Y = numbers[numIndex]; numIndex++;
                }                
            }

            var pointLoadCount = (int)numbers[numIndex]; numIndex++;
            for (var i = 0; i < pointLoadCount; i++)
            {
                var pl = new PointLoad();
                var x = numbers[numIndex]; numIndex++;
                var v = numbers[numIndex]; numIndex++;
                pl.Point = new Point3D() { X = x };
                pl.Value = v;
                PointLoads.Add(pl);
            }

            var csCount = (int)numbers[numIndex]; numIndex++;
            for (int i = 0; i < csCount; i++)
            { 
                var x = numbers[numIndex]; numIndex++;
                AnalyzedCrossSectionX.Add(x);
            }

            // Cross section (shape) points indices
            // First two points define slab-to-beam connection
            // Next two point define slab haunch end
            var slabToBeam1 = (int)numbers[numIndex]; numIndex++;
            var slabToBeam2 = (int)numbers[numIndex]; numIndex++;
            var beamHaunch1 = (int)numbers[numIndex]; numIndex++;
            var beamHaunch2 = (int)numbers[numIndex]; numIndex++;

            // Ballast bed flange points - if it is a first or last beam
            if (!SingleBeam)
            {
                if (FirstOrLast)
                {
                    var flangeToSlab1 = (int)numbers[numIndex]; numIndex++;
                    var flangeToSlab2 = (int)numbers[numIndex]; numIndex++;
                }
            } else
            {
                // If a suprestructure has single beam
                var leftFlangeToSlab1 = (int)numbers[numIndex]; numIndex++;
                var leftFlangeToSlab2 = (int)numbers[numIndex]; numIndex++;
                var rightFlangeToSlab1 = (int)numbers[numIndex]; numIndex++;
                var rightFlangeToSlab2 = (int)numbers[numIndex]; numIndex++;
            }

            var leftSep = (int)numbers[numIndex] == 1; numIndex++;
            var rightSep = (int)numbers[numIndex] == 1; numIndex++;

            var csPointsCount = (int)numbers[numIndex];
            var symmetric = csPointsCount != 9999; numIndex++;
            if (symmetric)
                csCount = (csCount + 1) / 2;

            for (var i = 0; i < csCount; i++)
            {
                var PointIndices = new int[csCount];
                for (var j = 0; j < PointIndices.Length; j++) PointIndices[j] = j;

                if (i == 0 && !symmetric)
                    csPointsCount = (int)numbers[numIndex]; numIndex++;

                if (i > 0)
                {
                    csPointsCount = (int)numbers[numIndex]; numIndex++;
                    for (var j = 0; j < csPointsCount; j++)
                    {
                        PointIndices[j] = (int)numbers[numIndex]; numIndex++;
                    }
                }

                for (var j = 0; j < csPointsCount; j++)
                {

                }
            }
        }
        /// <summary>
        /// Z coordinate (in lateral direction) of beam longitudinal axis 
        /// </summary>
        public double AxisZ { get; set; }
        /// <summary>
        /// Sidewalk dead load value, kN/m
        /// </summary>
        public double SidewalkDeadLoad { get; set; }
        /// <summary>
        /// Z coordinate of sidewalk axis - load application
        /// </summary>
        public double SidewalkZ { get; set; }
        /// <summary>
        /// Railing dead load value, kN/m
        /// </summary>
        public double RailingDeadLoad { get; set; }
        /// <summary>
        /// Z coordinate of sidewalk railing axis - load application
        /// </summary>
        public double RailingZ { get; set; }
        /// <summary>
        /// Slab reinforcement bars 
        /// </summary>
        public List<Rebar> SlabRebars { get; set; } = new List<Rebar>();
        /// <summary>
        /// Point loads applied
        /// </summary>
        public List<PointLoad> PointLoads { get; set; } = new List<PointLoad>();
        /// <summary>
        /// X-coordinate (longitudinal) of cross sections to analyze
        /// </summary>
        public List<double> AnalyzedCrossSectionX { get; set; } = new List<double>();  
    }
}
