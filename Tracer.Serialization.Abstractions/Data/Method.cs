using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Serialization.Abstractions.Data
{
    public class Method
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public long Time { get; set; }
        public List<Method> Methods { get; set; }

        public Method(string name, string className, long time, List<Method> methods)
        {
            Name = name;
            ClassName = className;
            Time = time;
            Methods = methods;
        }
        public Method()
        {

        }
    }
}
