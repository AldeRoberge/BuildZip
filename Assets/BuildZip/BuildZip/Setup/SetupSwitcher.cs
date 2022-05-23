using System.Collections;
using System.Collections.Generic;
using BuildZip.BuildZip.Setup;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// See SetupSwitcher
/// </summary>
public class SetupSwitcher : MonoBehaviour
{
    [Button("Development")]
    public void Development()
    {
        ChangeSetupTo(Setup.Development);
    }
    
    [Button("Testing")]
    public void Testing()
    {
        ChangeSetupTo(Setup.Testing);
    }

    [Button("Production")]
    public void Production()
    {
        ChangeSetupTo(Setup.Production);
    }

    void ChangeSetupTo(SetupInfo setup)
    {
        Setup.SetCurrentSetup(setup);
    }
}