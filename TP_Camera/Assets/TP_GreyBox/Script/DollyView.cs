using System.Collections.Generic;
using UnityEngine;

public class DollyView : AView
{
    public float roll;
    public float fov;
    public Transform target;
    public Rail rail;
    public float distanceOnRail = 0f;
    public float speed = 5f;
    public bool isAuto = false;


    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();
        if (rail == null)
        {
            Debug.LogWarning("Rail non d�fini pour DollyView.");
            return config;
        }
        Vector3 railPosition = rail.GetPosition(distanceOnRail);
        if (target != null && !isAuto)
        {
            Vector3 dir = (target.position - railPosition).normalized;
            config.yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            config.pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        }
        else if (target != null && isAuto)
        {
            float minDistance = float.MaxValue;
            float closestDistanceOnRail = distanceOnRail;
            List<Vector3> nodes = rail.GetRailNodes();
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                Vector3 a = nodes[i];
                Vector3 b = nodes[i + 1];
                Vector3 projectedPoint = MathUtils.GetNearestPointOnSegment(a, b, target.position);
                float distanceToTarget = Vector3.Distance(target.position, projectedPoint);
                if (distanceToTarget < minDistance)
                {
                    minDistance = distanceToTarget;
                    closestDistanceOnRail = GetDistanceOnRailAtSegment(i, projectedPoint);
                }
            }
            distanceOnRail = closestDistanceOnRail;
            Vector3 dir = (target.position - railPosition).normalized;
            config.yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            config.pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        }
        config.roll = roll;
        config.fieldOfView = fov;
        config.pivot = railPosition;
        config.distance = 0;
        return config;
    }
    private float GetDistanceOnRailAtSegment(int segmentIndex, Vector3 projectedPoint)
    {
        float distance = 0f;
        List<Vector3> nodes = rail.GetRailNodes();
        for (int i = 0; i < segmentIndex; i++)
        {
            distance += Vector3.Distance(nodes[i], nodes[i + 1]);
        }
        distance += Vector3.Distance(nodes[segmentIndex], projectedPoint);
        return distance;
    }
    void Update()
    {
        if (!isAuto)
        {
            float input = Input.GetAxis("Horizontal"); 
            distanceOnRail += input * speed * Time.deltaTime;  
            if (rail.isLoop)
            {
                distanceOnRail = Mathf.Repeat(distanceOnRail, rail.GetLength());
            }
            else
            {
                distanceOnRail = Mathf.Clamp(distanceOnRail, 0, rail.GetLength());
            }
        }
    }
}