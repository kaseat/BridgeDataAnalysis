using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Conv.Abstract;
using Conv.Dto;
using DTO;

namespace Conv.Concrete
{
    public class ExcelXmlV2ToBridge : IImporter<IEnumerable<Bridge>>
    {
        private readonly Stream ioStream;

        public ExcelXmlV2ToBridge(Stream ioStream)
        {
            this.ioStream = ioStream;
        }

        public IEnumerable<Bridge> Import() =>
            ((DataSetV2ImportClass) new XmlSerializer(typeof(DataSetV2ImportClass)).Deserialize(ioStream)).Bridges
                .GroupBy(x => new
                {
                    x.IssoID,
                    x.CardNum,
                    x.EntryYear,
                    x.RoadCode,
                    x.RoadValue,
                    x.DistCode,
                    x.DistValue,
                    x.Km,
                    x.PK,
                    x.RoadWayCode,
                    x.RoadWayValue,
                    x.DirectionCode,
                    x.DirectionValue,
                    x.ObstecleName,
                    x.IssoCode,
                    x.IssoVal,
                    x.ObstecleCode,
                    x.ObstecleVal
                }).Select(x => new Bridge
                {
                    Id = x.Key.IssoID,
                    CrdNumber = x.Key.CardNum,
                    EntryYear = x.Key.EntryYear,
                    RoadCode = x.Key.RoadCode,
                    RoadValue = x.Key.RoadValue,
                    DistCode = x.Key.DistCode,
                    DistValue = x.Key.DistValue,
                    Km = x.Key.Km,
                    Pk = x.Key.PK,
                    WayCode = String.IsNullOrEmpty(x.Key.RoadWayCode) ? 0 : Int32.Parse(x.Key.RoadWayCode),
                    WayValue = x.Key.RoadWayValue,
                    DirectionCode = x.Key.DirectionCode,
                    DirectionValue = x.Key.DirectionValue,
                    ObstecleName = x.Key.ObstecleName,
                    IssoCode = x.Key.IssoCode,
                    IssoVal = x.Key.IssoVal,
                    ObstecleCode = x.Key.ObstecleCode,
                    ObstecleVal = x.Key.ObstecleVal,
                    Superstructures = x.GroupBy(y => new
                    {
                        y.SpanNumber,
                        y.SpanMaterialCode,
                        y.SpanMaterialVal,
                        y.SpanCode,
                        y.SpanLength,
                        PlaceDate = new DateTime(Int32.Parse(y.SpanPlaced), 4, 1),
                        ManufDate = new DateTime(Int32.Parse(y.SpanManufactured), 4, 1),
                        y.MPolotnoCode,
                        y.MpolotnoVal,
                    }).Select(y => new Superstructure
                    {
                        Number = y.Key.SpanNumber,
                        MaterialCode = y.Key.SpanMaterialCode,
                        Material = y.Key.SpanMaterialVal,
                        SuperstructureCode = y.Key.SpanCode,
                        PlaceDate = y.Key.PlaceDate,
                        ManufacturedDate = y.Key.ManufDate,
                        Length = y.Key.SpanLength,
                        MostPolCode = y.Key.MPolotnoCode,
                        MostPolValue = y.Key.MpolotnoVal,
                        Defects = y.Select(z => new Defect
                            {
                                DefectTypeCode = z.DefectTypeCode,
                                DefectTypeValue = z.DefectTypeValue,
                                DefectEntry = Math.Abs(z.DefectEntry) < 1e-5
                                    ? Double.NaN
                                    : z.DefectEntry,
                                Type = Math.Abs(z.DefectEntry) < 1e-5
                                    ? DefectType.Qualitative
                                    : DefectType.Quantitative,
                                DetectionDate = String.IsNullOrEmpty(z.FoundDate)
                                    ? DateTime.MinValue
                                    : DateTime.Parse(z.FoundDate),
                                PlanRepairDate = String.IsNullOrEmpty(z.PlanRepairDate)
                                    ? DateTime.MinValue
                                    : DateTime.Parse(z.PlanRepairDate),
                                FactRepairDate = String.IsNullOrEmpty(z.FactRepairDate)
                                    ? DateTime.MinValue
                                    : DateTime.Parse(z.FactRepairDate),
                                DefectCharCode = z.DefectCode,
                                DefectCharCodeValue = z.DefectVal,
                                WayNumber = String.IsNullOrEmpty(z.WayNumber)
                                    ? 0
                                    : Int32.Parse(z.WayNumber),
                                Position = z.DefectPlace,
                                ElementCode = z.IssoElementCode,
                                ElementValue = z.IssoElementVal,
                            })
                            .Where(a => a.FactRepairDate != DateTime.MinValue && a.FactRepairDate>=y.Key.PlaceDate && a.DefectTypeCode == 1240
                            && a.ElementCode < 101050000000000)
                            .OrderBy(a => a.DetectionDate).ToList()
                    }).OrderBy(z => z.Number).ToList()
                }).Where(x => x.Superstructures.Sum(superstructure => superstructure.Defects.Count) > 0 
               // && x.RoadCode!=96 
               // && x.RoadCode != 51
                ).Select(x => x).OrderBy(x => x.Id);
    }
}