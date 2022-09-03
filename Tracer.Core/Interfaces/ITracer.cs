using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer.Core.Domain;

namespace Tracer.Core
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        TraceResults GetTraceResult();
    }
}
