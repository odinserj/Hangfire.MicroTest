using System;
using System.ComponentModel;
using System.Linq;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Storage;
using Newtonsoft.Json;

namespace Hangfire.MicroTest.Shared
{
    public class CustomJob
    {
        public CustomJob(
            [NotNull] string payload,
            [CanBeNull] string args,
            [CanBeNull] JobFilterAttribute[] typeFilters,
            [CanBeNull] JobFilterAttribute[] methodFilters)
        {
            Payload = payload ?? throw new ArgumentNullException(nameof(payload));
            Args = args;
            TypeFilters = typeFilters;
            MethodFilters = methodFilters;
        }
        
        [JsonProperty("p")]
        public string Payload { get; }
        
        [JsonProperty("a", NullValueHandling = NullValueHandling.Ignore)]
        public string Args { get; }

        [JsonProperty("t", NullValueHandling = NullValueHandling.Ignore)]
        public JobFilterAttribute[] TypeFilters { get; }
        
        [JsonProperty("m", NullValueHandling = NullValueHandling.Ignore)]
        public JobFilterAttribute[] MethodFilters { get; }

        [DisplayName("{0}")]
        public static void Execute(string displayName, CustomJob customJob)
        {
            if (customJob == null) throw new ArgumentNullException(nameof(customJob));
            
            var invocationData = InvocationData.DeserializePayload(customJob.Payload);
            invocationData.Arguments = customJob.Args;
            var job = invocationData.DeserializeJob();

            if (!job.Method.IsStatic)
            {
                throw new NotImplementedException("Only support for static methods is implemented!");
            }

            job.Method.Invoke(null, job.Args.ToArray());
        }
    }
}