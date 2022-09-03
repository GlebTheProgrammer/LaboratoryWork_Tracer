using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer.Core.Domain;

namespace Tracer.Serialization.Abstractions
{
    public interface ITraceResultSerializer
    {
        void Serialize(TraceResults traceResults,Stream to);
    }
}
