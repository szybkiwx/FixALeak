using Newtonsoft.Json.Linq;
using System;

namespace FixALeak.JsonApiSerializer
{
    public class OutResourceObject
    {
        public int? ID { get; private set; }

        public object Instance { get; private set; }

        public OutResourceObject(JToken rootNode, Type type)
        {
            Instance = Convert.ChangeType(Activator.CreateInstance(type), type);
            if (rootNode["id"] != null)
            {
                string id = rootNode["id"].ToString();
                ID = int.Parse(id);
                type.GetProperty("ID").SetValue(Instance, ID);
            }
        }
    }
}
