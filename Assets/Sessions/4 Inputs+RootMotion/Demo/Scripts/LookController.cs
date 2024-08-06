using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LookController : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private Vector2Dampener lookVector;
    
    public void Look(InputAction.CallbackContext ctx)
    {
        lookVector.Target = (ctx.ReadValue<Vector2>() /new Vector2(Screen.width, Screen.height)) * 180f;
    }

    private void Update()
    {
        lookVector.Update();
        camera.RotateAround(transform.position, transform.up, lookVector.Value.x);
    }
}
