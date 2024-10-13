using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static Vector3 LinearBezier(Vector3 A, Vector3 B, float t)
    {
        return (1 - t) * A + t * B;
    }
    public static Vector3 QuadraticBezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        return (1 - t) * LinearBezier(A, B, t) + t * LinearBezier(B, C, t);
    }
    public static Vector3 CubicBezier(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        return (1 - t) * QuadraticBezier(A, B, C, t) + t * QuadraticBezier(B, C, D, t);
    }
    
    public static Vector3 GetNearestPointOnSegment(Vector3 a, Vector3 b, Vector3 target)
    {
        Vector3 ab = b - a;
        Vector3 ac = target - a;
        float abLengthSquared = ab.sqrMagnitude;

        Vector3 abNorm = ab.normalized;
        float dotProduct = Vector3.Dot(ac, abNorm);
        float clampedDotProduct = Mathf.Clamp(dotProduct, 0, Mathf.Sqrt(abLengthSquared));

        Vector3 nearestPoint = a + abNorm * clampedDotProduct;
        return nearestPoint;
    }
}
