using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class AimController2 : MonoBehaviour
{
    [SerializeField] private AimConstraint chestAim;
    [SerializeField] private Transform aimRig;
    [SerializeField] private Transform camTransform;
    [SerializeField] private Animator anim;

    private bool aiming;
    public void Aim(InputAction.CallbackContext ctx)
    {
        bool val = ctx.ReadValueAsButton();
        aiming = val;
        chestAim.enabled = val;
        aimRig.gameObject.SetActive(val);
        anim.SetBool("Aim", val);
    }

    private void Awake()
    {
        aimRig.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(aiming) transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(camTransform.forward, transform.up).normalized);
    }
}
