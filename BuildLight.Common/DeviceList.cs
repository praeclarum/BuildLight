using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using ListDiff;
using System.Threading;
using System.IO.IsolatedStorage;
using System.IO;

namespace BuildLight.Common
{
    public class DeviceList
    {
        readonly object devicesLock = new object();
        List<DeviceInfo> Devices { get; } = new List<DeviceInfo>();
        public event EventHandler? CollectionChanged;

        const string serviceType = "urn:schemas-upnp-org:device:basic:1";

        public static DeviceList Shared { get; } = new DeviceList();
        public DateTime? RefreshTime { get; private set; }

        bool needsLoad = true;

        public DeviceList()
        {
        }

        public async Task RefreshAsync()
        {
            if (needsLoad)
            {
                needsLoad = false;
                await LoadOldDataAsync();
            }
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

            lock (devicesLock)
            {
	            // var diff = Devices.Diff(discoveredDevices);
	            Devices.MergeInto(discoveredDevices,
			            match: (a, b) => a.UniqueKey == b.UniqueKey,
			            create: (b) => b,
			            update: (source, destination) => destination.SyncFrom(source),
			            delete: (a) => { });
            }

            RefreshTime = DateTime.Now;
            SetNeedsSave();

            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        const string deviceListFileName = "BuildLight_DeviceList.txt";

        async Task LoadOldDataAsync()
        {
            using var isoStore =
                IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            if (isoStore.FileExists(deviceListFileName))
            {
                using var jsonStream = isoStore.OpenFile(deviceListFileName, System.IO.FileMode.Open);
                using var jsonReader = new StreamReader(jsonStream);
                var json = await jsonReader.ReadToEndAsync();

                var loadedDevices = JsonConvert.DeserializeObject<DeviceInfo[]>(json);
                lock (devicesLock)
                {
	                Devices.MergeInto(loadedDevices, (a, b) => a.UniqueKey == b.UniqueKey);
                }
            }
        }

        public void SetNeedsSave()
        {
            SaveDeviceListAsync().ContinueWith(t =>
            {
            });
        }

        async Task SaveDeviceListAsync()
        {
            try
            {
                await Task.Delay(1).ConfigureAwait(false);

                using var isoStore =
                    IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

                var json = JsonConvert.SerializeObject(GetDevicesCopy());

                using var jsonStream = isoStore.OpenFile(deviceListFileName, FileMode.Create);
                using var jsonWriter = new StreamWriter(jsonStream);
                await jsonWriter.WriteAsync(json);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public DeviceInfo? GetDeviceWithUniqueKey(string uniqueKey)
        {
            return GetDevicesCopy().FirstOrDefault(x => x.UniqueKey == uniqueKey);
        }

        class LightInfo
        {
            public string Name { get; set; } = "Unknown";
        }

        public async Task SetColorAsync(byte red, byte green, byte blue, CancellationToken token)
        {
            var enabledDevices = GetDevicesCopy().Where(x => x.Enabled);
            var tasks = enabledDevices.Select(x =>
            {
                var client = new DeviceClient(x);
                return client.SetColorAsync(red, green, blue, token);
            });
            await Task.WhenAll(tasks);
        }

        public List<DeviceInfo> GetDevicesCopy()
        {
            lock(devicesLock)
            {
                return new List<DeviceInfo>(Devices);
            }
        }
    }
}



