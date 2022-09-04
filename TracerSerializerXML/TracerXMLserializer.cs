using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer.Serialization.Abstractions;
using Tracer.Serialization.Abstractions.Data;
using System.Xml.Serialization;

namespace TracerSerializerXML
{
    public class TracerXMLserializer : ITraceResultSerializer
    {
        public async Task<string> Serialize(List<Thread> threadsResult, FileStream to)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Thread>));

                xmlSerializer.Serialize(to, threadsResult);
            }
            catch (Exception)
            {
                return "Data.xml was not saved.";
            }
            return "Data.xml saved successfully.";
        }
    }
}
