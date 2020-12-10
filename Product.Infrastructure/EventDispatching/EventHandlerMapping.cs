using System;
using System.Linq;
using Template.Shared;
using System.Reflection;
using System.Collections.Generic;

namespace Product.Infrastructure.EvnetDispatching
{
    public class EventHandlerMapping
    {
        public static IDictionary<Type, List<Type>> DomainEventHandlerMapping<THandler>(Assembly domainEventsAssembly, Assembly[] eventHandlersAssembly)
        {
            IDictionary<Type, List<Type>> result =
            new Dictionary<Type, List<Type>>();

            var domainEventTypes = domainEventsAssembly.GetTypes();

            var domainEvents = domainEventTypes
                .Where(at => typeof(DomainEvent).IsAssignableFrom(at)
                 && at.IsClass && !at.IsAbstract && !at.IsInterface);

            foreach (var domainEvent in domainEvents)
            {
                result[domainEvent] = new List<Type>();
                foreach (var assemblyType in eventHandlersAssembly.SelectMany(assembly => assembly.GetTypes()))
                {
                    var interfaces = assemblyType.GetInterfaces();
                    var eventHandlers = assemblyType.GetInterfaces()
                        .Where(inrfc => inrfc.IsGenericType && inrfc.GetGenericTypeDefinition() == typeof(THandler).GetGenericTypeDefinition());

                    if (eventHandlers != null)
                    {
                        foreach (var eventHandler in eventHandlers)
                        {
                            var genericarguments = eventHandler.GetGenericArguments().FirstOrDefault(tp => domainEvent == tp);
                            if (genericarguments != null)
                            {
                                result[domainEvent].Add(assemblyType);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
