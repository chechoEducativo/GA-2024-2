using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingPicker : StateMachineBehaviour
{
    [SerializeField] private string inputParameter;
    [SerializeField] private string curveValue;
    [SerializeField] private float defaultValue;
    [SerializeField] private string outputParameter;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(outputParameter, Mathf.Lerp(defaultValue, animator.GetFloat(curveValue), animator.GetFloat(inputParameter)));
    }
}
