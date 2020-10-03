using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using StreamDeckLib;
using StreamDeckLib.Messages;

namespace BuildLightPlugin
{
  [ActionUuid(Uuid="com.thefrankshow.buildlight.action.fail")]
  public class FailAction : BaseStreamDeckActionWithSettingsModel<Models.FailSettingsModel>
  {
	//readonly DeviceList deviceList = DeviceList.Shared;
	CancellationTokenSource? endCancellationTokenSource = null;
	DeviceClient deviceClient = new DeviceClient ();
	Helper helper = new Helper ();

	public override async Task OnKeyUp(StreamDeckEventPayload args)
	{
		var color = SettingsModel.Color.ToString ();
		await Manager.SetTitleAsync (args.context, color);

		endCancellationTokenSource?.Cancel ();
		endCancellationTokenSource = new CancellationTokenSource ();
		//await deviceList.SetColorAsync (red: 255, green: 0, 0, endCancellationTokenSource.Token);
		//var rgb = helper.HexToRGB (color);

		//await deviceClient.SetColorAsync (rgb.r, rgb.g, rgb.b, endCancellationTokenSource.Token);
		await deviceClient.SetColorAsync (255, 0, 0, endCancellationTokenSource.Token);

		//update settings
		await Manager.SetSettingsAsync(args.context, SettingsModel);
	}

	public override async Task OnDidReceiveSettings(StreamDeckEventPayload args)
	{
	  await base.OnDidReceiveSettings(args);
	  await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());
	}

	public override async Task OnWillAppear(StreamDeckEventPayload args)
	{
	  await base.OnWillAppear(args);
	  await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());
	}

  }
}
