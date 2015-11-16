﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FixALeak.JsonApiSerializer.PropertySerializer
{
    public class PropertySerializationContext : IPropertySerializationContext
    {
        private Dictionary<Func<PropertyInfo, object, bool>, IPropertySerializer> _strategies;

        public PropertySerializationContext(PropertySerializerAggregate propertySerializerAggregate)
        {
            _strategies = new Dictionary<Func<PropertyInfo, object, bool>, IPropertySerializer>();
            _strategies.Add(
                (prop, obj) => prop.Name.EndsWith("ID") && prop.Name.Length > 2,
                propertySerializerAggregate.KeyPropertySerializer);
            _strategies.Add(
                (prop, obj) => prop.PropertyType.IsClass && prop.PropertyType != typeof(string) && prop.GetValue(obj) != null,
                propertySerializerAggregate.ValuePropertySerializer);
            _strategies.Add(
                (prop, obj) => prop.GetValue(obj) != null && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string),
                propertySerializerAggregate.CollectionPropertySerializer);
            _strategies.Add(
                (prop, obj) => true, 
                propertySerializerAggregate.NullSerializer);
       
        }

        public IPropertySerializer GetSerializer(object obj, PropertyInfo prop)
        {
            return _strategies.FirstOrDefault(x => x.Key.Invoke(prop, obj)).Value;
        }

     }
}
