using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer.Serialization.Abstractions.Data;

namespace TracerSerializerData
{
    public interface ITraceResultSerializer
    {
        string Serialize(List<Thread> threadsResult, FileStream to);
    }
}
