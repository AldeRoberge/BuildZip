using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BuildZip;
using BuildZip.BuildZip;
using Unity.VisualScripting.IonicZip;
using UnityEditor;
using UnityEditor.Callbacks;
using VirtualRamen.Build.Editor.BuildActions;
using Debug = UnityEngine.Debug;

namespace VirtualRamen.Build.Editor.Runner
{
    /// <summary>
    /// After Unity builds the game, this will copy the files to a folder with the build version as a name.
    /// The build version (bundle version) can be set manually in the player settings, or automatically incremented by BuildVersionProcessor.
    /// </summary>
    public static class PostBuildRunner
    {
        public static bool VerboseLogging = false;

        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            BuildAction buildAction = BuildActions.BuildActions.instance.BuildAction;

            if (buildAction.PublishOnPlayFlow)
            {
                Debug.LogWarning("[PostBuildRunner] PublishOnPlayFlow is set to true. Will not perform any post build actions as PlayFlow takes care of building and publishing the server.");
                return;
            }

            var folderName = MoveToFolder(pathToBuiltProject, buildAction, out var directory);

            var zipFileName = GetFullBuildName(buildAction.IsClient) + ".zip";
            string zipFilePath = Path.Combine(directory, zipFileName);

            CompressDirectory(folderName, zipFilePath);


            if (buildAction.PublishToItchIO)
            {
                if (!buildAction.IsClient)
                {
                    Debug.LogWarning("[PostBuildRunner] ItchIO is only supported for client builds.");
                    return;
                }

                // Copy zip to Itchio Butler
                Debug.Log("[PostBuildRunner] Sending game client to Itch.io...");
                SendToItchIO(zipFilePath, BuildVersion.Version, buildAction.ItchIOPlatformName.ToLower(), buildAction.SetupName);
            }

            if (buildAction.CopyToGoogleDrive)
            {
                // Copy zip to Google Drive
                Debug.Log("[PostBuildRunner] Copying to Google Drive...");
                CopyFile(zipFilePath, "G:/My Drive/Alien Garden/Build");
            }
        }

        private static string MoveToFolder(string pathToBuiltProject, BuildAction buildAction, out string directory)
        {
            var folderName = GetFullBuildName(buildAction.IsClient);

            Debug.Log("[PostBuildRunner] Zipping folder with new name '" + folderName + "'");

            directory = Path.GetDirectoryName(pathToBuiltProject);
            string buildVersionFolder = Path.Combine(directory, folderName);

            // Create the folder if it doesn't exist
            if (!Directory.Exists(buildVersionFolder)) Directory.CreateDirectory(buildVersionFolder);

            Debug.Log("[PostBuildRunner] Moving everything to folder : " + buildVersionFolder);
            MoveFolders(directory, buildVersionFolder);

            // Move the files to the new folder
            MoveFiles(directory, buildVersionFolder, ".zip", pathToBuiltProject);
            return buildVersionFolder;
        }

        private static string GetFullBuildName(bool isClient)
        {
            string platform = EditorUserBuildSettings.activeBuildTarget.ToString();
            string platformName = GetFriendlyPlatformName(platform);
            string server = isClient ? "" : "Server-";
            return GameConstants.GameName + "-" + platformName + "-" + server + "V" + BuildVersion.Version;
        }

        private static void SendToItchIO(string filePath, string version, string platform, string channel)
        {
            Debug.Log("[PostBuildRunner] Sending to Itch.io : " + filePath + " with version : " + version + " and channel : " + channel);

            var process = new Process();
            process.StartInfo.FileName = "C:/butler/butler.exe";
            process.StartInfo.Arguments = "push \"" + filePath + "\" --verbose alderoberge/alien-garden:-" + platform + "-" + channel.ToLower() + " --userversion " + version;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();
            process.WaitForExit();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            Debug.Log(output);
            Debug.Log(error);
        }

        // Returns a friendly name for the platform
        private static string GetFriendlyPlatformName(string platform)
        {
            if (platform.Contains("Android")) return "Android";
            if (platform.Contains("iOS")) return "iOS";
            if (platform.Contains("WebGL")) return "WebGL";
            if (platform.Contains("OSX")) return "OSX";
            if (platform.Contains("Windows")) return "Windows";
            if (platform.Contains("Linux")) return "Linux";
            return platform;
        }

