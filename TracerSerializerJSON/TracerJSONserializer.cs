using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Tracer.Serialization.Abstractions;
using Tracer.Serialization.Abstractions.Data;

namespace TracerSerializerJSON
{
    public class TracerJSONserializer : ITraceResultSerializer
    {
        public async Task<string> Serialize(List<Thread> threadSResult, FileStream to)
        {
            try
            {
                await JsonSerializer.SerializeAsync<List<Thread>>(to, threadSResult);
            }
            catch (Exception)
            {
                return "Data.json was not saved.";
            }

            return "Data.json saved successfully.";
        }
    }
}
