using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class Curve
{
    public Vector3 A, B, C;

    public Vector3 GetPosition(float t)
    {
        return MathUtils.QuadraticBezier(A, B, C, t);
    }

    public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
    {
        return localToWorldMatrix.MultiplyPoint(GetPosition(t));
    }

    public void DrawGizmo(Color c, Matrix4x4 localToWorldMatrix)
    {
        Gizmos.color = c;
        Vector3 lastPos = GetPosition(0, localToWorldMatrix);
        for (int i = 1; i <= 100; i++)
        {
            float t = i / 100.0f;
            Vector3 newPos = GetPosition(t, localToWorldMatrix);
            Gizmos.DrawLine(lastPos, newPos);
            lastPos = newPos;
        }
    }
}
