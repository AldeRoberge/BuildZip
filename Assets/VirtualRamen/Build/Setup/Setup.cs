using System;
using UnityEngine;

namespace VirtualRamen.Build.Setup
{
    public static class Setup
    {
        private const string SetupPlayerPrefsKey = "Setup";

        public static readonly SetupInfo Development = new SetupInfo("Development", "dev", 7770, "localhost");
        public static readonly SetupInfo Testing = new SetupInfo("Testing", "test", 23356, "us-east.cloud.playflow.app");
        public static readonly SetupInfo Production = new SetupInfo("Production", "prod", 23356, "us-east.cloud.playflow.app");

        public static readonly SetupInfo DefaultSetupInfo = Development;

        public static SetupInfo GetCurrentSetup()
        {
            string setupName = PlayerPrefs.GetString(SetupPlayerPrefsKey, Setup.DefaultSetupInfo.Name);
            return Setup.GetFromString(setupName);
        }

        public static SetupInfo GetFromString(string setupName)
        {
            switch (setupName)
            {
                case "Development":
                    return Development;
                case "Testing":
                    return Testing;
                case "Production":
                    return Production;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public static void SetCurrentSetup(SetupInfo setupInfo)
        {
            SetCurrentSetup(setupInfo.Name);
        }

        public static void SetCurrentSetup(string setupName)
        {
            PlayerPrefs.SetString(SetupPlayerPrefsKey, setupName);
        }
    }
}