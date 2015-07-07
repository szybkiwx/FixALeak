using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FixALeak.JsonApiSerializer
{
    public class ResourceObject
    {
        private object _val;

        public int ID
        {
            get
            {
                return Int32.Parse(_val.GetType().GetProperty("ID").GetValue(_val).ToString());
            }
        }

        public string TypeName
        {
            get
            {
                Type type = _val.GetType();
                string name = type.Name.ToLower();
                if (type.Namespace == "System.Data.Entity.DynamicProxies")
                {
                    return name.Split('_')[0];
                }

                return name;
            }
        }

        public object Value
        {
            get
            {
                return _val;
            }
        }

        public JObject GetJObject()
        {
            return JObject.FromObject(new
            {
                type = TypeName,
                id = ID
            });
        }

        public UrlBuilder GetRelatedLink(string relationshipName)
        {
            return GetSelfLink().Resource(relationshipName);
        }

        public UrlBuilder GetRelatedLink(string relationshipName, int id)
        {
            return GetRelatedLink(relationshipName).Id(id);
        }

        public UrlBuilder GetRelatedSelfLink(string relationshipName)
        {
            return GetSelfLink().Resource("relationships").Resource(relationshipName);
        }

        public UrlBuilder GetRelatedSelfLink(string relationshipName, int id)
        {
            return GetRelatedSelfLink(relationshipName).Id(id);
        }

        public UrlBuilder GetSelfLink()
        {
            return GetSelfCollectionLink().Id(ID);
        }

        public UrlBuilder GetSelfCollectionLink()
        {
            return UrlBuilder.Initialize().Resource(TypeName);
        }

        public ResourceObject(object val)
        {
            _val = val;
        }

        public ResourceObject(JObject rootNode)
        {
            var typeNode = rootNode["type"];
            var id = int.Parse(rootNode["id"].ToString());
            var resourceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.Name.ToLower() == typeNode.ToString())
                .SingleOrDefault();
            var instance = Activator.CreateInstance(resourceType);
            if (id != null)
            {
                resourceType.GetProperty("ID").SetValue(instance, int.Parse(id.ToString()));
            }
            _val = instance;
        }
    }
}
