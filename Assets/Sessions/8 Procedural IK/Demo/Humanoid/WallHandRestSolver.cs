using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WallHandRestSolver : MonoBehaviour
{
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private Transform detector;
    [SerializeField] private Transform targetHand;
    [SerializeField] private float detectionRadius;
    [SerializeField][Range(0,180)] private float detectionFOV;
    [SerializeField] private FloatDampener animationTransition;

    private bool hasPositionToSnapTo;
    RaycastHit hit;
    private Collider[] suitableColliders = new Collider[5];


    private Animator anim;

    private int PopulateSuitableSurfaces()
    {
        for (int i = 0; i < suitableColliders.Length; i++)
        {
            suitableColliders[i] = null;
        }
        int detectedCount = Physics.OverlapSphereNonAlloc(detector.position, detectionRadius, suitableColliders);
        return detectedCount;
    }

    private bool ComputeNearestSurfacePosition(int validColliderCount, out RaycastHit hit)
    {
        Vector3 nearestSurfacePoint = suitableColliders[0].ClosestPoint(targetHand.position);
        for (int i = 0; i < validColliderCount; i++)
        {
            Collider c = suitableColliders[i];
            Vector3 currentNearestPoint = c.ClosestPoint(targetHand.position);
            float distanceToHand = Vector3.Distance(currentNearestPoint, targetHand.position);
            if (distanceToHand <= 0.01)
            {
                nearestSurfacePoint = targetHand.position;
                break;
            }

            nearestSurfacePoint = distanceToHand > Vector3.Distance(nearestSurfacePoint, targetHand.position)
                ? nearestSurfacePoint
                : currentNearestPoint;
        }
        Vector3 rayDir = nearestSurfacePoint - detector.position;
        float pointAngle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(rayDir.normalized, detector.forward));
        if (pointAngle > detectionFOV * 0.5f)
        {
            Vector3 axis = Vector3.Cross(detector.forward, rayDir.normalized);
            rayDir = Quaternion.AngleAxis(detectionFOV, axis) * detector.forward * rayDir.magnitude;
        }
        Ray r = new Ray(detector.position, rayDir);
        Debug.DrawLine(detector.position, detector.position + rayDir.normalized * rayDir.magnitude * (1 + 0.1f), Color.blue);
        return Physics.Raycast(r, out hit, rayDir.magnitude * (1 + 0.1f), detectionMask);
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
        hit = new RaycastHit
        {
            point = targetHand.position,
            normal = targetHand.forward
        };
    }

    private void OnAnimatorIK(int layerIndex)
    {
        int suitableSurfaces = PopulateSuitableSurfaces();
        hasPositionToSnapTo = false;
        animationTransition.TargetValue = 0;
        if (suitableSurfaces > 0)
        {
            hasPositionToSnapTo = ComputeNearestSurfacePosition(suitableSurfaces, out hit);
            animationTransition.TargetValue = hasPositionToSnapTo ? 1 : 0;
        }
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, animationTransition.CurrentValue);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, animationTransition.CurrentValue);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, hit.point);

        Vector3 rotAxis = Vector3.Cross(Vector3.up, hit.normal);
        float angle = Vector3.Angle(Vector3.up, hit.normal);
        Quaternion rot = Quaternion.AngleAxis(angle * animationTransition.CurrentValue, rotAxis);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, rot);
    }

    private void FixedUpdate()
    {
        animationTransition.Update();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (detector == null)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            Handles.Label(transform.position, "Missing detector!", style);
            return;
        }
        Color drawColor = Color.green;
        if (!hasPositionToSnapTo)
        {
            drawColor = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(hit.point, 0.02f);
            Gizmos.DrawLine(hit.point, hit.point + hit.normal * 0.1f);
        }

        Gizmos.color = drawColor;
        Handles.color = drawColor;
        Vector3 forward = detector.forward;
        Vector3 r = Quaternion.AngleAxis(detectionFOV, detector.up) * forward;
        Vector3 l = Quaternion.AngleAxis(-detectionFOV, detector.up) * forward;
        Vector3 t = Quaternion.AngleAxis(detectionFOV, detector.right) * forward;
        Vector3 d = Quaternion.AngleAxis(-detectionFOV, detector.right) * forward;
        Vector3 center = detector.position;
        Gizmos.DrawLine(center, center + r * detectionRadius);
        Gizmos.DrawLine(center, center + l * detectionRadius);
        Gizmos.DrawLine(center, center + t * detectionRadius);
        Gizmos.DrawLine(center, center + d * detectionRadius);
        Handles.DrawWireArc(center, detector.up, l,detectionFOV * 2,detectionRadius);
        Handles.DrawWireArc(center, detector.right, d,detectionFOV * 2,detectionRadius);
        Handles.DrawWireArc(center, detector.forward, d,360,detectionRadius);
    }
#endif
}
