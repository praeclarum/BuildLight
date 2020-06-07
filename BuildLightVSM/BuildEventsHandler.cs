using System;
using GLib;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace BuildLightVSM
{
    public class BuildEventsHandler
    {
        public BuildEventsHandler()
        {
            IdeApp.Initialized += HandleInitialized;
        }

        private void HandleInitialized(object sender, EventArgs e)
        {
            IdeApp.ProjectOperations.StartBuild += HandleStartBuild;
            IdeApp.ProjectOperations.EndBuild += HandleEndBuild;
        }

        private void HandleStartBuild(object sender, BuildEventArgs args)
        {
            Log.Write("BuildLightVSM", LogLevelFlags.Info, "BUILD STARTED");
        }

        private void HandleEndBuild(object sender, BuildEventArgs args)
        {
            Log.Write("BuildLightVSM", LogLevelFlags.Info, "BUILD ENDED");
        }
    }
}
