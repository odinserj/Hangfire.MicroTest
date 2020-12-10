using System;
using Hangfire.Common;

namespace Hangfire.MicroTest.Shared
{
    public class CustomJob
    {
        public CustomJob(JobFilterAttribute[] filters, params object[] args)
        {
            Filters = filters ?? throw new ArgumentNullException(nameof(filters));
            Args = args ?? throw new ArgumentNullException(nameof(args));
        }

        public JobFilterAttribute[] Filters { get; }
        public object[] Args { get; }
    }
}