using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class AimController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject aimCamera;
    [Serializable] public class AimToggleEvent : UnityEvent<bool> { }

    public AimToggleEvent onAimToggled;
    public AimToggleEvent onAimExit;

    private void ToggleAim(bool val)
    {
        aimCamera.SetActive(val);
    }
    
    public void Aim(InputAction.CallbackContext ctx)
    {
        bool val = ctx.ReadValueAsButton();
        onAimToggled?.Invoke(val);
        ToggleAim(val);
    }
}
