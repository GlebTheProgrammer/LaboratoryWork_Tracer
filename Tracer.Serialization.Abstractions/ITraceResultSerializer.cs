using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer.Core.Domain;
using Tracer.Serialization.Abstractions.Data;

namespace Tracer.Serialization.Abstractions
{
    public interface ITraceResultSerializer
    {
        Task<string> Serialize(List<Thread> threadsResult, FileStream to);
    }
}
