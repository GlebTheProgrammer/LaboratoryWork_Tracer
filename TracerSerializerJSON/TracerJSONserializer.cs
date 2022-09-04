using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Tracer.Serialization.Abstractions.Data;

namespace TracerSerializerJSON
{
    public class TracerJSONserializer //: ITraceResultSerializer
    {
        public string Serialize(List<Thread> threadSResult, FileStream to)
        {
            JsonSerializer.Serialize<List<Thread>>(to, threadSResult);

            return "Data saved successfully!";
        }
    }
}
