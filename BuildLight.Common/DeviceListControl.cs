using System;
using System.Diagnostics;
using Xwt;

namespace BuildLight.Common
{
    public class DeviceListControl : UIControl
    {
		readonly DeviceList deviceList = DeviceList.Shared;
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

			var toolbar = new Xwt.HBox();
			var refreshButton = new Xwt.Button("Refresh");
			toolbar.PackStart(refreshButton);
            refreshButton.Clicked += RefreshButton_Clicked;


			var vbox = new Xwt.VBox();
			vbox.PackStart(toolbar);
			vbox.PackStart(deviceListView, fill: true, expand: true);

			Widget = vbox;
		}

        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            try
            {
				await deviceList.RefreshAsync();
            }
            catch (Exception ex)
            {
				PresentError(ex);
			}
        }
    }
}
