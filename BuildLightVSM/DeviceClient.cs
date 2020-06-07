using System;

using System.Net.Http;
using System.Threading.Tasks;

namespace BuildLightVSM
{
    public class DeviceClient
    {
        static readonly string Host = "buildlight.local";

        readonly HttpClient httpClient =
            new HttpClient ();

        public DeviceClient()
        {
        }

        public Task SetColorAsync(byte red, byte green, byte blue)
        {
            var url = $"http://{Host}/color";
            var color = new[] { red, green, blue };
            var content = new ByteArrayContent(color);
            return httpClient.PostAsync(url, content);
        }
    }
}
