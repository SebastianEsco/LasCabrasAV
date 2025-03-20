using Celeste_Garcia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollisionAdjust : MonoBehaviour
{
    struct RayResult
    {
        public Ray ray;
        public bool result;
        public RaycastHit hitInfo;
    }

    [SerializeField] private Transform HandIK;
    [SerializeField] AvatarIKGoal triggerHand = AvatarIKGoal.RightHand, holdingHand = AvatarIKGoal.LeftHand;

    [SerializeField] private Transform weaponReference;
    [SerializeField] private Transform weaponBarrier;
    [SerializeField] private float weaponLength;
    [SerializeField] private float profileThickness;

    [SerializeField] private LayerMask layerMask;
    

    Animator animator;
    RayResult rayResult;
    [SerializeField] FloatDampener offset;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        SolveOffset();
    }

    private void Update()
    {
        HandIK.Translate(transform.forward * offset.CurrentValue);
    }

    private void SolveOffset()
    {
        RayResult result = new RayResult();
        result.ray = new Ray(weaponReference.position, weaponReference.forward);
        result.result = Physics.SphereCast(result.ray, profileThickness, out result.hitInfo, weaponLength, layerMask);

        rayResult = result;
        offset.TargetValue = Mathf.Max(0,weaponLength - Vector3.Distance(rayResult.hitInfo.point, weaponReference.position));
    }

    private void OnAnimatorIK(int layerIndex)
    {
        offset.Update();
        Vector3 originalIkPosition = animator.GetIKPosition(AvatarIKGoal.RightHand);
        animator.SetIKPositionWeight(triggerHand, 1);
        animator.SetIKPosition(triggerHand, originalIkPosition + weaponReference.forward * offset.CurrentValue);

        animator.SetIKPositionWeight(holdingHand, 1);
        animator.SetIKPosition(holdingHand, weaponBarrier.position);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (weaponReference == null) return;

        Vector3 startPos = weaponReference.position;
        Vector3 endPos = startPos + weaponReference.forward * weaponLength;
        Gizmos.DrawWireSphere(startPos, profileThickness);
        Gizmos.DrawWireSphere(endPos, profileThickness);
    }
#endif
}
