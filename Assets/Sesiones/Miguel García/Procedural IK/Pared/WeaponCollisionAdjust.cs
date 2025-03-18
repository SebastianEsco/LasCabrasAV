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

    [SerializeField] private Transform weaponReference;
    [SerializeField] private float weaponLength;
    [SerializeField] private float profileThickness;

    [SerializeField] private LayerMask layerMask;

    Animator animator;
    RayResult rayResult;
    float offset;

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
        HandIK.Translate(transform.forward * offset);
    }

    private void SolveOffset()
    {
        RayResult result = new RayResult();
        result.ray = new Ray(weaponReference.position, weaponReference.forward);
        result.result = Physics.SphereCast(result.ray, profileThickness, out result.hitInfo, weaponLength, layerMask);

        rayResult = result;
        offset = Mathf.Max(0,weaponLength - Vector3.Distance(rayResult.hitInfo.point, weaponReference.position));
    }

    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 originalIkPosition = animator.GetIKPosition(AvatarIKGoal.RightHand);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, originalIkPosition + weaponReference.forward * offset);
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
