using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using DTO;

namespace Conv
{
    public static class Converter
    {
        public static List<Bridge> Import(String path)
        {
            DataSetImportClass rawData;
            using (var fs = new FileStream(path, FileMode.Open))
            {
                rawData = (DataSetImportClass) new XmlSerializer(typeof(DataSetImportClass)).Deserialize(fs);
                fs.Close();
            }
            return rawData.Bridges.GroupBy(x => new
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
                            Code = z.DefectCode,
                            Name = z.DefectValue,
                            Value = String.IsNullOrEmpty(z.DefectCountValVal)
                                ? Double.NaN
                                : Double.Parse(z.DefectCountValVal),
                            Type = String.IsNullOrEmpty(z.DefectCountValVal)
                                ? DefectType.Qualitative
                                : DefectType.Quantitative,
                            DetectionDate = z.DefectCountValDate != String.Empty
                                ? DateTime.Parse(z.DefectCountValDate)
                                : DateTime.MinValue,
                            PlanRepairDate = z.PlanRepairDate != String.Empty
                                ? DateTime.Parse(z.PlanRepairDate)
                                : DateTime.MinValue,
                            FactRepairDate = z.FactRepairDate != String.Empty
                                ? DateTime.Parse(z.FactRepairDate)
                                : DateTime.MinValue,
                        }).ToList()
                    }).ToList()
                }
            ).ToList();
        }

        public static Boolean SaveXml(List<Bridge> data, Stream fs)
        {
            try
            {
                new XmlSerializer(typeof(List<Bridge>)).Serialize(fs, data);
            }
            catch (Exception)
            {
                return true;
            }
            return false;
        }

        public static Boolean SaveBin(List<Bridge> data, Stream fs)
        {
            try
            {
                new BinaryFormatter().Serialize(fs, data);
            }
            catch (Exception)
            {
                return true;
            }
            return false;
        }
    }
}