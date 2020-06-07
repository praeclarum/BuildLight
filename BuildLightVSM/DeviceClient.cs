using System;

using System.Net.Http;
using System.Threading.Tasks;
using GLib;

namespace BuildLightVSM
{
    public class DeviceClient
    {
        static readonly string Host = "buildlight.local";

        public DeviceClient()
        {
        }

        public async Task SetColorAsync(byte red, byte green, byte blue)
        {
            try
            {
                using var httpClient = new HttpClient();
                var url = $"http://{Host}/color?r={red}&g={green}&b={blue}";
                var color = new[] { red, green, blue };
                var content = new ByteArrayContent(color);
                await httpClient.PostAsync(url, content);
            }
            catch (Exception ex)
            {
                Log.Write("BuildLightVSM", LogLevelFlags.Error, ex.ToString());
            }
        }
    }
}
