using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace FixALeak.JsonApiSerializer
{
    public class OutResourceObject
    {
        public int? ID { get; private set; }

        public Type ObjectType { get; private set;}
        
        public object Instance { get; private set; }

        public OutResourceObject(JToken rootNode)
        {
            var typeNode = rootNode["type"];

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => {
                try {
                    return assembly.GetTypes();
                }
                catch (FileNotFoundException e) {
                    //LOGGER
                    return new Type[0];
                }
            });

            ObjectType = types.Where(t => t.Name.ToLower() == typeNode.ToString()).SingleOrDefault();

            Instance = Convert.ChangeType(Activator.CreateInstance(ObjectType), ObjectType);
            if (rootNode["id"] != null)
            {
                string id = rootNode["id"].ToString();
                ID = int.Parse(id);
                ObjectType.GetProperty("ID").SetValue(Instance, ID);
            }
        }
    }
}
