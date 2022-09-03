using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Serialization.Abstractions.Data
{
    public class TraceResults
    {
        // Main Data
        public readonly List<string> methodsName = new List<string>();
        public readonly List<string> classesName = new List<string>();
        public readonly List<long> workTimes = new List<long>();

        // Additional Data
        public readonly List<string> inheritedMethodsName = new List<string>();
        public readonly List<int> threadsId = new List<int>();

        public void AddResult(string methodName, string className, long workTime, string inheritedMethodName, int threadId)
        {
            methodsName.Add(methodName);
            classesName.Add(className);
            workTimes.Add(workTime);
            inheritedMethodsName.Add(inheritedMethodName);
            threadsId.Add(threadId);
        }
    }
}
