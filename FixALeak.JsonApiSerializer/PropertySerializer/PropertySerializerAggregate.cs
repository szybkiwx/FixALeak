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
