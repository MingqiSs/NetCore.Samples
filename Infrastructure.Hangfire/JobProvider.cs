using Hangfire.RecurringJobExtensions;
using Hangfire.RecurringJobExtensions.Configuration;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Hangfire
{
    public class JobProvider : IConfigurationProvider
    {
        private static string _jsonFile;
        public JobProvider(string jsonFile)
        {
            _jsonFile = jsonFile;
        }
        public IEnumerable<RecurringJobInfo> Load()
        {
            IList<JobJsonOptions> jobOptionsList = null;
            if (!string.IsNullOrWhiteSpace(_jsonFile))
            {
                jobOptionsList = FileHelper.ReadFile<JobJsonOptions>(_jsonFile);
            }
            var result = new List<RecurringJobInfo>();
            foreach (var type in Assembly.GetEntryAssembly().GetTypes())
            {
                var t=type.Name.EndsWith("Job");
                if (!t) continue;
             
                foreach (var method in type.GetTypeInfo().DeclaredMethods)
                {
                    var attribute = method.GetCustomAttribute<RecurringJobAttribute>(false);
                    if (jobOptionsList == null && attribute == null) continue;

                    RecurringJobAttribute jobAttribute;
                    var recurringJobId = method.GetRecurringJobId();
                    if (jobOptionsList != null && jobOptionsList.Count > 0)
                    {
                        var jobName = method.Name;
                        var cron = jobOptionsList.Where(p => p.JobName == jobName).FirstOrDefault()?.Cron;
                        if (cron == null) continue;
                        var queue = jobOptionsList.Where(p => p.JobName == jobName).FirstOrDefault()?.Queue ?? EnqueuedState.DefaultQueue;

                        jobAttribute = new RecurringJobAttribute(cron);
                        jobAttribute.RecurringJobId = recurringJobId;
                        jobAttribute.Queue = queue;
                    }
                    else
                    {
                        if (!method.IsDefined(typeof(RecurringJobAttribute), false)) continue;
                        if (string.IsNullOrWhiteSpace(attribute.RecurringJobId))
                        {
                            attribute.RecurringJobId = recurringJobId;
                        }
                        jobAttribute = attribute;
                    }
                    result.Add(new RecurringJobInfo
                    {
                        RecurringJobId = jobAttribute.RecurringJobId,
                        Cron = jobAttribute.Cron,
                        Queue = jobAttribute.Queue,
                        Enable = jobAttribute.Enabled,
                        Method = method,
                        TimeZone = TimeZoneInfo.Local
                    });
                }
            }
            return result;
        }
    }
}
