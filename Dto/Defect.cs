using System;

namespace DTO
{
    [Serializable]
    public class Defect
    {
        public DefectType Type { get; set; }
        public Int32 Code { get; set; }
        public String Name { get; set; }
        public Double Value { get; set; }
        public DateTime DetectionDate { get; set; }
        public DateTime PlanRepairDate { get; set; }
        public DateTime FactRepairDate { get; set; }
    }
}