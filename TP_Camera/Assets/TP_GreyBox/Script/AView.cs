using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AView : MonoBehaviour
{
    
    public float weight = 1f;

    public virtual CameraConfiguration GetConfiguration()
    {
        return new CameraConfiguration();
    }

    public void Start()
    {
        SetActive(true);
    }
    public void SetActive(bool isActive)
    {
        if (isActive)
        {
            CameraController.Instance.AddView(this);
        }
        else
        {
            CameraController.Instance.RemoveView(this);
        }
    }

    
}
