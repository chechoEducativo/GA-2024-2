using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CombatVirtualCamera : GameSubSystem
{
    private CinemachineVirtualCamera vcam;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public float GetFocusScore(Transform target)
    {
        Vector3 targetDir = (target.position - transform.position).normalized;
        return Mathf.Clamp01(Vector3.Dot(targetDir, transform.forward) * 0.5f + 0.5f);
    }

    public void LockTarget(Transform target)
    {
        vcam.LookAt = target;
    }
}
