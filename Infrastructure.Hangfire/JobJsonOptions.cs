using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Hangfire
{
    public class JobJsonOptions
    {
        /// <summary>
        /// Job Name
        /// </summary>
        [JsonProperty("job-name")]
#if !NET45
        [JsonRequired]
#endif
        public string JobName { get; set; }
        /// <summary>
        /// Cron expressions
        /// </summary>
        [JsonProperty("cron-expression")]
#if !NET45
        [JsonRequired]
#endif
        public string Cron { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("queue")]
        public string Queue { get; set; }
    }
}
