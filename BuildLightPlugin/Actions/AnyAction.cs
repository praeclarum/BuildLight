using StreamDeckLib;
using StreamDeckLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

//using BuildLight.Common;

namespace BuildLightPlugin
{
  [ActionUuid(Uuid="com.thefrankshow.buildlight.action.any")]
  public class AnyAction : BaseStreamDeckActionWithSettingsModel<Models.AnySettingsModel>
  {
		//readonly DeviceList deviceList = DeviceList.Shared;
		//CancellationTokenSource? endCancellationTokenSource = null;
		
		public override async Task OnKeyUp(StreamDeckEventPayload args)
		{
			await Manager.SetTitleAsync(args.context, SettingsModel.Color.ToString());

			//endCancellationTokenSource?.Cancel ();
			//endCancellationTokenSource = new CancellationTokenSource ();
			//await deviceList.SetColorAsync (red: 255, green: 0, 0, endCancellationTokenSource.Token);

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
