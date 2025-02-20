using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeste_Garcia
{
    public class LockTarget : MonoBehaviour, ICharacterComponent
    {
        [SerializeField] Camera camera;
        [SerializeField] float detectionRadius, detectionAngle;
        [SerializeField] LayerMask detectionMask;

        public Character parentCharacter { get; set; }

        public void OnLock(InputAction.CallbackContext ctx)
        {
            if (!ctx.started) return;

            Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);

            if(detectedObjects.Length == 0) return;

            float nearestAngle = detectionAngle;
            float nearestDistance = detectionRadius;
            int closestObject = 0;
            Vector3 cameraForward = camera.transform.forward;

            for (int i = 0; i < detectedObjects.Length; i++)
            {
                Collider obj = detectedObjects[i];
                Vector3 objViewDirection = obj.transform.position - camera.transform.position;
                float dot = Vector3.Dot(cameraForward, objViewDirection.normalized);
                float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (angle > detectionAngle) continue;

                float distance = Vector3.Distance(obj.transform.position, transform.position);

                if(distance < nearestDistance && angle < nearestAngle) closestObject = i;

                nearestDistance = Mathf.Min(distance, nearestDistance);
                nearestAngle = Mathf.Min(angle, nearestAngle);
            }

            parentCharacter.LockTarget = detectedObjects[closestObject].transform;
        }
    }
}