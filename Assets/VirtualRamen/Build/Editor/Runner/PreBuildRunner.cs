using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using VirtualRamen.Build.Editor.BuildActions;

namespace VirtualRamen.Build.Editor.Runner
{
    // Runs actions before the build starts
    public class PreBuildRunner : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            BuildAction buildAction = BuildActions.BuildActions.instance.BuildAction;

            UpdateCurrentSetup(buildAction.SetupName);

            if (buildAction.IncrementBuildVersion)
            {
                IncrementBuildVersion();
            }
        }
        
        private void UpdateCurrentSetup(string setupName)
        {
            Debug.Log("[PreBuildRunner] Setting current setup to '" + setupName + "'");
            Setup.Setup.SetCurrentSetup(setupName);
        }

        private void IncrementBuildVersion()
        {
            Debug.Log("[PreBuildRunner] Incrementing build version.");
            BuildVersion.BuildVersion.IncrementBuildVersion();
            PlayerSettings.bundleVersion = BuildVersion.BuildVersion.Version;
        }
    }
}