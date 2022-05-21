using System;
using System.Globalization;
using UnityEngine;

namespace BuildZip.BuildZip
{
    public static class BuildVersion
    {
        /// <summary>
        /// Represents a build version number.
        /// Major.Minor.Revision
        /// Used by the build process to zip the build in a folder with the version number.
        /// </summary>
        private class BuildVersionInfo
        {
            public uint Major { get; }
            public uint Minor { get; }
            public uint Revision { get; private set; }

            public static BuildVersionInfo Default => new(0, 0, 0);

            /// <summary>
            /// Parses a string version number into a <see cref="BuildVersionInfo"/>.
            /// </summary>
            /// <param name="version">Version string to be parsed. Must be formatted in the MAJOR.MINOR.REVISION format.</param>
            /// <exception cref="ArgumentException">Thrown when the version is wrongly formatted.</exception>
            public static BuildVersionInfo FromString(string version)
            {
                // Parse the version string
                var versionParts = version.Split('.');
                if (versionParts.Length != 3)
                {
                    throw new ArgumentException("Version string must be in the format of 'Major.Minor.SubMinor'");
                }

                var major = uint.Parse(versionParts[0], CultureInfo.InvariantCulture);
                var minor = uint.Parse(versionParts[1], CultureInfo.InvariantCulture);
                var revision = uint.Parse(versionParts[2], CultureInfo.InvariantCulture);

                return new BuildVersionInfo(major, minor, revision);
            }

            public BuildVersionInfo(uint major, uint minor, uint revision)
            {
                Major = major;
                Minor = minor;
                Revision = revision;
            }

            public void Increment()
            {
                Revision += 1;
            }

            public override string ToString()
            {
                return $"{Major}.{Minor}.{Revision}";
            }
        }

        private const string VersionKey = "Version";

        public static string Version => PlayerPrefs.GetString(VersionKey, BuildVersionInfo.Default.ToString());

        private static BuildVersionInfo GetCurrentBuildVersion()
        {
            return BuildVersionInfo.FromString(Version);
        }

        public static bool IsValidVersion(string version)
        {
            try
            {
                BuildVersionInfo.FromString(version);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void IncrementBuildVersion()
        {
            var newBuildVersion = GetCurrentBuildVersion();
            newBuildVersion.Increment();

            PlayerPrefs.SetString(VersionKey, newBuildVersion.ToString());
        }
    }
}