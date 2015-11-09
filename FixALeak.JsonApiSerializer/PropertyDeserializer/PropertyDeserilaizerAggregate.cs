using Microsoft.Practices.Unity;

namespace FixALeak.JsonApiSerializer.PropertyDeserializer
{
    public class PropertyDeserilaizerAggregate
    {
        /*[Dependency]
        public KeyPropertyDeserializer KeyPropertySerializer { get; set; }*/

        [Dependency]
        public ValuePropertyDeserializer ValuePropertySerializer { get; set; }

        [Dependency]
        public CollectionPropertyDeserializer CollectionPropertySerializer { get; set; }

        [Dependency]
        public RefPropertyDeserializer RefPropertyDeserializer { get; set; }
    }
}