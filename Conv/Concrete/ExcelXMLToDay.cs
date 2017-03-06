using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Conv.Abstract;
using Conv.Dto;
using DTO;

namespace Conv.Concrete
{
    public class ExcelXMLToDay : IImporter<IEnumerable<DayClass>>
    {
        private readonly Stream ioStream;

        public ExcelXMLToDay(Stream ioStream)
        {
            this.ioStream = ioStream;
        }

        public IEnumerable<DayClass> Import() =>
            ((TemperatureImporter) new XmlSerializer(typeof(TemperatureImporter)).Deserialize(ioStream)).Days;
    }
}