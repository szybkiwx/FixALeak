using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.JsonApiSerializer.Tests
{
    public class DeserializerTestObject
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Src { get; set; }
        public float SomeFloat { get; set; }
        public decimal SomeDecimal { get; set; }
        public double SomeDouble { get; set; }
        public DeserializerTestRelatedObject Des { get; set; }
        public ICollection<DeserializerTestRelatedObject> DesCollection { get; set; }
    }

    public class DeserializerTestRelatedObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
