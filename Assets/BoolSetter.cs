using System;
using UnityEngine;

public class BoolSetter : StateMachineBehaviour
{
    [Flags]
    enum ExecutionPhase
    {
        None = 0,
        OnStateEnter = 2,
        OnStateExit = 4
    }
    
    [SerializeField] private string parameterName;
    [SerializeField] private bool value;
    [SerializeField] private ExecutionPhase executionPhase;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!executionPhase.HasFlag(ExecutionPhase.OnStateEnter)) return;
        animator.SetBool(parameterName, value);
    }
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!executionPhase.HasFlag(ExecutionPhase.OnStateExit)) return;
        animator.SetBool(parameterName, value);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
