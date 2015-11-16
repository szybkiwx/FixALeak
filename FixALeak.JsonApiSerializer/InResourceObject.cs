using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace FixALeak.JsonApiSerializer
{
    public class InResourceObject
    {
        private object _val;

        int? _id;
        string _typeName;

        private static PluralizationService PluralizationService
        {
            get
            {
                return PluralizationService.CreateService(new CultureInfo("en-US"));
            }
        }

        public int ID
        {
            get
            {
                if (_id == null)
                {
                    _id = Int32.Parse(_val.GetType().GetProperty("ID").GetValue(_val).ToString());
                }

                return _id.Value;
            }
        }

        public string TypeName
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_typeName))
                {
                    Type type = _val.GetType();
                    string typeName = type.Name.ToLower();

                    if (type.Namespace == "System.Data.Entity.DynamicProxies")
                    {
                        typeName = typeName.Split('_')[0];
                    }

                    _typeName = PluralizationService.Pluralize(typeName);
                }

                return _typeName;
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

        public InResourceObject(object val)
        {
            _val = val;
        }
    }
}
