using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuildLight.Common
{
    public class DeviceClient
    {
        readonly static Random chaosMonkey = new Random();

        readonly DeviceInfo device;

        public DeviceClient(DeviceInfo device)
        {
            this.device = device;
        }

        public async Task SetColorAsync(byte red, byte green, byte blue, CancellationToken token)
        {
            try
            {
#if MONKEY
                if (chaosMonkey.NextDouble() > 0.5)
                {
                    throw new Exception("CHAOS");
                }
#endif
                var host = device.Host;
                var port = device.Port;
                using (var httpClient = new HttpClient())
                {
                    var url = $"http://{host}:{port}/color?r={red}&g={green}&b={blue}";
                    var color = new[] { red, green, blue };
                    var content = new ByteArrayContent(color);
                    if (!token.IsCancellationRequested)
                    {
                        await httpClient.PostAsync(url, content, cancellationToken: token);
                    }
                }
            }
            catch (Exception ex)
            {
                //LoggingService.LogError("Can't set color", ex);
                Debug.Print($"Can't set color - {ex.Message} - {ex}");
            }
        }
    }
}
