using System.Collections.Generic;
using Hangfire.Common;

namespace Hangfire.MicroTest.Shared
{
    internal class CustomJobFilterProvider : IJobFilterProvider
    {
        public IEnumerable<JobFilter> GetFilters(Job job)
        {
            foreach (var arg in job.Args)
            {
                if (arg is CustomJob customJob)
                {
                    foreach (var filter in customJob.Filters)
                    {
                        yield return new JobFilter(filter, JobFilterScope.Method, filter.Order);
                    }
                }
            }
        }
    }
}