using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class AimController2 : MonoBehaviour
{
    [SerializeField] private AimConstraint chestAim;
    [SerializeField] private Transform aimRig;
    [SerializeField] private VectorDampener lookVector;
    
    public void Aim(InputAction.CallbackContext ctx)
    {
        bool val = ctx.ReadValueAsButton();
        // Activar o desactivar el constraint de apuntado del pecho
        chestAim.enabled = val;
        // Desencadenar la activacion de la camara de apuntado
        aimRig.gameObject.SetActive(val);
    }

    public void Look(InputAction.CallbackContext ctx)
    {
        lookVector.TargetValue = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        lookVector.Update();
        aimRig.RotateAround(aimRig.position, transform.up, lookVector.CurrentValue.x);
    }

    private void Awake()
    {
        aimRig.gameObject.SetActive(false);
    }
}
