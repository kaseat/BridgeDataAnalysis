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
    public class ExcelXmlToBridge : IImporter<IEnumerable<Bridge>>
    {
        private readonly Stream ioStream;

        public ExcelXmlToBridge(Stream ioStream)
        {
            this.ioStream = ioStream;
        }

        public IEnumerable<Bridge> Import() =>
        ((DataSetImportClass) new XmlSerializer(typeof(DataSetImportClass))
            .Deserialize(ioStream)).Bridges.GroupBy(x => new
        {
            x.IssoId,
            x.MapNumber,
            x.EntryYear,
            x.RoadCode,
            x.RoadValue,
            x.IssoCode,
            x.IssoVal,
            x.ObstecleCode,
            x.ObstecleVal
        }).Select(x => new Bridge
        {
            Id = x.Key.IssoId,
            CrdNumber = x.Key.MapNumber,
            EntryYear = x.Key.EntryYear,
            RoadCode = x.Key.RoadCode,
            RoadValue = x.Key.RoadValue,
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
            }).Select(y => new Superstructure
            {
                Number = y.Key.SpanNumber,
                MaterialCode = y.Key.SpanMaterialCode,
                Material = y.Key.SpanMaterialVal,
                SuperstructureCode = y.Key.SpanCode,
                PlaceDate = y.Key.PlaceDate,
                Length = y.Key.SpanLength,
                Defects = y.Select(z => new Defect
                {
                    DefectTypeCode = z.DefectCountCode,
                    DefectTypeValue = z.DefectCountVal,
                    DefectEntry = Math.Abs(z.DefectCountValVal) < 1e-5
                        ? Double.NaN
                        : z.DefectCountValVal,
                    Type = Math.Abs(z.DefectCountValVal) < 1e-5
                        ? DefectType.Qualitative
                        : DefectType.Quantitative,
                    DetectionDate = String.IsNullOrEmpty(z.DefectCountValDate)
                        ? DateTime.MinValue
                        : DateTime.Parse(z.DefectCountValDate),
                    PlanRepairDate = String.IsNullOrEmpty(z.PlanRepairDate)
                        ? DateTime.MinValue
                        : DateTime.Parse(z.PlanRepairDate),
                    FactRepairDate = String.IsNullOrEmpty(z.FactRepairDate)
                        ? DateTime.MinValue
                        : DateTime.Parse(z.FactRepairDate),
                }).Where(a=>a.DefectTypeCode==1240 &&a.FactRepairDate!=DateTime.MinValue)
                .OrderBy(a=>a.DetectionDate).ToList()
            }).Where(z=>z.Defects.Count>5 &&z.PlaceDate<z.Defects.Min(t=>t.DetectionDate))
            .OrderBy(z=>z.Number).ToList()
        }).Where(x => x.Superstructures
                    .Sum(superstructure => superstructure.Defects.Count) > 5)
                    .Select(x => x).OrderBy(x=>x.Id);
    }
}