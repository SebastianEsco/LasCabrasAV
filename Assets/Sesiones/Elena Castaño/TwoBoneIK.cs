using UnityEngine;

public class TwoBoneIK : MonoBehaviour
{
    //Que IK queremos afetar
    [SerializeField] AvatarIKGoal ikGoal;
    [SerializeField] AvatarIKHint ikHint;

    [SerializeField] Transform ikTarget, hintTarget;
    [SerializeField][Range(0f, 1f)] float targetWeight, hintWeight; 

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float tWeight = targetWeight * (ikTarget == null ? 0 : 1);
        anim.SetIKPositionWeight(ikGoal, tWeight);
        anim.SetIKPosition(ikGoal, ikTarget.position);

        anim.SetIKRotationWeight(ikGoal, tWeight);
        anim.SetIKRotation(ikGoal, ikTarget.rotation);

        float hweight = hintWeight * (hintTarget == null ? 0 : 1);
        anim.SetIKHintPositionWeight(ikHint, hweight);
        anim.SetIKHintPosition(ikHint, hintTarget.position);

    }
}
