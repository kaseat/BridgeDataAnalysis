using System;

namespace DTO
{
    [Serializable]
    public class Defect
    {
        public DefectType Type { get; set; }
        public UInt64 ElementCode { get; set; }
        public String ElementValue { get; set; }
        public Int32 DefectTypeCode { get; set; }
        public String DefectTypeValue { get; set; }
        public Int32 WayNumber { get; set; }
        public String Position { get; set; }
        public DateTime DetectionDate { get; set; }
        public DateTime PlanRepairDate { get; set; }
        public DateTime FactRepairDate { get; set; }
        public Int32 DefectCharCode { get; set; }
        public String DefectCharCodeValue { get; set; }
        public Double DefectEntry { get; set; }
        public Int32 Id { get; set; }
    }
}