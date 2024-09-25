using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

[RequireComponent(typeof(Animator))]
public class MotionController : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private bool isLocked;
    private Animator animator;
    private Vector3 motionVector;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMove(CallbackContext ctx)
    {
        Transform cameraTransform = cam.transform;
        Vector3 cameraForward = Vector3.Lerp(cameraTransform.forward, cameraTransform.up, Vector3.Dot(transform.up, cameraTransform.forward)).normalized;
        cameraForward = Vector3.ProjectOnPlane(cameraForward, transform.up).normalized;
        Vector2 input = ctx.ReadValue<Vector2>();
        motionVector = cameraForward * input.y + cameraTransform.right * input.x;
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
}
