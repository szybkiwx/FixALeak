using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace FixALeak.API.Models
{
   /* public interface IJsonApiData 
    {

    }

    [DataContract]
    public class JsonApiDataObject : IJsonApiData
    {
        [DataMember(Name="type")]
        public string Type { get; set; }

        [DataMember(Name="id")]
        public int ID { get; set; }

        [DataMember(Name = "attributes")]
        public Dictionary<string, object> Attributes { get; set; }

        [DataMember(Name = "relationshops")]
        public Dictionary<string, object> Relationshops { get; set; }

        public JsonApiDataObject()
        {
            Attributes = new Dictionary<string, object>();
        }
    }

    [DataContract]
    public class JsonApiDataCollecion : List<JsonApiDataObject>, IJsonApiData
    {
    }


    [DataContract]
    public class JsonApiModel {
        [DataMember(Name="data")]
        public IJsonApiData Data { get; set; }
    }*/
}


    /**
     *  var data = categories.Select(x => new {
                    type = "categories",
                    id = x.ID,
                    attributes = new {
                        name = x.Name,
                    },
                    links = new {
                        self = "/api/categories/" + x.ID,
                        sub_categories =  x.SubCategories.Select(sub => new {
                            self = "/api/categories/" + x.ID + "/categoryleafs",
                            linkage = new {
                                type = "category_leafs",
                                id = sub.ID
                            }
                        })
                    }
                });
                
                dynamic included;
                if (include == "SubCategories") 
                {
                    included = categories
                        .SelectMany(x => x.SubCategories)
                        .Select(x => new
                        {
                            type = "category_leafs",
                            id = x.ID,
                            attributes = new
                            {
                                name = x.Name
                            }
                        });
                }
                else
     * 
     */

