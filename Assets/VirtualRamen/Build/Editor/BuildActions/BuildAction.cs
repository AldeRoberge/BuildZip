using System;

namespace VirtualRamen.Build.Editor.BuildActions
{
    [Serializable]
    public class BuildAction
    {
        public string SetupName;

        public bool IsClient;

        public bool IncrementBuildVersion;
        
        public bool PublishToItchIO;

        public bool CopyToGoogleDrive;
        
        public bool PublishOnPlayFlow;
        
        public BuildAction()
        {
        }

        public BuildAction(string setupName, bool isClient, bool incrementBuildVersionAction = false, bool publishToItchIO = false, bool copyToGoogleDrive = false, bool publishOnPlayFlow = false)
        {
            SetupName = setupName;
            IsClient = isClient;
            IncrementBuildVersion = incrementBuildVersionAction;
            PublishToItchIO = publishToItchIO;
            CopyToGoogleDrive = copyToGoogleDrive;
            PublishOnPlayFlow = publishOnPlayFlow;
        }
    }
}