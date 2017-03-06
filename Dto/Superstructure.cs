using System;
using System.Collections.Generic;

namespace DTO
{
    [Serializable]
    public class Superstructure
    {
        public Int32 Number { get; set; }
        public Int32 MaterialCode { get; set; }
        public String Material { get; set; }
        public String SuperstructureCode { get; set; }
        public DateTime ManufacturedDate { get; set; }
        public DateTime PlaceDate { get; set; }
        public Double Length { get; set; }
        public Int32 MostPolCode { get; set; }
        public String MostPolValue { get; set; }
        public List<Defect> Defects { get; set; }

        public override String ToString()
            => $"DefCnt= {Defects.Count}";
    }
}