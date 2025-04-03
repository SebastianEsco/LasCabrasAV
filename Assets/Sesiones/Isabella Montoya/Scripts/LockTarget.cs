using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockTarget : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float detectionAngle;

    public Character ParentCharacter { get;set; }
    public void OnLock(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        if(ParentCharacter.LockTarget != null)
        {
            ParentCharacter.LockTarget = null;
            return;
        }

        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);
        if(detectedObjects.Length == 0 ) return;

        float nearestAngle = detectionAngle;
        float nearestDistance = detectionRadius;
        int closestObject = 0;
        Vector3 cameraForward = camera.transform.forward;

        

        for (int i = 0; i < detectedObjects.Length; i++)
        {
            Collider obj = detectedObjects[i];
            if (detectedObjects.Length == 0) return;

            Vector3 objViewDirection = obj.transform.position - camera.transform.position;
            float dot = Vector3.Dot(cameraForward, objViewDirection.normalized);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            if (angle > detectionAngle) 
                continue; //descarta la interacción
            float distance = Vector3.Distance(obj.transform.position, transform.position);

            if (distance < nearestDistance && angle < nearestAngle)
                closestObject = i;

            nearestDistance = Mathf.Min(nearestDistance, distance);
            nearestAngle= Mathf.Min(angle, nearestAngle);

           
        }

        ParentCharacter.LockTarget = detectedObjects[closestObject].transform;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
       Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

#endif

}
