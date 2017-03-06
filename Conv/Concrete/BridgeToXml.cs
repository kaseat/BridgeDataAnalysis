using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Conv.Abstract;
using DTO;

namespace Conv.Concrete
{
    public class BridgeToXml : IExporter<IEnumerable<Bridge>>
    {
        private readonly Stream stream;

        public BridgeToXml(Stream stream)
        {
            this.stream = stream;
        }

        public void Export(IEnumerable<Bridge> data)
            => new XmlSerializer(typeof(IEnumerable<Bridge>)).Serialize(stream, data);
    }
}