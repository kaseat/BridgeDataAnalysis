using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Conv.Dto
{
    [Serializable]
    [XmlRoot("DataSet")]
    public class DataSetImportClass
    {
        [XmlArrayItem("Bridge")]
        public List<BridgeImportClass> Bridges { get; set; }
    }
    [Serializable]
    [XmlRoot("DataSet")]
    public class DataSetV2ImportClass
    {
        [XmlArrayItem("Bridge")]
        public List<BridgeImportClassV2> Bridges { get; set; }
    }

    [Serializable]
    [XmlRoot("DataSet")]
    public class TemperatureImporter
    {
        [XmlArrayItem("Day")]
        public List<DayClass> Days { get; set; }
    }
}