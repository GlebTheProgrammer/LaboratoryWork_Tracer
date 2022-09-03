using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Core.Domain
{
    public class TraceResults
    {
        public readonly List<string> methodsName = new List<string>();
        public readonly List<string> classesName = new List<string>();
        public readonly List<long> workTimes = new List<long>();
        public readonly List<string> inheritedMethodsName = new List<string>();

        public void AddResult(string methodName, string className, long workTime, string inheritedMethodName)
        {
            methodsName.Add(methodName);
            classesName.Add(className);
            workTimes.Add(workTime);
            inheritedMethodsName.Add(inheritedMethodName);
        }
    }
}
