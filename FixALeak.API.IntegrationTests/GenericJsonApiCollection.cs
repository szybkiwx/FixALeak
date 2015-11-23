using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.API.IntegrationTests
{
    [DataContract]
    public class GenericJsonApiCollection
    {
        [DataMember(Name = "data")]
        public IEnumerable<Data> Data { get; set; }

        public GenericJsonApiCollection()
        {
            Data = new List<Data>();
        }
    }
}
