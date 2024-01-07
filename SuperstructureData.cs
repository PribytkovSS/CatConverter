using CatConverter.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatConverter
{
    internal class SuperstructureData
    {
        public SuperstructureData(string sourceFile) 
        {
            var dataLines = File.ReadAllLines(sourceFile);
            if (dataLines != null && dataLines.Length > 0)
            {
                Description = new DescriptionData(dataLines);
                // The rest of data file is a numbers collection 
                // each of which is used to define structural
                // or geometry parameters of superstructure
                // Some of parameters may be ignored
                var i = Description.Lines.Length + 1;

                // Removing description and empty lines
                dataLines = dataLines.TakeLast(dataLines.Length - i).Where(dl => !string.IsNullOrWhiteSpace(dataLines[i])).ToArray();
                
                // Some lines may contain more than one number: splitting those lines
                var numbers = new List<double>();
                foreach (var line in dataLines)
                {
                    var txtNumbers = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var number in txtNumbers)
                    {
                        numbers.Add(Double.Parse(number, System.Globalization.NumberStyles.Number));
                    }
                }

                // The first number is "results detalization level" - it may be ignored
                // The second number is Concrete strength
                Rc = numbers[1];
                TensileRebarKind = (RebarKind)numbers[2];
                CompressedRebarKind = (RebarKind)numbers[3];
                SlabRebarKind = (RebarKind)numbers[4];
                StirrupsRebarKind = (RebarKind)numbers[5];

                // Bearing points coordinates (X - is longitudinal axis)
                BearingPointLeftX = numbers[6];
                BearingPointRightX = numbers[7];

                // Bearing plates internal ends
                BearingPlateLeftX = numbers[8];
                BearingPlateRightX = numbers[9];

                FullLength = numbers[10];
                var mainBeamCount = (int)numbers[11];
                var beamsZ = Array.Empty<double>();
                var numIndex = 12;
                if (mainBeamCount > 0)
                {
                    beamsZ = numbers.GetRange(12, 12 + mainBeamCount).ToArray();
                    numIndex += mainBeamCount;
                }

                // Ballast bed
                BallastMaterial = (BallastKind)numbers[numIndex]; numIndex++;
                SleepersMaterial = (SleepersKind)numbers[numIndex]; numIndex++;
                TrackAxisZ1 = numbers[numIndex]; numIndex++;
                TrackAxisZ2 = numbers[numIndex]; numIndex++;
                if (mainBeamCount > 1)
                {
                    Diaphragm = (int)numbers[numIndex] == 1; 
                    numIndex++;
                }
                var ballastBedPoints = (int)numbers[numIndex]; 
                BallastBedShape = new BallastBed(numbers.GetRange(numIndex + 1, ballastBedPoints * 2));
                numIndex += ballastBedPoints * 2 + 1;

                // Main beams geometry
                MainBeams = new MainBeam[mainBeamCount];
                for (var b = 0; b < mainBeamCount; b++)
                {
                    MainBeams[b] = new MainBeam(beamsZ[b], b == 0 || b == mainBeamCount - 1,
                                        mainBeamCount = 1,
                                        numbers, ref numIndex);
                }
            } else
            {
                Description = new DescriptionData();
            }
        }

        public DescriptionData Description;
        /// <summary>
        /// Concrete strength, MPa
        /// </summary>
        public double Rc { get; set; } 
        /// <summary>
        /// Tensile reinforcement rebars kind
        /// </summary>
        public RebarKind TensileRebarKind { get; set; }
        /// <summary>
        /// Compressed reinforcement rebars kind
        /// </summary>
        public RebarKind CompressedRebarKind { get; set; }
        /// <summary>
        /// Slab reinforcement rebars kind
        /// </summary>
        public RebarKind SlabRebarKind { get; set; }
        /// <summary>
        /// Strirrups reinforcement rebars kind
        /// </summary>
        public RebarKind StirrupsRebarKind { get; set; }
        /// <summary>
        /// Left bearing point coordinate, cm
        /// </summary>
        public double BearingPointLeftX { get; set; }
        /// <summary>
        /// Right bearing point coordinate, cm
        /// </summary>
        public double BearingPointRightX { get; set; }
        /// <summary>
        /// Left bearing plate internal end coordinate, cm
        /// </summary>
        public double BearingPlateLeftX { get; set; }
        /// <summary>
        /// Right bearing plate internal end coordinate, cm
        /// </summary>
        public double BearingPlateRightX { get; set; }
        /// <summary>
        /// Superstructure length, cm
        /// </summary>
        public double FullLength { get; set; }
        /// <summary>
        /// Distance between left and right bearing points, cm
        /// </summary>
        public double Length 
        { 
            get
            {
                return BearingPointRightX - BearingPointLeftX;
            }
        }
        /// <summary>
        /// Type of ballast material
        /// </summary>
        public BallastKind BallastMaterial { get; set; }
        /// <summary>
        /// Type of sleepers material
        /// </summary>
        public SleepersKind SleepersMaterial { get; set; }
        /// <summary>
        /// Z coordinate of track axis at the beginning of superstructure, cm
        /// </summary>
        public double TrackAxisZ1 { get; set; }
        /// <summary>
        /// Z coordinate of track axis at the end of superstructure, cm
        /// </summary>
        public double TrackAxisZ2 { get; set; }
        /// <summary>
        ///  Main beams data
        /// </summary>
        public MainBeam[] MainBeams { get; set; } = Array.Empty<MainBeam>();
        /// <summary>
        /// Main beams are linked together via diaphragms 
        /// </summary>
        public bool Diaphragm { get; set; } = false;
        /// <summary>
        /// Ballast bed geometry
        /// </summary>
        public BallastBed BallastBedShape { get; set; }
    }
}
