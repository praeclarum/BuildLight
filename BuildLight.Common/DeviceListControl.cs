using System;
using System.Collections.Specialized;
using System.Diagnostics;
using Xwt;

namespace BuildLight.Common
{
    public class DeviceListControl : UIControl
    {
		readonly DeviceList deviceList = DeviceList.Shared;

		readonly ListStore deviceListStore;
		readonly DataField<bool> enabledField = new Xwt.DataField<bool>();
		readonly DataField<string> nameField = new Xwt.DataField<string>();
		readonly DataField<string> idField = new Xwt.DataField<string>();
		readonly ListView deviceListView;

		readonly Label statusLabel = new Label();
		readonly Button refreshButton = new Xwt.Button("Refresh");

		public Xwt.Widget Widget { get; }

        public DeviceListControl()
        {

            //
            // Device List
            //
    		deviceListStore = new ListStore(idField, enabledField, nameField);
            deviceListView = new ListView(deviceListStore);
			var enabledColumn = new Xwt.ListViewColumn(
				"Enabled",
				new Xwt.CheckBoxCellView(enabledField) { Editable = true });
			deviceListView.Columns.Add(enabledColumn);
			var nameColumn = new Xwt.ListViewColumn(
				"Device",
				new Xwt.TextCellView(nameField) { Editable = false });
			deviceListView.Columns.Add(nameColumn);

			deviceList.Devices.CollectionChanged += Devices_CollectionChanged;
			PrepopulateList();
			deviceListView.ButtonPressed += DeviceListView_ButtonPressed;

			//
			// Toolbar
			//
			var toolbar = new Xwt.HBox();
			toolbar.PackStart(refreshButton);
			toolbar.PackStart(statusLabel);
            refreshButton.Clicked += RefreshButton_Clicked;

			//
			// Main UI
			//
			var vbox = new Xwt.VBox();
			vbox.PackStart(toolbar);
			vbox.PackStart(deviceListView, fill: true, expand: true);

			Widget = vbox;
		}

        private void DeviceListView_ButtonPressed(object sender, ButtonEventArgs e)
        {
			var rowIndex = deviceListView.GetRowAtPosition(e.Position);
			var uniqueKey = deviceListStore.GetValue<string>(rowIndex, idField);
			if (deviceList.GetDeviceWithUniqueKey(uniqueKey) is DeviceInfo device)
			{
				var enabled = deviceListStore.GetValue<bool>(rowIndex, enabledField);
				device.Enabled = !enabled;
				deviceList.SetNeedsSave();
			}
        }

        private void Devices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						if (e.NewItems != null)
						{
							var i = e.NewStartingIndex;
							foreach (DeviceInfo item in e.NewItems)
							{
								InsertDevice(i, item);
								i++;
							}
						}
						break;
					case NotifyCollectionChangedAction.Move:
						throw new NotImplementedException();
					case NotifyCollectionChangedAction.Remove:
						throw new NotImplementedException();
					case NotifyCollectionChangedAction.Replace:
						throw new NotImplementedException();
					case NotifyCollectionChangedAction.Reset:
						throw new NotImplementedException();
				}
			}
            catch (Exception ex)
            {
				PresentError(ex);
            }
		}

		private void PrepopulateList()
		{
			var index = 0;
			foreach (var device in deviceList.Devices)
			{
				InsertDevice(index, device);
				index++;
			}
		}

		private void InsertDevice(int index, DeviceInfo device)
        {
			var insertedIndex = deviceListStore.InsertRowBefore(index);
			deviceListStore.SetValue(insertedIndex, idField, device.UniqueKey);
			deviceListStore.SetValue(insertedIndex, enabledField, device.Enabled);
			deviceListStore.SetValue(insertedIndex, nameField, device.FriendlyName);
		}

        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            try
            {
				refreshButton.Sensitive = false;
				statusLabel.Text = "Refreshing...";
				await deviceList.RefreshAsync();
				var refreshTime = deviceList.RefreshTime;
				if (refreshTime != null)
				{
					statusLabel.Text = $"Refreshed at {refreshTime.Value.ToShortTimeString ()}";
				}
				else
				{
					statusLabel.Text = "";
				}
				refreshButton.Sensitive = true;
			}
            catch (Exception ex)
            {
				PresentError(ex);
				refreshButton.Sensitive = true;
			}
        }

        protected override void PresentError(Exception exception)
        {
            base.PresentError(exception);
			statusLabel.Text = $"{exception.GetType().Name}: {exception.Message}";
        }
    }
}
