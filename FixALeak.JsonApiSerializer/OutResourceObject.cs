using Newtonsoft.Json.Linq;
using System;

namespace FixALeak.JsonApiSerializer
{
    public class OutResourceObject
    {
        public int? ID { get; private set; }

        //public Type ObjectType { get; private set;}
        
        public object Instance { get; private set; }

        public OutResourceObject(JToken rootNode, Type type)
        {
            /*var typeNode = rootNode["type"];

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
            */

           
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
