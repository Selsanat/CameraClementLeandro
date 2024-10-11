using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveVolumeDisplay : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.Label("Active Volumes:");

        foreach (var volume in ViewVolumeBlender.Instance.GetActiveViewVolumes())
        {
            GUILayout.Label("Volume: " + volume.name + " - Priority: " + volume.Priority);
        }
    }
}
