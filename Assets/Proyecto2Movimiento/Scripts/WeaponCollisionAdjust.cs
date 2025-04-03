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

    [SerializeField] private AvatarIKGoal triggerHand = AvatarIKGoal.RightHand;
    [SerializeField] private AvatarIKGoal holdingHand = AvatarIKGoal.LeftHand;

    [SerializeField] private Transform weaponReference;
    [SerializeField] private Transform weaponHandle;
    [SerializeField] private float weaponLenght;
    [SerializeField] private float profileThickness;

    [SerializeField] private LayerMask layerMask;

    private Animator anim;
    RayResult rayResult;
    [SerializeField] private FloatDampener offset;

    private Character character;

    private void SolveOffset()
    {
        RayResult result = new RayResult();
        result.ray = new Ray(weaponReference.position, weaponReference.forward);
        result.result = Physics.SphereCast(result.ray, profileThickness, out result.hitInfo, weaponLenght, layerMask);
        rayResult = result;

        offset.TargetValue  = Mathf.Max(0, weaponLenght  -  Vector3.Distance(rayResult.hitInfo.point , weaponReference.position)) * -1;

    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        character = GetComponent<Character>();
    }

    private void FixedUpdate()
    {
        SolveOffset();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        offset.Update();

        if (character.IsAiming)
        {

            Vector3 originalIkPosition = anim.GetIKPosition(triggerHand);
            anim.SetIKPositionWeight(triggerHand, 1);
            anim.SetIKPosition(triggerHand, originalIkPosition + transform.forward  * offset.CurrentValue);

            anim.SetIKPositionWeight(holdingHand, 1);
            anim.SetIKPosition(holdingHand, weaponHandle.position);

        }
        else
        {
            anim.SetIKPositionWeight(triggerHand, 0);
            anim.SetIKPositionWeight(holdingHand, 0);
        }


    }


    //private void Update()
    //{
    //    handIk.Translate(transform.forward * offset);
    //}

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {

        if (weaponReference == null) return;

        Vector3 startPos = weaponReference.position;
        Vector3 endPos = startPos + weaponReference.forward * weaponLenght;
        Gizmos.DrawWireSphere(startPos, profileThickness);
        Gizmos.DrawWireSphere(endPos, profileThickness);
        Gizmos.DrawLine(startPos, endPos);

    }
#endif

}
