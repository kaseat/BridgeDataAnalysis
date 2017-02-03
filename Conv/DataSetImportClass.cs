using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Conv
{
    [Serializable]
    [XmlRoot("DataSet")]
    public class DataSetImportClass
    {
        [XmlArrayItem("Bridge")]
        public List<BridgeImportClass> Bridges { get; set; }
    }
}