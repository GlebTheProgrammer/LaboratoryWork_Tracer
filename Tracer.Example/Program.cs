using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracer.Core;
using Tracer.Core.Domain;

namespace Tracer.Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tracer = new MainTracer();
            var foo = new Foo(tracer);

            foo.MyMethod();

            var results = foo.getTracerResults();

            for (int i = 0; i < results.methodsName.Count; i++)
            {
                Console.WriteLine($"Class Name: {results.classesName[i]}\nMethod Name: {results.methodsName[i]}\nTime: {results.workTimes[i]}\ninher: {results.inheritedMethodsName[i]}\n");
            }
            Console.ReadLine();
        }
    }

    public class Foo
    {
        private Bar bar;
        private ITracer tracer;
        public Foo(ITracer tracer)
        {
            this.tracer = tracer;
            bar = new Bar(this.tracer);
        }

        public void MyMethod()
        {
            tracer.StartTrace();
            bar.InnerMethod();
            bar.InnerMethod();
            Thread.Sleep(100);
            tracer.StopTrace();
        }

        public TraceResults getTracerResults()
        {
            return tracer.GetTraceResult();
        }
    }


    public class Bar
    {
        private ITracer tracer;
        public Bar(ITracer tracer)
        {
            this.tracer = tracer;
        }

        public void InnerMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(1);
            tracer.StopTrace();
        }

        public TraceResults getTracerResults()
        {
            return tracer.GetTraceResult();
        }
    }
}
