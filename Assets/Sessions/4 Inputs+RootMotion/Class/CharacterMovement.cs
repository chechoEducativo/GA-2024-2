using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class CharacterMovement : MonoBehaviour
{ 
    [SerializeField] private Animator anim;
    [SerializeField] private VectorDampener motionVector = new VectorDampener(true);
    
    private int velXId;
    private int velYId;
    
    public void Move(CallbackContext ctx)
    {
        Vector2 direction = ctx.ReadValue<Vector2>();
        motionVector.TargetValue = direction;
    }

    public void ToggleSprint(CallbackContext ctx)
    {
        bool val = ctx.ReadValueAsButton();
        motionVector.Clamp = !val;
    }
    
    private void Awake()
    {
        velXId = Animator.StringToHash("VelX");
        velYId = Animator.StringToHash("VelY");
    }
    private void Update()
    {
        motionVector.Update();
        Vector2 direction = motionVector.CurrentValue;
        anim.SetFloat(velXId, direction.x);
        anim.SetFloat(velYId, direction.y);
    }
}
