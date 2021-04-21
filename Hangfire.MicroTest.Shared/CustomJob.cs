using System;
using System.ComponentModel;
using System.Linq;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.Storage;
using Newtonsoft.Json;

namespace Hangfire.MicroTest.Shared
{
    public class CustomJob
    {
        public CustomJob(
            [NotNull] string type,
            [NotNull] string method,
            [CanBeNull] string parameterTypes,
            [CanBeNull] string args,
            [CanBeNull] JobFilterAttribute[] typeFilters,
            [CanBeNull] JobFilterAttribute[] methodFilters)
        {
            Type = type;
            Method = method;
            ParameterTypes = parameterTypes;
            Args = args;
            TypeFilters = typeFilters;
            MethodFilters = methodFilters;
        }
        
        [JsonProperty("t")]
        public string Type { get; }
        
        [JsonProperty("m")]
        public string Method { get; }
        
        [JsonProperty("p", NullValueHandling = NullValueHandling.Ignore)]
        public string ParameterTypes { get; }
        
        [JsonProperty("a", NullValueHandling = NullValueHandling.Ignore)]
        public string Args { get; }

        [JsonProperty("tf", NullValueHandling = NullValueHandling.Ignore)]
        public JobFilterAttribute[] TypeFilters { get; }
        
        [JsonProperty("mf", NullValueHandling = NullValueHandling.Ignore)]
        public JobFilterAttribute[] MethodFilters { get; }

        [DisplayName("{0}")]
        public static void Execute(string displayName, CustomJob customJob, PerformContext context)
        {
            if (customJob == null) throw new ArgumentNullException(nameof(customJob));

            var invocationData = new InvocationData(
                customJob.Type,
                customJob.Method,
                customJob.ParameterTypes ?? String.Empty,
                customJob.Args);

            var job = invocationData.DeserializeJob();

            context.Performer.Perform(new PerformContext(
                context.Storage,
                context.Connection,
                new BackgroundJob(context.BackgroundJob.Id, job, context.BackgroundJob.CreatedAt),
                context.CancellationToken));
        }
    }
}