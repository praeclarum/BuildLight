using System;
using MonoDevelop.Components;
using MonoDevelop.Ide.Gui;

using Xwt;
using Xwt.Backends;

namespace BuildLightVSM
{
    public class DeviceListPad : PadContent
	{
		static Control MakeXwtControl()
		{
			var vbox = new Xwt.VBox();
			vbox.PackStart(new Xwt.Label("Hello Chat Room")
			{
				Visible = true
			});
			vbox.PackStart(new Xwt.Label("Hope you're well")
			{
				Visible = true
			});
			vbox.Visible = true;

			var control = (Gtk.Widget)vbox.GetBackend().NativeWidget;
			return (Control)control;
		}

		readonly object control = MakeXwtControl();

        public override Control Control => (Control)control;
    }
}
