using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AimBasedOnBone : StateMachineBehaviour
{
    [SerializeField] string aimTargetName;
    [SerializeField] [Range(50, 90)] private float rotationThreshold;

    Transform aimTarget;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aimTarget = animator.GetComponentsInChildren<Transform>().FirstOrDefault(transform => transform.gameObject.name ==aimTargetName);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (aimTarget == null) return;
        Transform characterTransform = animator.transform;
        Vector3 aimForward = Vector3.ProjectOnPlane(aimTarget.forward, characterTransform.up).normalized;
        Vector3 characterForward = characterTransform.forward;
        float angle = Vector3.SignedAngle(characterForward, aimForward, characterTransform.up);
        if(angle> rotationThreshold)
        {
            animator.SetTrigger(angle > 0 ? "RotateR" : "RotateL");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
