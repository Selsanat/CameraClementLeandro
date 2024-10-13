using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AViewVolume : MonoBehaviour
{
    public int Priority = 0;
    public GameObject View;

    private int Uid;
    public int _Uid => Uid;
    
    private static int NextUid = 0;

    protected bool IsActive { get; private set; }

    protected virtual void Awake()
    {
        Uid = NextUid++;
    }

    public virtual float ComputeSelfWeight()
    {
        return 1.0f;
    }

    protected void SetActive(bool isActive)
    {
        if (IsActive != isActive)
        {
            IsActive = isActive;

            if (IsActive)
            {
                ViewVolumeBlender.Instance.AddVolume(this);
            }
            else
            {
                ViewVolumeBlender.Instance.RemoveVolume(this);
            }
        }
    }
}
