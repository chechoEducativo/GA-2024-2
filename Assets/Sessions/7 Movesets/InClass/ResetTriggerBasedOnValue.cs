using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTriggerBasedOnValue : StateMachineBehaviour
{
    [SerializeField] private string targetTrigger;
    [SerializeField] private string valueParam;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetFloat(valueParam) > 0.5)
        {
            animator.ResetTrigger(targetTrigger);
        }
    }
}
