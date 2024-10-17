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
        if (!state.IsLocked)
        {
            vCam.LockTarget(null);
            return;
        }
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
}
