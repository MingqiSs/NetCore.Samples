using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Infrastructure.Hangfire
{
    internal class IPHostHelper
    {
        private static ConcurrentDictionary<string, List<string>> ipv4s = new ConcurrentDictionary<string, List<string>>();
        private static ConcurrentDictionary<string, List<string>> ipv6s = new ConcurrentDictionary<string, List<string>>();


        public static List<string> GetIpV4s()
        {
            return ipv4s.GetOrAdd("ipv4s", key => 
            {
                var ipEntry = Dns.GetHostEntryAsync(Dns.GetHostName()).Result;
                return ipEntry.AddressList.Where(c => c.AddressFamily == AddressFamily.InterNetwork).Select(q => q.ToString()).ToList();
            });
        }

        public static List<string> GetIpV6s()
        {
            return ipv4s.GetOrAdd("ipv6s", key =>
            {
                var ipEntry = Dns.GetHostEntryAsync(Dns.GetHostName()).Result;
                return ipEntry.AddressList.Where(c => c.AddressFamily == AddressFamily.InterNetworkV6).Select(q => q.ToString()).ToList();
            });
        }
    }
}
