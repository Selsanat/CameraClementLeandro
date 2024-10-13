using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;

public class DrawGizmoTest : MonoBehaviour
{
    [SerializeField]
    public Vector3 A, B, C;
    Curve Curve = new Curve();
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Curve.A = A;
        Curve.B = B;
        Curve.C = C;
        Curve.DrawGizmo(Color.red, transform.localToWorldMatrix);
    }
}
