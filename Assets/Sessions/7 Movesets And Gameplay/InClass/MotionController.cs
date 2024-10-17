using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace InClass
{
    [RequireComponent(typeof(Animator))]
    public class MotionController : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private bool isLocked;
        private Animator animator;
        private Vector3 motionVector;

        [SerializeField] private Vector2Dampener motionVectorDampener;

        public void OnMove(CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            motionVectorDampener.Target = input;
        }

        void DodgeNoLock()
        {
            transform.forward = motionVector;
            animator.SetFloat("DodgeX", 0);
            animator.SetFloat("DodgeY", motionVector.magnitude > 0.1 ? 1 : -1);
            animator.SetTrigger("Dodge");
        }

        public void OnDodge(CallbackContext ctx)
        {
            if (!gameObject.scene.IsValid()) return;
            bool performed = ctx.ReadValueAsButton();
            if (!performed) return;
            if (!character.State.IsLocked)
                DodgeNoLock();
            else
            {
                Vector2 motionTarget = motionVectorDampener.Target;
                Vector2 truncatedMotion = new Vector2(
                    Mathf.Abs(motionTarget.x) >= Mathf.Abs(motionTarget.y) ? motionTarget.x : 0,
                    Mathf.Abs(motionTarget.y) > Mathf.Abs(motionTarget.x) ? motionTarget.y : 0
                );
                truncatedMotion = truncatedMotion.sqrMagnitude > 0.1f ? truncatedMotion : Vector2.down;
                animator.SetFloat("DodgeX", truncatedMotion.x);
                animator.SetFloat("DodgeY", truncatedMotion.y);
                animator.SetTrigger("Dodge");
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            motionVectorDampener.Update();
            Transform cameraTransform = character.Player.CameraManager.Camera.transform;
            Vector3 cameraForward = Vector3.Lerp(cameraTransform.forward, cameraTransform.up,
                Vector3.Dot(transform.up, cameraTransform.forward)).normalized;
            cameraForward = Vector3.ProjectOnPlane(cameraForward, Vector3.up).normalized;
            motionVector = cameraForward * motionVectorDampener.Value.y +
                           cameraTransform.right * motionVectorDampener.Value.x;
            if (!animator.GetBool("Dodging"))
            {
                if (character.State.IsLocked)
                {
                    animator.SetFloat("MotionX", motionVectorDampener.Value.x);
                    animator.SetFloat("MotionY", motionVectorDampener.Value.y);
                    transform.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);
                }
                else
                {
                    animator.SetFloat("MotionY", motionVectorDampener.Value.magnitude);
                    if(motionVector.magnitude > 0.1)
                        transform.rotation = Quaternion.LookRotation(motionVector.normalized, Vector3.up);
                }
            }

        }
    }
}
