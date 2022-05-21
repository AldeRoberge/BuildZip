using UnityEditor;
using UnityEngine;

namespace BuildZip.BuildZip.Editor.BuildActions
{
    [FilePath("VirtualRamen/BuildActions.asset", FilePathAttribute.Location.PreferencesFolder)]
    public class BuildActions : ScriptableSingleton<BuildActions>
    {
        public BuildAction BuildAction = new();
        
        public void SetPostBuildAction(BuildAction action)
        {
            BuildAction = action;
            Save();
        }

        private void Save()
        {
            Save(true);
            Debug.Log("Saved to: " + GetFilePath());
            Log();
        }

        private void Log()
        {
            Debug.Log("BuildActions : " + JsonUtility.ToJson(this, true));
        }
    }
}