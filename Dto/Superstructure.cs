using System;
using System.Collections.Generic;

namespace DTO
{
    [Serializable]
    public class Superstructure
    {
        public Byte Number { get; set; }
        public Byte MaterialCode { get; set; }
        public String Material { get; set; }
        public String SuperstructureCode { get; set; }
        public DateTime PlaceDate { get; set; }
        public Double Length { get; set; }
        public List<Defect> Defects { get; set; }

        public override String ToString()
            => $"DefCnt= {Defects.Count}";
    }
}