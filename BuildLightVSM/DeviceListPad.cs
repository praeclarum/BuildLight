using System;
using MonoDevelop.Components;
using MonoDevelop.Ide.Gui;

using Xwt;
using Xwt.Backends;

using BuildLight.Common;

namespace BuildLightVSM
{
    public class DeviceListPad : PadContent
	{
		DeviceListControl deviceListControl = new DeviceListControl();

		readonly Control control;

        public DeviceListPad()
        {
			control = (Gtk.Widget)deviceListControl.Widget.GetBackend().NativeWidget;
		}

        public override Control Control => (Control)control;
    }
}
