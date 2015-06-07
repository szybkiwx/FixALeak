﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public class PropertySerializationContext
    {
        Dictionary<Func<PropertyInfo, object, bool>, IPropertySerializer> _strategies;

        public PropertySerializationContext()
        {
            _strategies = new Dictionary<Func<PropertyInfo, object, bool>, IPropertySerializer>();
            _strategies.Add((prop, obj) => { return prop.Name.EndsWith("ID") && prop.Name.Length > 2; }, new KeyPropertySerializer());
            _strategies.Add((prop, obj) => prop.PropertyType.IsClass && prop.PropertyType != typeof(string) && prop.GetValue(obj) != null, new ValuePropertySerializer());
            _strategies.Add((prop, obj) => typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string), new CollectionPropertySerializer());
            _strategies.Add((prop, obj) => true, new NullSerializer());
       
        }

        public JProperty Serialize(object obj, PropertyInfo prop)
        {
            return _strategies.FirstOrDefault(x => x.Key.Invoke(prop, obj)).Value.Serialize(obj, prop);
        } 
    }
}