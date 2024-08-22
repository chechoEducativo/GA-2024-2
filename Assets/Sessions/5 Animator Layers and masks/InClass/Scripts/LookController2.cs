using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class LookController2 : MonoBehaviour
{
    [SerializeField] private VectorDampener lookVector;
    [SerializeField] private Transform lookRig;
    [SerializeField] private float sensitivity;
    [SerializeField] private Vector2 verticalRotationLimits;

    private float rotationy;
    
    public void Look(InputAction.CallbackContext ctx)
    {
        lookVector.TargetValue = ctx.ReadValue<Vector2>() / new Vector2(Screen.width, Screen.height);
    }
    
    private void Update()
    {
        lookVector.Update();
        lookRig.RotateAround(lookRig.position, transform.up, lookVector.CurrentValue.x * sensitivity * 360f);
        rotationy -= lookVector.CurrentValue.y * sensitivity * 360f;
        rotationy = Mathf.Clamp(rotationy, verticalRotationLimits.x, verticalRotationLimits.y);
        Vector3 euler = lookRig.localEulerAngles;
        lookRig.localEulerAngles = new Vector3(rotationy, euler.y, euler.z);
    }
}
