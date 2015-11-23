using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.API.IntegrationTests
{
    [DataContract]
    public class Data
    {
        [DataMember(Name ="type")]
        public string Type { get; set; }

        [DataMember(Name = "id", EmitDefaultValue = false)]
        public int? Id { get; set; }

        [DataMember(Name = "attributes")]
        public Dictionary<string, object> Attributes { get; set; }

        public Data()
        {
            Attributes = new Dictionary<string, object>();
        }
    }

    [DataContract]
    public class GenericJsonApiObject
    {
        [DataMember(Name = "data")]
        public Data Data { get; set; }
        public GenericJsonApiObject()
        {
            Data = new Data();
        }
    }
}
