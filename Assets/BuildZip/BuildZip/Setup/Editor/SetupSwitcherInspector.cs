using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace BuildZip.BuildZip.Setup.Editor
{
    [CustomEditor(typeof(SetupSwitcher))]
    public class SetupSwitcherInspector : OdinEditor
    {
        // On GUI
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SetupInfo setupInfo = Setup.GetCurrentSetup();

            var setupName = setupInfo.Name;
            var setupPort = setupInfo.Port;
            var setupAddress = setupInfo.Address;

            // Horizontal line separator
            EditorGUILayout.Separator();

            // Set the color of the label to red
            GUILayout.Label("Current Setup : " + setupName, SirenixGUIStyles.BoldTitle);

            // Horizontal line separator
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Address : " + setupAddress, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Port : " + setupPort, EditorStyles.boldLabel);

            EditorGUILayout.Separator();
        }
    }
}