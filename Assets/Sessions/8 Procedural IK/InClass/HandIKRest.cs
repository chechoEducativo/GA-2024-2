using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIKRest : MonoBehaviour
{
    [SerializeField] private Transform detectionReference;
    [SerializeField] private Transform hand;
    [SerializeField] private AvatarIKGoal handGoal;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask detectionLayers;
    [SerializeField] private FloatDamperclass animationTransition;

    private Animator anim;
    private RaycastHit hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        animationTransition.Update();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        Collider[] detectedColliders = Physics.OverlapSphere(detectionReference.position, detectionRadius, detectionLayers);
        if (detectedColliders.Length <= 0) return;
        Vector3 nearestSurfacePoint = detectedColliders[0].ClosestPoint(hand.position);
        foreach (Collider detectedCollider in detectedColliders)
        {
            Vector3 currentClosestPoint = detectedCollider.ClosestPoint(hand.position);
            float currentHandDistance = (currentClosestPoint - hand.position).sqrMagnitude;
            //1. en caso de que la mano este dentro del collider, se asume que la mano es el punto mas cercano
            if (currentHandDistance < 0.01f)
            {
                nearestSurfacePoint = currentClosestPoint;
                break;
            }
            //si el punto actual esta mas cerca de la mano que la variable de referencia, intercambio
            float distanceToSurfacePoint = (nearestSurfacePoint - hand.position).sqrMagnitude;
            if (currentHandDistance < distanceToSurfacePoint)
            {
                nearestSurfacePoint = currentClosestPoint;
            }
        }
        Vector3 rayDir = nearestSurfacePoint - detectionReference.position;
        Ray r = new Ray(detectionReference.position, rayDir);
        bool hasSurface = Physics.Raycast(r, out hit, rayDir.magnitude * 1.05f, detectionLayers);
        animationTransition.TargetValue = hasSurface ? 1 : 0;
        anim.SetIKPositionWeight(handGoal, animationTransition.CurrentValue);
        anim.SetIKPosition(handGoal, hit.point);

        Vector3 rotationAxis = Vector3.Cross(transform.up, hit.normal);
        float rotationAngle = Vector3.Angle(transform.up, hit.normal);
        Quaternion rotation = Quaternion.AngleAxis(rotationAngle, rotationAxis);
        
        anim.SetIKRotationWeight(handGoal, animationTransition.CurrentValue);
        anim.SetIKRotation(handGoal, rotation);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (detectionReference == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(detectionReference.position, detectionRadius);
        Gizmos.DrawLine(hit.point, hit.point+ hit.normal * 0.1f);
    }
#endif
}
