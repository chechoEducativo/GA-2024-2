using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private Vector3 motionDebug;
    
    private int velXId;
    private int velYId;

#if UNITY_EDITOR
    private void OnValidate()
    {
        Move(motionDebug);
    }
#endif
    
    private void Awake()
    {
        velXId = Animator.StringToHash("VelX");
        velYId = Animator.StringToHash("VelY");
    }

    public void Move(Vector3 motionDirection)
    {
        anim.SetFloat(velXId, motionDirection.x);
        anim.SetFloat(velYId, motionDirection.y);
    }
}
