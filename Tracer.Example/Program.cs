using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tracer.Core;
using Tracer.Core.Domain;
using Tracer.Serialization.Abstractions.Data;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Tracer.Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Creating a single tracer 
            var tracer = new MainTracer();

            // Calling methods using the same tracer

            var e = new E(tracer);
            e.MethodE();

            var a = new A(tracer);
            a.MethodA();

            var c = new C(tracer);
            c.MethodC();

            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(e.MethodE));
            thread.Start();

            // Getting trace results as a List of all methods
            var results = a.getTracerResults();

            // Gettint results as ierarchy of all methods
            var threadsResult = GetThreadsForSerialization(results);

            // Showing tracing results to the user
            Console.WriteLine("Threads:\n");
            foreach (var threadResult in threadsResult)
            {
                Console.WriteLine($"\tid: {threadResult.Id}");
                Console.WriteLine($"\ttime: {threadResult.Time}");
                Console.WriteLine($"\tmethods:\n");
                PrintList(threadResult.Methods, "\t\t");
            }

            // JSON serialization
            string dllfile_JSON = @"C:\Users\Gleb\OneDrive\Рабочий стол\For Git\LaboratoryWork_Tracer\TracerSerializerJSON\bin\Debug\TracerSerializerJSON.dll";
            Assembly assembly_JSON = Assembly.LoadFile(dllfile_JSON);
            var myType_JSON = assembly_JSON.GetType("TracerSerializerJSON.TracerJSONserializer");
            MethodInfo myMethod_JSON = myType_JSON.GetMethod("Serialize");
            object instance_JSON = Activator.CreateInstance(myType_JSON);
            string savePath_JSON = @"C:\Users\Gleb\OneDrive\Рабочий стол\Data.json";
            Console.WriteLine("Saving data in JSON file process has been started...");
            var result_JSON = myMethod_JSON.Invoke(instance_JSON, new object[] { threadsResult, new FileStream(savePath_JSON, FileMode.OpenOrCreate) });
            Console.WriteLine(((Task<string>)result_JSON).Result);
            Console.WriteLine("JSON serialization has been completed...\n");

            // YAML serialization
            string dllfile_YAML = @"C:\Users\Gleb\OneDrive\Рабочий стол\For Git\LaboratoryWork_Tracer\TracerSerializerYAML\bin\Debug\TracerSerializerYAML.dll";
            Assembly assembly_YAML = Assembly.LoadFile(dllfile_YAML);
            var myType_YAML = assembly_YAML.GetType("TracerSerializerYAML.TracerYAMLserializer");
            MethodInfo myMethod_YAML = myType_YAML.GetMethod("Serialize");
            object instance_YAML = Activator.CreateInstance(myType_YAML);
            string savePath_YAML = @"C:\Users\Gleb\OneDrive\Рабочий стол\Data.yaml";
            Console.WriteLine("Saving data in YAML file process has been started...");
            var result_YAML = myMethod_YAML.Invoke(instance_YAML, new object[] { threadsResult, new FileStream(savePath_YAML, FileMode.OpenOrCreate) });
            Console.WriteLine(((Task<string>)result_YAML).Result);
            Console.WriteLine("YAML serialization has been completed...\n");

            // XML serialization
            string dllfile_XML = @"C:\Users\Gleb\OneDrive\Рабочий стол\For Git\LaboratoryWork_Tracer\TracerSerializerXML\bin\Debug\TracerSerializerXML.dll";
            Assembly assembly_XML = Assembly.LoadFile(dllfile_XML);
            var myType_XML = assembly_XML.GetType("TracerSerializerXML.TracerXMLserializer");
            MethodInfo myMethod_XML = myType_XML.GetMethod("Serialize");
            object instance_XML = Activator.CreateInstance(myType_XML);
            string savePath_XML = @"C:\Users\Gleb\OneDrive\Рабочий стол\Data.xml";
            Console.WriteLine("Saving data in XML file process has been started...");
            var result_XML = myMethod_XML.Invoke(instance_XML, new object[] { threadsResult, new FileStream(savePath_XML, FileMode.OpenOrCreate) });
            Console.WriteLine(((Task<string>)result_XML).Result);
            Console.WriteLine("XML serialization has been completed...\n");

            Console.WriteLine("All serialization processes have been completed!");

            Console.ReadLine();
        }

        private static void PrintList(List<Method> methods, string indent)
        {

            foreach (var method in methods)
            {
                Console.WriteLine($"{indent}name: {method.Name}");
                Console.WriteLine($"{indent}class: {method.ClassName}");
                Console.WriteLine($"{indent}time: {method.Time}");

                if(method.Methods.Count == 0)
                    Console.WriteLine($"{indent}methods: ...\n");
                else
                {
                    Console.WriteLine($"{indent}methods:\n");
                    PrintList(method.Methods, $"\t{indent}");
                }
            }
        }


        // Getting a list of all threads with their methods ierarchy
        private static List<Thread> GetThreadsForSerialization(TraceResults traceResults)
        {
            var result = new List<Thread>();


            int threadsCount = traceResults.threadsId.Max();

            for (int i = 0; i <= threadsCount; i++)
            {

                if (!traceResults.threadsId.Contains(i))
                    continue;
                else
                {
                    List<int> itemsId = new List<int>();


                    for (int j = 0; j < traceResults.methodsName.Count; j++)
                    {
                        if (traceResults.threadsId[j] == i)
                        {
                            itemsId.Add(j);
                        }
                    }

                    var traceResultsForCurrentThread = new TraceResults();
                    for (int k = 0; k < itemsId.Count; k++)
                    {
                        traceResultsForCurrentThread.threadsId.Add(traceResults.threadsId[k]);
                        traceResultsForCurrentThread.classesName.Add(traceResults.classesName[k]);
                        traceResultsForCurrentThread.inheritedMethodsName.Add(traceResults.inheritedMethodsName[k]);
                        traceResultsForCurrentThread.methodsName.Add(traceResults.methodsName[k]);
                        traceResultsForCurrentThread.workTimes.Add(traceResults.workTimes[k]);
                    }

                    for (int m = 0; m < itemsId.Count; m++)
                    {
                        itemsId[m] = m;
                    }

                    var methods = GetMethodsIerarchy(itemsId, traceResultsForCurrentThread, new List<Method>());

                    long threadTime = 0;
                    foreach (var method in methods)
                    {
                        threadTime += method.Time;
                    }

                    methods.Reverse();

                    result.Add(new Thread(i, threadTime, methods));
                }
            }

            return result;
        }

        // Getting methods ierarchy of a single thread
        private static List<Method> GetMethodsIerarchy(List<int> itemsId, TraceResults traceResults, List<Method> methods)
        {
            var result = new List<Method>();


            int id = itemsId.LastOrDefault();

            if(id == 0 || itemsId.Count == 1 ||
            (traceResults.methodsName.Count != id + 1 && traceResults.inheritedMethodsName[id] != traceResults.methodsName[id+1]))
            {
                result.Add(new Method(traceResults.methodsName[id],
                traceResults.classesName[id],
                traceResults.workTimes[id],
                new List<Method>()));

                return result;
            }

            if (traceResults.inheritedMethodsName[id - 1] == traceResults.inheritedMethodsName[id])
            {
                result.Add(new Method(traceResults.methodsName[id],
                traceResults.classesName[id],
                traceResults.workTimes[id],
                new List<Method>()));

                itemsId.RemoveAt(id);

                if (traceResults.inheritedMethodsName[itemsId.Last()] == traceResults.methodsName[id+result.Count])
                {
                    itemsId.RemoveAt(id-result.Count);

                    result.Add(new Method(traceResults.methodsName[id- result.Count],
                    traceResults.classesName[id- result.Count],
                    traceResults.workTimes[id- result.Count],
                    new List<Method>()));
                }

                return result;
            }

            itemsId.RemoveAt(id);

            result.Add(new Method(traceResults.methodsName[id],
            traceResults.classesName[id],
            traceResults.workTimes[id],
            GetMethodsIerarchy(itemsId, traceResults, result)));

            while(itemsId.Count != 0)
            {
                if (itemsId.Count == 1)
                {
                    int lastItemId = itemsId.Last();
                    itemsId.RemoveAt(lastItemId);

                    result.Add(new Method(traceResults.methodsName[lastItemId],
                    traceResults.classesName[lastItemId],
                    traceResults.workTimes[lastItemId],
                    new List<Method>()));

                    break;
                }

                int tempId = itemsId.Last();
                itemsId.RemoveAt(tempId);

                result.Add(new Method(traceResults.methodsName[tempId],
                traceResults.classesName[tempId],
                traceResults.workTimes[tempId],
                GetMethodsIerarchy(itemsId, traceResults, result)));
            }

            return result;
        }
    }

    // Classes to use when calling methods
    public class A
    {
        private B b;
        private ITracer tracer;
        public A(ITracer tracer)
        {
            this.tracer = tracer;
            b = new B(this.tracer);
        }

        public void MethodA()
        {
            tracer.StartTrace();
            b.MethodB();
            b.MethodB();
            System.Threading.Thread.Sleep(100);
            tracer.StopTrace();
        }

        public TraceResults getTracerResults()
        {
            return tracer.GetTraceResult();
        }
    }
    public class B
    {
        private ITracer tracer;
        public B(ITracer tracer)
        {
            this.tracer = tracer;
        }

        public void MethodB()
        {
            tracer.StartTrace();
            System.Threading.Thread.Sleep(1);
            tracer.StopTrace();
        }

        public TraceResults getTracerResults()
        {
            return tracer.GetTraceResult();
        }
    }
    public class C
    {
        private D d;
        private ITracer tracer;
        public C(ITracer tracer)
        {
            this.tracer = tracer;
            d = new D(this.tracer);
        }

        public void MethodC()
        {
            tracer.StartTrace();
            d.MethodD();
            d.MethodD();
            System.Threading.Thread.Sleep(100);
            tracer.StopTrace();
        }

        public TraceResults getTracerResults()
        {
            return tracer.GetTraceResult();
        }
    }
    public class D
    {
        private ITracer tracer;
        public D(ITracer tracer)
        {
            this.tracer = tracer;
        }

        public void MethodD()
        {
            tracer.StartTrace();
            System.Threading.Thread.Sleep(1);
            tracer.StopTrace();
        }

        public TraceResults getTracerResults()
        {
            return tracer.GetTraceResult();
        }
    }
    public class E
    {
        private B b;
        private ITracer tracer;
        public E(ITracer tracer)
        {
            this.tracer = tracer;
            b = new B(this.tracer);
        }

        public void MethodE()
        {
            tracer.StartTrace();
            System.Threading.Thread.Sleep(1);
            tracer.StopTrace();
        }

        public TraceResults getTracerResults()
        {
            return tracer.GetTraceResult();
        }
    }

}
