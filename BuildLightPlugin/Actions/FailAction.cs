using StreamDeckLib;
using StreamDeckLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuildLightPlugin
{
  [ActionUuid(Uuid="com.thefrankshow.buildlight.action.fail")]
  public class FailAction : BaseStreamDeckActionWithSettingsModel<Models.FailSettingsModel>
  {
	public override async Task OnKeyUp(StreamDeckEventPayload args)
	{
	  await Manager.SetTitleAsync(args.context, SettingsModel.Color.ToString());
	  
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
