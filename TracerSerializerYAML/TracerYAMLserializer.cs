using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Tracer.Serialization.Abstractions;
using Tracer.Serialization.Abstractions.Data;
using System.IO;

namespace TracerSerializerYAML
{
    public class TracerYAMLserializer : ITraceResultSerializer
    {
        public async Task<string> Serialize(List<Thread> threadsResult, FileStream to)
        {
            try
            {
                var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                var yaml = serializer.Serialize(threadsResult);

                byte[] info = new UTF8Encoding(true).GetBytes(yaml);
                await to.WriteAsync(info, 0, info.Length);
            }
            catch (Exception)
            {
                return "Data.yaml was not saved.";
            }
            return "Data.yaml saved successfully.";
        }
    }
}
