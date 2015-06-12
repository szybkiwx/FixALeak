using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public class PropertySerializerAggregate
    {
        [Dependency]
        public KeyPropertySerializer KeyPropertySerializer { get; set; }

        [Dependency]
        public ValuePropertySerializer ValuePropertySerializer { get; set; }

        [Dependency]
        public CollectionPropertySerializer CollectionPropertySerializer { get; set; }

        [Dependency]
        public NullSerializer NullSerializer{get;set;}
    }
}
