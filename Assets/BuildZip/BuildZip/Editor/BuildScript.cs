using System;
using System.Linq;
using BuildZip.BuildZip.Editor.BuildActions;
using UnityEditor;

namespace BuildZip.BuildZip.Editor
{
    public static class BuildScript
    {
        [MenuItem("Build/Build All")]
        public static void BuildAll()
        {
            BuildDevelopmentClient();
            BuildDevelopmentServer();

            BuildTestingClient();
            BuildTestServer();

            BuildProductionClient();
            BuildProductionServer();
        }

        #region DEV

        [MenuItem("Build/Build Development Client (Local)")]
        public static void BuildDevelopmentClient()
        {
            BuildActions.BuildActions.instance.SetPostBuildAction(new BuildAction()
            {
                SetupName = Setup.Setup.Development.Name,
                IsClient = true,
                ItchIOPlatformName = "Windows"
            });

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
            buildPlayerOptions.locationPathName = "Builds/Development/Client/Windows/Client.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

            Console.WriteLine("Building Client (Windows)...");
            BuildPipeline.BuildPlayer(buildPlayerOptions);
            Console.WriteLine("Build Completed.");
        }


        [MenuItem("Build/Build Development Server (Local)")]
        public static void BuildDevelopmentServer()
        {
            BuildActions.BuildActions.instance.SetPostBuildAction(new BuildAction()
            {
                SetupName = Setup.Setup.Development.Name,
                IsClient = false,
                ItchIOPlatformName = "Windows"
            });

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
            buildPlayerOptions.locationPathName = "Builds/Development/Server/Server.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.subtarget = (int)StandaloneBuildSubtarget.Server;
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

            Console.WriteLine("Building Server (Windows)...");
            BuildPipeline.BuildPlayer(buildPlayerOptions);
            Console.WriteLine("Build Completed.");
        }

        #endregion

        #region TEST

        [MenuItem("Build/Build Testing Client (Itch.io)")]
        public static void BuildTestingClient()
        {
            BuildActions.BuildActions.instance.SetPostBuildAction(new BuildAction()
            {
                SetupName = Setup.Setup.Testing.Name,
                IsClient = true,
                PublishToItchIO = true,
                IncrementBuildVersion = true,
                ItchIOPlatformName = "Windows"
            });

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
            buildPlayerOptions.locationPathName = "Builds/Testing/Client/Windows/Client.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

            Console.WriteLine("Building Client (Windows)...");
            BuildPipeline.BuildPlayer(buildPlayerOptions);
            Console.WriteLine("Build Completed.");
        }


        [MenuItem("Build/Build Test Server (PlayFlow)")]
        public static void BuildTestServer()
        {
            BuildActions.BuildActions.instance.SetPostBuildAction(new BuildAction()
            {
                SetupName = Setup.Setup.Testing.Name,
                PublishOnPlayFlow = true,
                IsClient = false,
                PublishToItchIO = false,
                IncrementBuildVersion = false,
            });

            Console.WriteLine("Building Client (Windows)...");

            // TODO call PlayFlow's build method

            Console.WriteLine("Build Completed.");
        }

        #endregion

        #region PRODUCTION

        [MenuItem("Build/Build Production Client (Itch.io)")]
        public static void BuildProductionClient()
        {
            BuildActions.BuildActions.instance.SetPostBuildAction(new BuildAction()
            {
                SetupName = Setup.Setup.Production.Name,
                IsClient = true,
                PublishToItchIO = true,
                ItchIOPlatformName = "Windows"
            });

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
            buildPlayerOptions.locationPathName = "Builds/Production/Client/Windows/Client.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

            Console.WriteLine("Building Client (Windows)...");
            BuildPipeline.BuildPlayer(buildPlayerOptions);
            Console.WriteLine("Build Completed.");
        }


        [MenuItem("Build/Build Production Server (AWS)")]
        public static void BuildProductionServer()
        {
            BuildActions.BuildActions.instance.SetPostBuildAction(new BuildAction()
            {
                SetupName = Setup.Setup.Production.Name,
                IsClient = false
            });

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
            buildPlayerOptions.locationPathName = "Builds/Production/Server/Server.x86_64";
            buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
            buildPlayerOptions.subtarget = (int)StandaloneBuildSubtarget.Server;
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

            Console.WriteLine("Building Production Server (Linux)...");
            BuildPipeline.BuildPlayer(buildPlayerOptions);
            Console.WriteLine("Build Completed.");

            // TODO publish to AWS
        }

        #endregion
    }
}