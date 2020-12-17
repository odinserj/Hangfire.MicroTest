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
                    if (customJob.MethodFilters != null)
                    {
                        foreach (var filter in customJob.MethodFilters)
                        {
                            yield return new JobFilter(filter, JobFilterScope.Method, null);
                        }
                    }

                    if (customJob.TypeFilters != null)
                    {
                        foreach (var filter in customJob.TypeFilters)
                        {
                            yield return new JobFilter(filter, JobFilterScope.Type, null);
                        }
                    }
                }
            }
        }
    }
}