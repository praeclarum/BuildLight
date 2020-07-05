using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuildLight.Common
{
    public class DeviceClient
    {
        readonly DeviceInfo device;

        static readonly HttpClient httpClient = new HttpClient();

        public DeviceClient(DeviceInfo device)
        {
            this.device = device;
        }

        public async Task SetColorAsync(byte red, byte green, byte blue, CancellationToken token)
        {
            var maxAttempts = 3;


            for (var i = 0; i < maxAttempts && !token.IsCancellationRequested; i++)
            {
                var r = await TrySetColorAsync(red, green, blue, token).ConfigureAwait(false);
                if (r == TryResult.Cancelled || r == TryResult.Success)
                    return;

                var delay = (1 << i) * 10;
                await Task.Delay(delay).ConfigureAwait(false);
            }            
        }

        enum TryResult
        {
            Success,
            Failure,
            Cancelled,
        }

        async Task<TryResult> TrySetColorAsync(byte red, byte green, byte blue, CancellationToken token)
        {
            try
            {
                var host = device.Host;
                var port = device.Port;
                var url = $"http://{host}:31337/color?r={red}&g={green}&b={blue}";
                var color = new[] { red, green, blue };
                var content = new ByteArrayContent(color);
                if (!token.IsCancellationRequested)
                {
                    //Console.WriteLine($"Settting BuildLight {device.FriendlyName}... (r={red}, g={green}, b={blue})");
                    await httpClient.PostAsync(url, content, cancellationToken: token);
                    //Console.WriteLine($"Set BuildLight {device.FriendlyName} (r={red}, g={green}, b={blue})");
                    return TryResult.Success;
                }
                else
                {
                    return TryResult.Cancelled;
                }
            }
            catch (TaskCanceledException)
            {
                //Console.WriteLine($"Cancel BuildLight {device.FriendlyName} (r={red}, g={green}, b={blue})");
                return TryResult.Cancelled;
            }
            catch (Exception ex)
            {
                //LoggingService.LogError("Can't set color", ex);
                Debug.Print($"Can't set color - {ex.Message} - {ex}");
                return TryResult.Failure;
            }
        }
    }
}
