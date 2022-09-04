using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer.Serialization.Abstractions.Data;

namespace Tracer.Serialization.JSON
{
    public class JsonSerializer : ITraceResultSerializer
    {
        public async void Serialize(List<Thread> threadSResult, FileStream to)
        {
            await System.Text.Json.JsonSerializer.SerializeAsync<List<Thread>>(to, threadSResult);
            Console.WriteLine("JSON serialization completed successfully!");
        }

    }
}
