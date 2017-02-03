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
        public Int16 EntryYear { get; set; }
        public Byte RoadCode { get; set; }
        public String RoadValue { get; set; }
        public Byte IssoCode { get; set; }
        public String IssoVal { get; set; }
        public Byte ObstecleCode { get; set; }
        public String ObstecleVal { get; set; }
        public List<Superstructure> Superstructures { get; set; }

        public override String ToString()
            => $"Defect count: {Superstructures.Sum(superstructure => superstructure.Defects.Count)}";
    }
}