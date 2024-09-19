using UnityEngine;

public class SetBoolOnEntry : StateMachineBehaviour
{
    [SerializeField] string parameterName;
    [SerializeField] bool parameterValue;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(parameterName, parameterValue);
    }
}
