using System;

namespace BuildZip.BuildZip.Editor.BuildActions
{
    /// <summary>
    /// A list of pre and post build actions to be performed by the build system.
    /// See <see cref="BuildScript"/> for more information.
    /// </summary>
    [Serializable]
    public class BuildAction
    {
        public string SetupName;

        public bool IsClient;

        public string ItchIOPlatformName;
        
        public bool IncrementBuildVersion;
        
        public bool PublishToItchIO;

        public bool CopyToGoogleDrive;
        
        public bool PublishOnPlayFlow;
        
        public BuildAction()
        {
        }

        public BuildAction(string setupName, bool isClient, string itchIOPlatformName, bool incrementBuildVersionAction = false, bool publishToItchIO = false, bool copyToGoogleDrive = false, bool publishOnPlayFlow = false)
        {
            SetupName = setupName;
            IsClient = isClient;
            ItchIOPlatformName = itchIOPlatformName;
            IncrementBuildVersion = incrementBuildVersionAction;
            PublishToItchIO = publishToItchIO;
            CopyToGoogleDrive = copyToGoogleDrive;
            PublishOnPlayFlow = publishOnPlayFlow;
        }
    }
}