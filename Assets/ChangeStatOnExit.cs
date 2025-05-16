using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStatOnExit : StateMachineBehaviour
{
    [SerializeField] private string inputStatName;
    [SerializeField] private string outputStatName;



    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(outputStatName, animator.GetFloat(inputStatName));
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(outputStatName, animator.GetFloat(inputStatName));
    }
}
