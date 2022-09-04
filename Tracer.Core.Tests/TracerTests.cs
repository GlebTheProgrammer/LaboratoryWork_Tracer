using Tracer.Core.Domain;
using Xunit;

namespace Tracer.Core.Tests
{
    public class TracerTests
    {
        [Fact]
        public void GetTraceResults_WithOneMethod_ReturnARightNumberOfTotalMethodsAndOtherFields()
        {
            // Arrange
            var tracer = new MainTracer();
            var a = new A(tracer);

            // Act
            a.MethodA();

            // Assert
            Assert.Equal(3, tracer.GetTraceResult().methodsName.Count);
            Assert.Equal(3, tracer.GetTraceResult().threadsId.Count);
            Assert.Equal(3, tracer.GetTraceResult().classesName.Count);
            Assert.Equal(3, tracer.GetTraceResult().inheritedMethodsName.Count);
            Assert.Equal(3, tracer.GetTraceResult().workTimes.Count);
        }

        [Fact]
        public void GetTraceResults_WithTwoOrMoreMethods_ReturnARightNumberOfTotalMethodsAndOtherFields()
        {
            // Arrange
            var tracer = new MainTracer();
            var a = new A(tracer);
            var c = new C(tracer);

            // Act
            a.MethodA();
            c.MethodC();

            // Assert
            Assert.Equal(6, tracer.GetTraceResult().methodsName.Count);
            Assert.Equal(6, tracer.GetTraceResult().threadsId.Count);
            Assert.Equal(6, tracer.GetTraceResult().classesName.Count);
            Assert.Equal(6, tracer.GetTraceResult().inheritedMethodsName.Count);
            Assert.Equal(6, tracer.GetTraceResult().workTimes.Count);
        }

        [Fact]
        public void GetTraceResults_WithOneMethodInOneThread_ReturnAllMethodsWithTheSameThreadId()
        {
            // Arrange
            var tracer = new MainTracer();
            var a = new A(tracer);

            // Act
            a.MethodA();

            // Assert
            for (int i = 1; i < tracer.GetTraceResult().threadsId.Count; i++)
            {
                Assert.Equal(tracer.GetTraceResult().threadsId[i - 1], tracer.GetTraceResult().threadsId[i]);
            }
        }

        [Fact]
        public void GetTraceResults_WithOneMethodWithAnotherMethodInside_ReturnRightNumberOfData()
        {
            // Arrange
            var tracer = new MainTracer();
            var a = new A(tracer);
            var b = new B(tracer);

            // Act
            a.MethodA();
            b.MethodB();

            // Assert
            Assert.Equal(4, tracer.GetTraceResult().methodsName.Count);
            Assert.Equal(4, tracer.GetTraceResult().threadsId.Count);
            Assert.Equal(4, tracer.GetTraceResult().classesName.Count);
            Assert.Equal(4, tracer.GetTraceResult().inheritedMethodsName.Count);
            Assert.Equal(4, tracer.GetTraceResult().workTimes.Count);
        }
    }
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
}