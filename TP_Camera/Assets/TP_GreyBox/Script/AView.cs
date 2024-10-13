using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class AView : MonoBehaviour
{
    
    public float weight = 1f;
    private Transform target;

    public virtual CameraConfiguration GetConfiguration()
    {
        return new CameraConfiguration();
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
            target = GameObject.FindGameObjectWithTag("Player").transform;
            target.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    
}
