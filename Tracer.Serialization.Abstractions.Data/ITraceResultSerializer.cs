using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Serialization.Abstractions.Data
{
    public interface ITraceResultSerializer
    {
        void Serialize(TraceResults traceResults, FileStream to);
    }
}
