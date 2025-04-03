using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoBoneIK : MonoBehaviour
{
    [SerializeField] private AvatarIKGoal ikGoal;
    [SerializeField] private AvatarIKHint ikHint;

    [SerializeField] private Transform ikTarget;
    [SerializeField] private Transform hintTarget;

    [SerializeField] [Range(0,1)] private float targetWeight;
    [SerializeField][Range(0, 1)] private float hintWeight;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float tWeight = targetWeight  * (ikTarget == null ? 0:1);
        anim.SetIKPositionWeight(ikGoal, tWeight);
        anim.SetIKRotationWeight(ikGoal, tWeight);

        anim.SetIKPosition(ikGoal, ikTarget.position);
        anim.SetIKRotation(ikGoal, ikTarget.rotation);

        float hWeight = hintWeight * (hintTarget == null? 0:1);

        anim.SetIKHintPositionWeight(ikHint, hWeight);
        anim.SetIKHintPosition(ikHint, hintTarget.position);
    }
}
