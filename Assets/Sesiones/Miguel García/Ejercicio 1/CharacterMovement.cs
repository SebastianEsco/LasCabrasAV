using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeste_Garcia
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private FloatDampener speedX, speedY;
        [SerializeField] private Camera camara; //machetazo pero aja, despues lo modificamos

        private Animator animator;

        private int speedXHash;
        private int speedYHash;

        public void OnMove(InputAction.CallbackContext ctx)
        {
            Vector2 inputValue = ctx.ReadValue<Vector2>();

            speedX.TargetValue = inputValue.x;
            speedY.TargetValue = inputValue.y;

            Vector3 floorNormal = transform.up;
            Vector3 cameraRealForward = camara.transform.forward;
            float angleInterpolator = Mathf.Abs(Vector3.Dot(cameraRealForward, floorNormal));
            Vector3 camaraForward = Vector3.Lerp(cameraRealForward, camara.transform.up, angleInterpolator).normalized;

            Vector3 characterForward = Vector3.ProjectOnPlane(camaraForward, floorNormal).normalized;
            Debug.DrawLine(transform.position, transform.position + characterForward * 2, Color.red, 2.0f);
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            speedXHash = Animator.StringToHash("SpeedX");
            speedYHash = Animator.StringToHash("SpeedY");
        }

        void Update()
        {
            speedX.Update();
            speedY.Update();

            animator.SetFloat(speedXHash, speedX.CurrentValue);
            animator.SetFloat(speedYHash, speedY.CurrentValue);
        }
    }
}