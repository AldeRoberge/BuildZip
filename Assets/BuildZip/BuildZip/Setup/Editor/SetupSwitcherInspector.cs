using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
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

            EditorGUILayout.LabelField("Current setup : " + setupName, EditorStyles.largeLabel);

            // Horizontal line separator
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Address : " + setupAddress, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Port : " + setupPort, EditorStyles.boldLabel);

            EditorGUILayout.Separator();
        }
    }
}