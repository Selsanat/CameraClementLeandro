using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : AView
{
     public Rail rail;              
    public float distanceOnRail; 
    public float speed;         
    public Transform target;       
    public bool isAuto;         
    public float roll, fov = 60;        

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();
        Vector3 positionOnRail = rail.GetPosition(distanceOnRail);
        Vector3 direction = (target.position - positionOnRail).normalized;

        config.yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        config.pitch = -Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        config.roll = roll;
        config.fieldOfView = fov;
        config.pivot = positionOnRail;
        config.distance = Vector3.Distance(positionOnRail, target.position);

        return config;
    }
    private void Update()
    {
        if (rail == null || target == null) return;

        if (isAuto)
        {
            distanceOnRail = GetClosestPointOnRail(target.position);
        }
        else
        {
            float input = Input.GetAxis("Horizontal");
            distanceOnRail += input * speed * Time.deltaTime;
        }

        distanceOnRail = Mathf.Repeat(distanceOnRail, rail.GetLength());
    }

    private float GetClosestPointOnRail(Vector3 targetPosition)
    {
        float closestDistance = float.MaxValue;
        float closestPointOnRail = 0f;
        float currentDistance = 0f;

        List<Vector3> nodes = rail.GetRailNodes();

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            Vector3 nearestPoint = MathUtils.GetNearestPointOnSegment(nodes[i], nodes[i + 1], targetPosition);
            float distanceToTarget = Vector3.Distance(nearestPoint, targetPosition);

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestPointOnRail = currentDistance + Vector3.Distance(nodes[i], nearestPoint);
            }

            currentDistance += Vector3.Distance(nodes[i], nodes[i + 1]);
        }

        if (rail.isLoop)
        {
            Vector3 nearestPoint = MathUtils.GetNearestPointOnSegment(nodes[nodes.Count - 1], nodes[0], targetPosition);
            float distanceToTarget = Vector3.Distance(nearestPoint, targetPosition);

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestPointOnRail = currentDistance + Vector3.Distance(nodes[nodes.Count - 1], nearestPoint);
            }
        }

        return closestPointOnRail;
    }
}
