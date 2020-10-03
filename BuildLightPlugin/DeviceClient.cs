using System;
namespace BuildLightPlugin
{
	public class DeviceClient
	{
		public DeviceClient ()
		{
		}

        public string Host { get; set; } = "127.0.0.1"; //"buildlight"
        public int Port { get; set; } = 80;

        static readonly HttpClient httpClient = new HttpClient ();

        enum TryResult
        {
            Success,
            Failure,
            Cancelled,
        }

        public async Task SetColorAsync (byte red, byte green, byte blue, CancellationToken token)
        {
            var maxAttempts = 3;

            for (var i = 0; i < maxAttempts && !token.IsCancellationRequested; i++) {
                var r = await TrySetColorAsync (red, green, blue, token).ConfigureAwait (false);
                if (r == TryResult.Cancelled || r == TryResult.Success)
                    return;

                var delay = (1 << i) * 10;
                await Task.Delay (delay).ConfigureAwait (false);
            }
        }

        async Task<TryResult> TrySetColorAsync (byte red, byte green, byte blue, CancellationToken token)
        {
            try {
                var host = Host;
                var port = Port;
                var url = $"http://{host}:31337/color?r={red}&g={green}&b={blue}";
                var color = new[] { red, green, blue };
                var content = new ByteArrayContent (color);
                if (!token.IsCancellationRequested) {
                    //Console.WriteLine($"Settting BuildLight {device.FriendlyName}... (r={red}, g={green}, b={blue})");
                    await httpClient.PostAsync (url, content, cancellationToken: token);
                    //Console.WriteLine($"Set BuildLight {device.FriendlyName} (r={red}, g={green}, b={blue})");
                    return TryResult.Success;
                }
                else {
                    return TryResult.Cancelled;
                }
            }
            catch (TaskCanceledException) {
                //Console.WriteLine($"Cancel BuildLight {device.FriendlyName} (r={red}, g={green}, b={blue})");
                return TryResult.Cancelled;
            }
            catch (Exception ex) {
                //LoggingService.LogError("Can't set color", ex);
                Debug.Print ($"Can't set color - {ex.Message} - {ex}");
                return TryResult.Failure;
            }
        }
    }
}
