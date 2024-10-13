using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class FreeFollowView : AView
{
    //Cannot do multiple assignation, otherwise only the last element will be assigned
    public float[] pitch = new float[3];
    public float[] roll = new float[3];
    public float[] fov = new float[3];
    public float yaw, yawSpeed = 1.0f;
    public Transform target;
    Curve curve = new Curve();
    public float curvePosition = 0.0f;
    public float curveSpeed = 1.0f;
    public float distance = 5f;

    public override CameraConfiguration GetConfiguration()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        yaw  += Input.GetAxis("Mouse X") * yawSpeed;
        curvePosition= Math.Clamp(curvePosition - Input.GetAxis("Mouse Y")/10 * curveSpeed, 0,1);
        distance= Math.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, 0 , 50);
        curve.A = new Vector3(pitch[0], roll[0], fov[0]);
        curve.B = new Vector3(pitch[1], roll[1], fov[1]);
        curve.C = new Vector3(pitch[2], roll[2], fov[2]);
        Matrix4x4 curveToWorldMatrix = Matrix4x4.TRS(target.position, Quaternion.Euler(0, curvePosition, 0), Vector3.one);
        target.rotation = Quaternion.Euler(0, yaw, 0);

        CameraConfiguration config = new CameraConfiguration();
        config.yaw = yaw;
        config.pitch = curve.GetPosition(curvePosition, curveToWorldMatrix).x;
        config.roll = curve.GetPosition(curvePosition, curveToWorldMatrix).y;
        config.fieldOfView = curve.GetPosition(curvePosition, curveToWorldMatrix).z;
        config.pivot = target.position;
        config.distance = distance; 
        return config;
    }
}


