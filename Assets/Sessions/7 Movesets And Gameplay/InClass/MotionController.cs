using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

[RequireComponent(typeof(Animator))]
public class MotionController : MonoBehaviour
{
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
        animator.SetFloat("MotionX", 0);
        animator.SetFloat("MotionY", motionVector.magnitude > 0.1 ? 1 : -1);
        animator.SetTrigger("Dodge");
    }

    public void OnDodge(CallbackContext ctx)
    {
        if (!gameObject.scene.IsValid()) return;
        bool performed = ctx.ReadValueAsButton();
        if (!performed) return;
        if(!isLocked)
            DodgeNoLock();
        else
        {
            Vector2 truncatedMotion = new Vector2(
                Mathf.Abs(motionVector.x) >= Mathf.Abs(motionVector.y) ? motionVector.x : 0,
                Mathf.Abs(motionVector.y) > Mathf.Abs(motionVector.x) ? motionVector.y : 0
            );
            truncatedMotion = truncatedMotion.sqrMagnitude > 0.1f ? truncatedMotion : Vector2.down;
            animator.SetFloat("MotionX", truncatedMotion.x);
            animator.SetFloat("MotionY", truncatedMotion.y);
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
        Transform cameraTransform = GameGame.Instance.Player.GetComponent<PlayerCameraManager2>().Camera.transform;
        Vector3 cameraForward = Vector3.Lerp(cameraTransform.forward, cameraTransform.up, Vector3.Dot(transform.up, cameraTransform.forward)).normalized;
        cameraForward = Vector3.ProjectOnPlane(cameraForward, transform.up).normalized;
        motionVector = cameraForward * motionVectorDampener.Value.y + cameraTransform.right * motionVectorDampener.Value.x;
        if (!animator.GetBool("Dodging"))
        {
            animator.SetFloat("MotionX", motionVectorDampener.Value.x);
            animator.SetFloat("MotionY", motionVectorDampener.Value.y);
        }
    }
}
