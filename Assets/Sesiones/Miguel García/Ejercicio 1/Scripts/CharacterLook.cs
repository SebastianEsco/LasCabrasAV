using Celeste_Garcia;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeste_Garcia
{
    public class CharacterLook : MonoBehaviour, ICharacterComponent
    {
        [SerializeField] Transform target;
        [SerializeField] FloatDampener horizontalDampener, verticalDampener;

        [SerializeField] float horizontalRotationSpeed, verticalRotationSpeed;
        [SerializeField] Vector2 verticalRotationLimits;

        float verticalRotation;

        public Character parentCharacter { get; set; }

        public void OnLook(InputAction.CallbackContext ctx)
        {
            Vector2 inputValue = ctx.ReadValue<Vector2>();

            inputValue = inputValue / new Vector2(Screen.width, Screen.height);
            horizontalDampener.TargetValue = inputValue.x;
            verticalDampener.TargetValue = inputValue.y;
        }

        private void Update()
        {
            horizontalDampener.Update();
            verticalDampener.Update();

            ApplyLookRotation();
        }

        private void ApplyLookRotation()
        {
            if (target == null) throw new NullReferenceException("NO HAY TARGET, PONELO EN EL INSPECTOR BB");

            if (parentCharacter.LockTarget != null)
            {
                Vector3 lookDirection = (parentCharacter.LockTarget.position - target.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                target.rotation = rotation;
                return;
            }

            target.RotateAround(target.position, transform.up, horizontalDampener.CurrentValue * horizontalRotationSpeed * Time.deltaTime);
            //Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalDampener.CurrentValue * horizontalRotationSpeed * Time.deltaTime, transform.up);
            //target.rotation *= horizontalRotation;

            verticalRotation += verticalDampener.CurrentValue * verticalRotationSpeed * Time.deltaTime;
            verticalRotation = Mathf.Clamp(verticalRotation, verticalRotationLimits.x, verticalRotationLimits.y);

            Vector3 euler = target.localEulerAngles;
            euler.x = verticalRotation;
            target.localEulerAngles = euler;
        }
    }
}
