using System;
using System.Net;

namespace BuildLight.Common
{
    public class DeviceInfo
    {
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 80;
        public string FriendlyName { get; set; } = "Unknown";
        public string LightId { get; set; } = "1";
        public bool Enabled { get; set; } = true;

        public string UniqueKey => $"{Host}:{Port}:{LightId}";

        public override string ToString()
        {
            return FriendlyName;
        }

        public void SyncFrom(DeviceInfo source)
        {
            Host = source.Host;
            Port = source.Port;
            LightId = source.LightId;
        }
    }
}
