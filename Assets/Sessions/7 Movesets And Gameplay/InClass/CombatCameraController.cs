using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CombatCameraController : GameSystem
{
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private float detectionRadius;
    private CombatVirtualCamera vCam;

    private Transform currentTarget;

    protected override void Awake()
    {
        base.Awake();
        vCam = GetSubSystem<CombatVirtualCamera>();
    }

    public void ToggleLock()
    {
        if (currentTarget != null)
        {
            currentTarget = null;
            vCam.LockTarget(currentTarget);
            return;
        }
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);
        if (detectedColliders.Length == 0) return;
        int bestFocusedTarget = 0;
        for (int i = 0; i < detectedColliders.Length; i++)
        {
            float focusScore = vCam.GetFocusScore(detectedColliders[i].transform);
            float currentBestScore = vCam.GetFocusScore(detectedColliders[bestFocusedTarget].transform);
            if (1 - focusScore < 1 - currentBestScore) bestFocusedTarget = i;
        }

        currentTarget = detectedColliders[bestFocusedTarget].transform;
        vCam.LockTarget(currentTarget);
    }

    private void Update()
    {
        if (currentTarget == null) return;
        Vector3 forward = (Vector3.ProjectOnPlane(currentTarget.position, Vector3.up) - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(forward, Vector3.up);
        transform.rotation = lookRot;
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
#endif
}