        // compress the folder into a ZIP file, uses https://github.com/r2d2rigo/dotnetzip-for-unity
        static void CompressDirectory(string directory, string zipFileOutputPath)
        {
            Debug.Log("[PostBuildRunner] Attempting to compress " + directory + " into " + zipFileOutputPath);
            EditorUtility.DisplayProgressBar("Zipping the Build Folder", zipFileOutputPath, 0f);
            using (ZipFile zip = new ZipFile())
            {
                zip.ParallelDeflateThreshold = -1; // DotNetZip bugfix that corrupts DLLs / binaries http://stackoverflow.com/questions/15337186/dotnetzip-badreadexception-on-extract

                var directoryName = Path.GetFileName(directory);
                zip.AddDirectory(directory, directoryName);

                // Dont include the folder(s) from the previous build(s)
                foreach (var folder in Directory.GetDirectories(directory))
                {
                    string folderName = Path.GetFileName(folder);

                    if (IsValidGameVersionBuildFolder(folderName))
                    {
                        Debug.Log("[PostBuildRunner] Ignoring other build folder: " + folderName + " from zip.");
                        zip.RemoveEntry(folder);
                    }
                }

                // Dont include zip files from the previous build(s)
                foreach (string file in Directory.GetFiles(directory, "*.zip"))
                {
                    Debug.Log("[PostBuildRunner] Excluding file " + file);
                    zip.RemoveEntry(file);
                }

                // Save it up!
                zip.Save(zipFileOutputPath);
            }

            EditorUtility.ClearProgressBar();
        }

        // Move all files from the build folder to the build version folder
        private static void MoveFiles(string fromFolder, string toFolder, params string[] exclude)
        {
            var files = Directory.GetFiles(fromFolder);

            foreach (var file in files)
            {
                if (exclude.Contains(Path.GetFileName(file)))
                {
                    continue;
                }

                // Do not move zip files
                if (file.Contains(".zip")) continue;

                Debug.Log("[PostBuildRunner] Moving " + file + " to " + toFolder);
                string fileName = Path.GetFileName(file);
                string newFilePath = Path.Combine(toFolder, fileName);
                File.Move(file, newFilePath);
            }
        }

        private static void CopyFile(string file, string toFolder)
        {
            // Combine file and toFolder to get the new file path
            string newFilePath = Path.Combine(toFolder, Path.GetFileName(file));

            Debug.Log("[PostBuildRunner] Copying " + file + " to " + newFilePath);
            File.Copy(file, newFilePath, true);
        }

        // Move all the folders inside the directory to the build version folder
        private static void MoveFolders(string directory, string buildVersionFolder)
        {
            // Move the folders inside the BuildVersion folder
            string[] folders = Directory.GetDirectories(directory);
            foreach (string folder in folders)
            {
                // Skip if the folder is a float (it is a previous build)
                if (float.TryParse(Path.GetFileName(folder), out float _)) continue;

                if (IsValidGameVersionBuildFolder(folder))
                {
                    if (VerboseLogging) Debug.Log("[PostBuildRunner] Not moving build folder " + folder + ".");
                    continue;
                }

                if (folder != buildVersionFolder)
                {
                    string folderName = Path.GetFileName(folder);

                    // Don't include build folder(s) from the previous build(s)
                    if (IsValidGameVersionBuildFolder(folderName))
                    {
                        continue;
                    }

                    if (VerboseLogging) Debug.Log("[PostBuildRunner] Moving folder " + folderName + " from '" + folder + "' to '" + buildVersionFolder + "'.");

                    var newFolder = Path.Combine(buildVersionFolder, folderName);

                    // Move the folder inside of the build version folder
                    Directory.Move(folder, newFolder);
                }
            }
        }

        /// <summary>
        /// Used to avoid moving the build folder of the previous build(s)
        /// </summary>
        private static bool IsValidGameVersionBuildFolder(string folderName)
        {
            // Exclude the build folder of the previous build(s), but not the _Data folder
            if (folderName.Contains(GameConstants.GameName) && !folderName.Contains("_Data"))
                return true;

            return false;
        }
    }
}