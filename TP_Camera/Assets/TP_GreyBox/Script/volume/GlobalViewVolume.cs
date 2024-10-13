using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalViewVolume : AViewVolume
{
    private void Start()
    {
        View.GetComponent<AView>().SetActive(true);
    }
    
}
