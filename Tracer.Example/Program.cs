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

namespace Tracer.Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tracer = new MainTracer();

            var foo = new Foo(tracer);
            foo.MyMethod();

            var car = new Car(tracer);
            car.MyMethod();

            var results = foo.getTracerResults();

            var threadsResult = GetThreadsForSerialization(results);

            Console.WriteLine("Threads:\n");
            foreach (var threadResult in threadsResult)
            {
                Console.WriteLine($"\tid: {threadResult.Id}");
                Console.WriteLine($"\ttime: {threadResult.Time}");
                Console.WriteLine($"\tmethods:\n");
                PrintList(threadResult.Methods, "\t\t");
            }

            string dllfile2 = @"C:\Users\Gleb\OneDrive\Рабочий стол\For Git\LaboratoryWork_Tracer\TracerSerializerJSON\bin\Debug\TracerSerializerJSON.dll";
            // Use the file name to load the assembly into the current
            // application domain.
            Assembly a2 = Assembly.LoadFile(dllfile2);
            // Get the type to use.
            var myType2 = a2.GetType("TracerSerializerJSON.TracerJSONserializer");
            //var myType2 = a2.GetType("TracerSerializerJSON.ITraceResultSerializer");
            // Get the method to call.

            MethodInfo myMethod2 = myType2.GetMethod("Serialize");
            // Create an instance.
            object obj2 = Activator.CreateInstance(myType2);
            // Execute the method.
            //myMethod.Invoke(obj, new object[] { threadsResult, new FileStream("Data.json", FileMode.OpenOrCreate) });

            string savePath = @"C:\Users\Gleb\OneDrive\Рабочий стол\Data.json";

            Console.WriteLine("Yoooo");
            var res2 = myMethod2.Invoke(obj2, new object[] { threadsResult, new FileStream(savePath, FileMode.OpenOrCreate) });
            Console.WriteLine(((Task<string>)res2).Result);
            Console.WriteLine("JSON serialization completed successfully!");
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

                    var methods = GetMethodsIerarchy(itemsId, traceResults, new List<Method>());

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

        private static List<Method> GetMethodsIerarchy(List<int> itemsId, TraceResults traceResults, List<Method> methods)
        {
            var result = new List<Method>();


            int id = itemsId.LastOrDefault();

            if(id == 0 || 
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

            if (itemsId.Count != 0)
            {
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
            System.Threading.Thread.Sleep(100);
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
            System.Threading.Thread.Sleep(1);
            tracer.StopTrace();
        }

        public TraceResults getTracerResults()
        {
            return tracer.GetTraceResult();
        }
    }





    public class Car
    {
        private Bus bar;
        private ITracer tracer;
        public Car(ITracer tracer)
        {
            this.tracer = tracer;
            bar = new Bus(this.tracer);
        }

        public void MyMethod()
        {
            tracer.StartTrace();
            bar.InnerMethod();
            bar.InnerMethod();
            System.Threading.Thread.Sleep(100);
            tracer.StopTrace();
        }

        public TraceResults getTracerResults()
        {
            return tracer.GetTraceResult();
        }
    }
    public class Bus
    {
        private ITracer tracer;
        public Bus(ITracer tracer)
        {
            this.tracer = tracer;
        }

        public void InnerMethod()
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
