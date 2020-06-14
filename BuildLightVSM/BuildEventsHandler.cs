using System;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace BuildLightVSM
{
    public class BuildEventsHandler
    {
        readonly DeviceClient device = new DeviceClient();

        public BuildEventsHandler()
        {
            IdeApp.Initialized += HandleInitialized;
        }

        private void HandleInitialized(object sender, EventArgs e)
        {
            IdeApp.ProjectOperations.StartBuild += HandleStartBuild;
            IdeApp.ProjectOperations.EndBuild += HandleEndBuild;
        }

        private async void HandleStartBuild(object sender, BuildEventArgs args)
        {            
            try
            {
                await device.SetColorAsync(255, 255, 0);
            }
            catch (Exception ex)
            {
                LoggingService.LogError("Failed to handle start build", ex);
            }
        }

        private async void HandleEndBuild(object sender, BuildEventArgs args)
        {
            try
            {
                if (args.Success)
                {
                    await device.SetColorAsync(red: 0, green: 255, 0);
                }
                else
                {
                    await device.SetColorAsync(red: 255, green: 0, 0);
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError("Failed to handle end build", ex);
            }
        }
    }
}
