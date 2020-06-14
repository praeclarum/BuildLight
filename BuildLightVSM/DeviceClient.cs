using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using MonoDevelop.Core;

namespace BuildLightVSM
{
    public class DeviceClient
    {
        static readonly string Host = "buildlight.local";

        static string? IpAddress = null;

        public DeviceClient()
        {
        }

        public async Task SetColorAsync(byte red, byte green, byte blue)
        {
            try
            {
                var host = await GetIPAsync();
                using var httpClient = new HttpClient();
                var url = $"http://{host}/color?r={red}&g={green}&b={blue}";
                var color = new[] { red, green, blue };
                var content = new ByteArrayContent(color);
                await httpClient.PostAsync(url, content);
            }
            catch (Exception ex)
            {
                LoggingService.LogError("Can't set color", ex);
            }
        }

        static async Task<string> GetIPAsync()
        {
            if (IpAddress is object)
            {
                return IpAddress;
            }
            var ips = await System.Net.Dns.GetHostAddressesAsync(Host);
            if (ips.Length > 0)
            {
                IpAddress = ips[0].ToString ();
                return IpAddress;
            }
            return Host;
        }
    }
}
