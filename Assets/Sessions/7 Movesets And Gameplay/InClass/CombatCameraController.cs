using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using InClass;
using UnityEngine;

public class CombatCameraController : GameSystem
{
    [SerializeField] private Character character;
    private CombatVirtualCamera vCam;

    protected override void Awake()
    {
        base.Awake();
        vCam = GetSubSystem<CombatVirtualCamera>();
    }

    public void ToggleLock()
    {
        CharacterState state = character.State;
        if (state.IsLocked)
        {
            state.LockedTarget = null;
            vCam.LockTarget(null);
            return;
        }
        
        //Collider[] detectedColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, 5, 0);
        if (detectedColliders.Length == 0) return;
        int bestFocusedTarget = 0;
        for (int i = 0; i < detectedColliders.Length; i++)
        {
            float focusScore = vCam.GetFocusScore(detectedColliders[i].transform);
            float currentBestScore = vCam.GetFocusScore(detectedColliders[bestFocusedTarget].transform);
            if (1 - focusScore < 1 - currentBestScore) bestFocusedTarget = i;
        }
        state.LockedTarget = detectedColliders[bestFocusedTarget].transform;
        vCam.LockTarget(state.LockedTarget);
    }

    private void Update()
    {
        CharacterState state = character.State;
        if (!state.IsLocked) return;
        Vector3 forward = (Vector3.ProjectOnPlane(state.LockedTarget.position, Vector3.up) - transform.position).normalized;
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
