using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.API.IntegrationTests
{
   
    public class RelationshipData
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }

    public interface Relationship
    {

    }

    [DataContract]
    public class OneToRelationship : Relationship
    {
        [DataMember(Name = "data")]
        public RelationshipData Data { get; set; }
    }

    [DataContract]
    public class ManyToRelationship : Relationship
    {
        [DataMember(Name = "data")]
        public IEnumerable<RelationshipData> Data { get; set; }
    }


    [DataContract]
    public class Data
    {
        [DataMember(Name ="type")]
        public string Type { get; set; }

        [DataMember(Name = "id", EmitDefaultValue = false)]
        public int? Id { get; set; }

        [DataMember(Name = "attributes")]
        public Dictionary<string, object> Attributes { get; set; }

        //[DataMember(Name = "relationships", IsRequired = false)]
        //public Dictionary<string, Relationship> Relationships { get; set; }

        public Data()
        {
            Attributes = new Dictionary<string, object>();
            //Relationships = new Dictionary<string, Relationship>();
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
