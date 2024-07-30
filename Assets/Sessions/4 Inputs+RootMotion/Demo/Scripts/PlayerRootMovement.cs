using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

[RequireComponent(typeof(PlayerInput))]
public class PlayerRootMovement : MonoBehaviour
{
	[SerializeField] private Animator anim;

	[SerializeField] private Vector2 vec;
	[SerializeField] private Vector2Dampener motionVector;
	private int speedXId;
	private int speedYId;

	private void Awake()
	{
		speedXId = Animator.StringToHash("VelX");
		speedYId = Animator.StringToHash("VelY");
	}

	public void Move(CallbackContext ctx)
	{
		motionVector.Target = ctx.ReadValue<Vector2>();
    }

	private void Update()
	{
		motionVector.Update();
		anim.SetFloat(speedXId,motionVector.Value.x);
		anim.SetFloat(speedYId, motionVector.Value.y);
	}
}
