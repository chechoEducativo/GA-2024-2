using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    [Serializable]
    public class AimEvent : UnityEvent<bool>
    {
        
    }
    
    [Serializable] public class AimToggleEvent : UnityEvent<bool> { }

    [SerializeField] private Animator anim;
    [SerializeField] private GameObject aimCamera;
    [SerializeField] private Transform cameraRotator;
    [SerializeField] private MultiAimConstraint aimTorso;

    [SerializeField] private Vector2Dampener lookVector;

    public AimEvent onAim;
    
    private float yrotation;
    private bool aiming;

    private void ToggleAim(bool val)
    {
        aimCamera.SetActive(val);
        onAim?.Invoke(val);
    }
    
    public void Aim(InputAction.CallbackContext ctx)
    {
        bool val = ctx.ReadValueAsButton();
        ToggleAim(val);
        anim.SetBool("Aim", val);
        aiming = val;
    }

    public void Look(InputAction.CallbackContext ctx)
    {
        lookVector.Target = (ctx.ReadValue<Vector2>() /new Vector2(Screen.width, Screen.height)) * 180f;
    }

    private void SolveAimRotation()
    {
        cameraRotator.RotateAround(transform.position, transform.up, lookVector.Value.x);
        yrotation -= lookVector.Value.y;
        yrotation = Mathf.Clamp(yrotation, -75, 75);
        Vector3 currentEuler = cameraRotator.localEulerAngles;
        cameraRotator.localEulerAngles = new Vector3(yrotation, currentEuler.y, currentEuler.z);
    }

    private void Update()
    {
        lookVector.Update();
        SolveAimRotation();
        if (aiming)
        {
            Quaternion lookRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(cameraRotator.forward, transform.up), transform.up);
            transform.rotation = lookRot;
            aimTorso.weight = 1;
        }
        else
        {
            aimTorso.weight = 0;
        }
    }

    public Transform CameraRotator => cameraRotator;
}
