using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AimControllerGizmoDrawer
{
    [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.NonSelected)]
    static void DrawGizmosForAimController(AimController controller, GizmoType gyzmoType)
    {
        Transform rotator = controller.CameraRotator;
        Transform arm = rotator.Find("Arm");
        Transform target = rotator.Find("Target");
        if (rotator == null || arm == null || target == null) return;
        Gizmos.color = Color.green;
        Handles.color = Color.green;
        Vector3 projectedVector = Vector3.ProjectOnPlane(arm.position - rotator.position, controller.transform.up);
        Handles.DrawWireDisc(rotator.position, controller.transform.up, projectedVector.magnitude * 0.5f);
        Gizmos.DrawLine(rotator.position + projectedVector.normalized * projectedVector.magnitude * 0.5f, arm.position);
        Matrix4x4 currentGizmoMatrix = Gizmos.matrix;
        Gizmos.matrix = arm.localToWorldMatrix;
        Gizmos.DrawFrustum(Vector3.zero, 45,0.2f, 0.001f, 1.7777778f);
        Gizmos.matrix = currentGizmoMatrix;
        Gizmos.DrawSphere(target.position, 0.015f);
        Gizmos.DrawLine(target.position + rotator.up * 0.1f, target.position + rotator.up * 0.15f);
        Gizmos.DrawLine(target.position + rotator.right * 0.1f, target.position + rotator.right * 0.15f);
        Gizmos.DrawLine(target.position - rotator.right * 0.1f, target.position - rotator.right * 0.15f);
        Gizmos.DrawLine(target.position - rotator.up * 0.1f, target.position - rotator.up * 0.15f);
    }
}
