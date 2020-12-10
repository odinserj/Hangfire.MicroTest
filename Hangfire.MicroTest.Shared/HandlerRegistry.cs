using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Hangfire.MicroTest.Shared
{
    public class HandlerRegistry
    {
        private readonly Dictionary<string, Type> _handlers = new Dictionary<string, Type>();
        private readonly IServiceProvider _provider;

        public HandlerRegistry(IServiceProvider provider)
        {
            _provider = provider;
        }

        public HandlerRegistry Register<T>(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            _handlers.Add(name, typeof(T));
            return this;
        }

        public object FindHandler(string name)
        {
            if (_handlers.TryGetValue(name, out var handlerType))
            {
                return _provider.GetRequiredService(handlerType);
            }

            return null;
        }
    }
}