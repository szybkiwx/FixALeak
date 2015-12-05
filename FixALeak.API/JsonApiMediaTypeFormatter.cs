using FixALeak.JsonApiSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;

namespace FixALeak.API
{
    public class JsonApiMediaTypeFormatter : BufferedMediaTypeFormatter
    {

        private bool isEntity(Type type)
        {
            return type.GetInterface("IEntity") != null;
        }

        private bool isEntityCollection(Type type)
        {
            var generics = type.GetGenericArguments();
            return type.GetInterface("IEnumerable") != null 
                && generics.Length > 0 
                && generics[0].GetInterface("IEntity") != null;
        }       
        
        private bool IsPatch(Type type)
        {
            return type == typeof(JsonApiPatch) || type.BaseType == typeof(JsonApiPatch);
        }

        private bool isRelationshipObject(Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(JsonApiRelationship<>))
                || type == typeof(JsonApiRelationship);
        }

        public override bool CanReadType(Type type)
        {
            if(isEntity(type))
            {
                return true;
            }

            if (isEntityCollection(type))
            {
                return true;
            }

            if(IsPatch(type))
            {
                return true;
            }

            return false;
        }

        public override bool CanWriteType(Type type)
        {

            if (isEntity(type))
            {
                return true;
            }

            if (isEntityCollection(type))
            {
                return true;
            }

            if(isRelationshipObject(type))
            {
                return true;
            }

            return false;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            using (var writer = new StreamWriter(writeStream))
            {
                if (isRelationshipObject(type))
                {
                    writer.Write(SerializerBuilder.Create().SerializeRelationship((JsonApiRelationship)value).ToString());
                }
                else if (isEntityCollection(type))
                {
                    writer.Write(SerializerBuilder.Create().Serialize(value as IEnumerable).ToString());
                }
                else
                {
                    writer.Write(SerializerBuilder.Create().Serialize(value).ToString());
                }
            }
        }

        public override object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {

            using (var reader = new StreamReader(readStream))
            {

                if (IsPatch(type))
                {
                    return SerializerBuilder.Create().DeserializePatch(reader.ReadToEnd(), type.GetGenericArguments()[0]);
                }

                return SerializerBuilder.Create().Deserialize(reader.ReadToEnd(), type);
                
            }
        }
    }
}