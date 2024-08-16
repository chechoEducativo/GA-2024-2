using UnityEngine;
using UnityEngine.InputSystem;

public class LookController2 : MonoBehaviour
{
    [SerializeField] private VectorDampener lookVector;
    [SerializeField] private Transform lookRig;
    [SerializeField] private float sensitivity;
    
    public void Look(InputAction.CallbackContext ctx)
    {
        lookVector.TargetValue = ctx.ReadValue<Vector2>() / new Vector2(Screen.width, Screen.height);
    }
    
    private void Update()
    {
        lookVector.Update();
        lookRig.RotateAround(lookRig.position, transform.up, lookVector.CurrentValue.x * sensitivity * 360f);
    }
}
