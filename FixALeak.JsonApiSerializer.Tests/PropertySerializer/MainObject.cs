using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.JsonApiSerializer.Tests.PropertySerializer
{
    class RelatedObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    class RelatedObject2
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    class RelatedObject3
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    class MainObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public RelatedObject RelOb { get; set; }
        public ICollection<RelatedObject2> RealtedObject2List { get; set; }
        public IEnumerable<RelatedObject3> RealtedObject3List { get; set; }
    }
}
