using System;
using System.ComponentModel;
using System.Reflection;

namespace Hangfire.MicroTest.Shared
{
    public sealed class CustomJobDispatcher
    {
        private readonly HandlerRegistry _registry;

        public CustomJobDispatcher(HandlerRegistry registry)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        [DisplayName("custom/{0}")]
        public object Execute(string message, CustomJob job)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (job == null) throw new ArgumentNullException(nameof(job));

            var handler = _registry.FindHandler(message);
            if (handler == null) throw new InvalidOperationException($"No handler found for message '{message}'");

            var type = handler.GetType().GetTypeInfo();

            var method = type.GetMethod("Execute");
            if (method == null) throw new InvalidOperationException($"No public method 'Execute' found in type {type.FullName}");

            return method.Invoke(handler, job.Args);
        }
    }
}