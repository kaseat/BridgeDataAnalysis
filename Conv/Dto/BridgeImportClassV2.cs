using System;

namespace Conv.Dto
{
    [Serializable]
    public class BridgeImportClassV2
    {
        public Int32 IssoID { get; set; }
        public String CardNum { get; set; }
        public Int32 EntryYear { get; set; }
        public Int32 RoadCode { get; set; }
        public String RoadValue { get; set; }
        public Int32 DistCode { get; set; }
        public String DistValue { get; set; }
        public Int32 Km { get; set; }
        public Int32 PK { get; set; }
        public String RoadWayCode { get; set; }
        public String RoadWayValue { get; set; }
        public Int32 DirectionCode { get; set; }
        public String DirectionValue { get; set; }
        public String ObstecleName { get; set; }
        public Int32 IssoCode { get; set; }
        public String IssoVal { get; set; }
        public Int32 ObstecleCode { get; set; }
        public String ObstecleVal { get; set; }
        public Int32 SpanNumber { get; set; }
        public Int32 SpanMaterialCode { get; set; }
        public String SpanMaterialVal { get; set; }
        public String SpanCode { get; set; }
        public String SpanManufactured { get; set; }
        public String SpanPlaced { get; set; }
        public Double SpanLength { get; set; }
        public Int32 MPolotnoCode { get; set; }
        public String MpolotnoVal { get; set; }
        public Int32 DefectTypeCode { get; set; }
        public String DefectTypeValue { get; set; }
        public String WayNumber { get; set; }
        public String DefectPlace { get; set; }
        public String FoundDate { get; set; }
        public String PlanRepairDate { get; set; }
        public String FactRepairDate { get; set; }
        public UInt64 IssoElementCode { get; set; }
        public String IssoElementVal { get; set; }
        public Int32 DefectCode { get; set; }
        public String DefectVal { get; set; }
        public Double DefectEntry { get; set; }
        public String DefectDate { get; set; }
        public Byte DefectId { get; set; }
    }
}