using System;
using System.Runtime.InteropServices;
using System.Threading;
using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;
using ui = Microsoft.VisualStudio.VSConstants.UICONTEXT;
using System.Diagnostics;
using BuildLight.Common;

namespace BuildLightVS
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(ui.SolutionOpening_string, flags: PackageAutoLoadFlags.BackgroundLoad)]
    [Guid(BuildLightVSPackage.PackageGuidString)]
    public sealed class BuildLightVSPackage : AsyncPackage
    {
        /// <summary>
        /// BuildLightVSPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "523d7267-dc94-44d7-9b26-dea72bcea40e";

        private DTE _dte;
        private Events _events;
        private BuildEvents _buildEvents;

        readonly DeviceClient device = new DeviceClient();
        CancellationTokenSource startCancellationTokenSource = null;
        CancellationTokenSource endCancellationTokenSource = null;

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            _dte = (DTE)await GetServiceAsync(typeof(DTE));
            Assumes.Present(_dte);
            _events = _dte.Events;
            Assumes.Present(_events);

            _buildEvents = _events.BuildEvents;
            _buildEvents.OnBuildBegin += BuildEvents_OnBuildBegin;
            _buildEvents.OnBuildDone += BuildEvents_OnBuildDone;
            _buildEvents.OnBuildProjConfigBegin += BuildEvents_OnBuildProjConfigBegin;
            _buildEvents.OnBuildProjConfigDone += BuildEvents_OnBuildProjConfigDone;
        }

        private async void BuildEvents_OnBuildProjConfigDone(string Project, string ProjectConfig, string Platform, string SolutionConfig, bool Success)
        {
            Debug.Print($"{Project} {ProjectConfig} {Platform} {SolutionConfig} - {Success}");

            try
            {
                startCancellationTokenSource?.Cancel();
                endCancellationTokenSource?.Cancel();
                endCancellationTokenSource = new CancellationTokenSource();

                if (Success)
                {
                    await device.SetColorAsync(red: 0, green: 255, 0, endCancellationTokenSource.Token);
                }
                else
                {
                    await device.SetColorAsync(red: 255, green: 0, 0, endCancellationTokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                //LoggingService.LogError("Failed to handle end build", ex);
                Debug.Print($"Failed to handle end build - {ex.Message} - {ex}");
            }
        }

        private void BuildEvents_OnBuildProjConfigBegin(string Project, string ProjectConfig, string Platform, string SolutionConfig)
        {
            Debug.Print($"{Project} {ProjectConfig} {Platform} {SolutionConfig}");
        }

        private void BuildEvents_OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            switch (Action)
            {
                case vsBuildAction.vsBuildActionBuild:
                case vsBuildAction.vsBuildActionRebuildAll:
                    Debug.Print("Built");
                    break;
                case vsBuildAction.vsBuildActionClean:
                case vsBuildAction.vsBuildActionDeploy:
                default:
                    Debug.Print("Ignore");
                    break;
            }
        }

        private async void BuildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            // Begin Build.
            Debug.Print("Beginning Build.");
            
            try
            {
                startCancellationTokenSource?.Cancel();
                endCancellationTokenSource?.Cancel();
                startCancellationTokenSource = new CancellationTokenSource();
                await device.SetColorAsync(255, 255, 0, startCancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                //LoggingService.LogError("Failed to handle start build", ex);
                Debug.Print($"Failed to handle start build - {ex.Message} - {ex}");
            }

        }

        #endregion
    }
}
