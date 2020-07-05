using System;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using ListDiff;

namespace BuildLight.Common
{
    public class DeviceList
    {
        public ObservableCollection<DeviceInfo> Devices { get; } = new ObservableCollection<DeviceInfo>();

        const string serviceType = "urn:schemas-upnp-org:device:basic:1";

        public static DeviceList Shared { get; } = new DeviceList();
        public DateTime? RefreshTime { get; private set; }

        public DeviceList()
        {
        }

        public async Task RefreshAsync()
        {
            var httpClient = new HttpClient();
            using var ssdpClient = new Discovery.SSDP.Agents.ClientAgent();
            var services = await Task.Run (() => ssdpClient.Discover(serviceType));
            if (services == null)
                return;
            var lightServices =
                (from s in services
                where s.ServiceType == serviceType
                select s).ToArray ();
            var discoveredDevices = new List<DeviceInfo>();
            foreach (var s in lightServices)
            {
                var descriptorUrl = new Uri(s.Location);
                var apiUrl = $"http://{descriptorUrl.Host}:{descriptorUrl.Port}/api?lights=0";
                var apiRawResult = await httpClient.GetStringAsync(apiUrl);
                var apiResult = JsonConvert.DeserializeObject<Dictionary<string, LightInfo>>(apiRawResult);
                foreach (var kv in apiResult)
                {
                    discoveredDevices.Add(new DeviceInfo
                    {
                        FriendlyName = kv.Value.Name,
                        LightId = kv.Key,
                        Host = descriptorUrl.Host,
                        Port = descriptorUrl.Port,
                        Enabled = true,
                    });
                }
            }

            discoveredDevices.Sort((a, b) => a.UniqueKey.CompareTo(b.UniqueKey));

            var diff = Devices.Diff(discoveredDevices);
            Devices.MergeInto(discoveredDevices, (a, b) => a.UniqueKey == b.UniqueKey);

            RefreshTime = DateTime.Now;
        }

        class LightInfo
        {
            public string Name { get; set; } = "Unknown";
        }
    }
}



