using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;


namespace Demo.RootMotionController
{
	[RequireComponent(typeof(PlayerCameraManager))]
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerRootMovement : MonoBehaviour
	{
		[SerializeField] private Animator anim;
		[SerializeField] private Vector2Dampener motionVector = new Vector2Dampener(0.3f);
		private int speedXId;
		private int speedYId;

		private PlayerCameraManager cameraManager;

		internal Vector3 targetForward;

		public PlayerCameraManager CameraManager
		{
			get
			{
				cameraManager ??= GetComponent<PlayerCameraManager>();
				return cameraManager;
			}
		}

		private void Awake()
		{
			speedXId = Animator.StringToHash("VelX");
			speedYId = Animator.StringToHash("VelY");
		}

		public void Move(CallbackContext ctx)
		{
			motionVector.Target = ctx.ReadValue<Vector2>();
		}

		public void ToggleSprint(CallbackContext ctx)
		{
			motionVector.Clamp = !ctx.ReadValueAsButton();
		}

		private void Update()
		{
			motionVector.Update();
			anim.SetFloat(speedXId, motionVector.Value.x);
			anim.SetFloat(speedYId, motionVector.Value.y);
		}

		private void OnAnimatorMove()
		{
			Transform cameraTransform = CameraManager.Cam.transform;
			Vector3 normal = Vector3.up;
			float upThreshold = Mathf.Abs(Vector3.Dot(cameraTransform.forward, normal));
			Vector3 forward = Vector3.Lerp(cameraTransform.forward, cameraTransform.up, upThreshold).normalized;
			targetForward = Vector3.ProjectOnPlane(forward, normal);
			Quaternion rot = Quaternion.LookRotation(targetForward, Vector3.up);
			float interpolator = MathUtils.ShaderLikeSmoothstep(0, 0.6f, motionVector.Value.magnitude);
			anim.rootRotation = Quaternion.Slerp(anim.rootRotation, rot, Time.deltaTime * 10f * interpolator);
			anim.ApplyBuiltinRootMotion();
			#if UNITY_EDITOR			
			Debug.DrawLine(transform.position, transform.position + targetForward, Color.cyan, 0.03334f);
			Debug.DrawLine(transform.position, transform.position + transform.forward * interpolator, Color.red, 0.03334f);
			#endif
		}
	}
}
