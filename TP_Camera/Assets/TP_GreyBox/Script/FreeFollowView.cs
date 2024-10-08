using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : AView
{
    public float[] pitch, roll, fov = new float[3];
    public float yaw, yawSpeed = 1.0f;
    public Vector3 target;
    public Curve curve;
    float curvePosition = 0.0f;
    public float curveSpeed = 1.0f;
}
