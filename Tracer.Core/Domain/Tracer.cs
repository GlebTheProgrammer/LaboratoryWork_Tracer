using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracer.Core.Domain;

namespace Tracer.Core
{
    public class MainTracer : ITracer
    {
        Stopwatch stopWatch = new Stopwatch();

        TraceResults traceResults = new TraceResults();

        public TraceResults GetTraceResult()
        {
            return traceResults;
        }

        public void StartTrace()
        {
            stopWatch.Start();
        }

        public void StopTrace()
        {
            stopWatch.Stop();

            string methodName = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name;
            string className = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().DeclaringType.Name;
            string inheritedMethodName = (new System.Diagnostics.StackTrace()).GetFrame(2).GetMethod().Name;
            int threadId = Thread.CurrentThread.ManagedThreadId;

            long additionalTime = 0;

            for (int i = 0; i < traceResults.methodsName.Count; i++)
            {
                if (traceResults.inheritedMethodsName[i] == methodName)
                    additionalTime += traceResults.workTimes[i];
                else
                    continue;
            }

            traceResults.AddResult(methodName, className, stopWatch.ElapsedMilliseconds + additionalTime, inheritedMethodName, threadId);

            stopWatch.Reset();
            StartTrace();
        }
    }
}
