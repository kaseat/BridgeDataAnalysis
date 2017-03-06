using System;
using System.Collections.Generic;
using System.Linq;

namespace DTO
{
    [Serializable]
    public class Bridge
    {
        public Int32 Id { get; set; }
        public String CrdNumber { get; set; }
        public Int32 EntryYear { get; set; }
        public Int32 RoadCode { get; set; }
        public String RoadValue { get; set; }
        public Int32 DistCode { get; set; }
        public String DistValue { get; set; }
        public Int32 Km { get; set; }
        public Int32 Pk { get; set; }
        public Int32 WayCode { get; set; }
        public String WayValue { get; set; }
        public Int32 DirectionCode { get; set; }
        public String DirectionValue { get; set; }
        public String ObstecleName { get; set; }
        public Int32 IssoCode { get; set; }
        public String IssoVal { get; set; }
        public Int32 ObstecleCode { get; set; }
        public String ObstecleVal { get; set; }
        public List<Superstructure> Superstructures { get; set; }

        public override String ToString()
            => $"Defect count: {Superstructures.Sum(superstructure => superstructure.Defects.Count)}";
    }
}