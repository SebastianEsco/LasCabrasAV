using Celeste_Garcia;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeste_Garcia
{
    public class CharacterLook : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] FloatDampener horizontalDampener, verticalDampener;

        [SerializeField] float horizontalRotationSpeed, verticalRotationSpeed;

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

            Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalDampener.CurrentValue * horizontalRotationSpeed, transform.up);
            transform.rotation *= horizontalRotation;
        }
    }
}
