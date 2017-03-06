using System;

namespace Conv.Dto
{
    [Serializable]
    public class BridgeImportClass
    {
        public String MapNumber { get; set; }
        public Int16 EntryYear { get; set; }
        public Byte RoadCode { get; set; }
        public String RoadValue { get; set; }
        public Byte IssoCode { get; set; }
        public String IssoVal { get; set; }
        public Byte ObstecleCode { get; set; }
        public String ObstecleVal { get; set; }
        public Byte SpanNumber { get; set; }
        public Byte SpanMaterialCode { get; set; }
        public String SpanMaterialVal { get; set; }
        public String SpanCode { get; set; }
        public String SpanPlaced { get; set; }
        public Double SpanLength { get; set; }
        public Int32 IssoId { get; set; }
        public Int32 DefectCode { get; set; }
        public String DefectValue { get; set; }
        public String FoundDate { get; set; }
        public String PlanRepairDate { get; set; }
        public String FactRepairDate { get; set; }
        public String IssoElementCode { get; set; }
        public String IssoElementVal { get; set; }
        public Int32 DefectCountCode { get; set; }
        public String DefectCountVal { get; set; }
        public Double DefectCountValVal { get; set; }
        public String DefectCountValDate { get; set; }
    }
}