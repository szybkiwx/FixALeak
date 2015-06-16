﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixALeak.JsonApiSerializer.PropertySerializer;
using Microsoft.Practices.Unity;

namespace FixALeak.JsonApiSerializer
{
    public static class SerializerBuilder
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static Serializer Create()
        {
            return container.Value.Resolve<Serializer>();
        }
        
        private static void RegisterTypes(UnityContainer container)
        {
            container.RegisterType<Serializer>();
            
            container.RegisterType<KeyPropertySerializer>();
            container.RegisterType<ValuePropertySerializer>();
            container.RegisterType<CollectionPropertySerializer>();
            container.RegisterType<NullSerializer>();
            container.RegisterType<PropertySerializerAggregate>();
            container.RegisterType<IPropertySerializationContext, PropertySerializationContext>();
            container.RegisterType<ISingleObjectSerializer, SingleObjectSerializer>(); 
        }
    }
}
