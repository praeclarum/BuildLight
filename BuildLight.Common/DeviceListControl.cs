using System;
using Xwt;

namespace BuildLight.Common
{
    public class DeviceListControl
    {
		public ListStore DeviceListStore { get; }
		public Xwt.Widget Widget { get; }

        public DeviceListControl()
        {
			var enabledField = new Xwt.DataField<bool>();
			var nameField = new Xwt.DataField<string>();

			DeviceListStore = new ListStore(enabledField, nameField);

			var deviceListView = new ListView(DeviceListStore);

			var enabledColumn = new Xwt.ListViewColumn(
				"Enabled",
				new Xwt.CheckBoxCellView(enabledField) { Editable = true });
			deviceListView.Columns.Add(enabledColumn);

			var nameColumn = new Xwt.ListViewColumn(
				"Device",
				new Xwt.TextCellView(nameField) { Editable = false });
			deviceListView.Columns.Add(nameColumn);

			var vbox = new Xwt.VBox();
			vbox.PackStart(new Xwt.Label("Hello Chat Room")
			{
				Visible = true
			});
			vbox.PackStart(deviceListView);

			Widget = vbox;
		}
	}
}
