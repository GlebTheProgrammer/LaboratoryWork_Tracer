using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Serialization.Abstractions.Data
{
    public class Thread
    {
        public int Id { get; set; }
        public long Time { get; set; }
        public List<Method> Methods { get; set; }

        public Thread(int id, long time, List<Method> methods)
        {
            Id = id;
            Time = time;
            Methods = methods;
        }

        public Thread()
        {

        }
    }
}
