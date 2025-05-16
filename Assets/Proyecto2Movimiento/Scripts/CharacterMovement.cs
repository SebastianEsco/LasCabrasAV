using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Animator))] 
public class CharacterMovement : MonoBehaviour, ICharacterComponent
{
    [SerializeField] private Camera camera;
    [SerializeField] private FloatDampener speedX;
    [SerializeField] private FloatDampener speedY;
    [SerializeField] private float angularSpeed;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float rotationTreshold;

    private Animator animator;

    private int speedXHash;
    private int speedYHash;

    Quaternion targetRotation;
    private void SolveCharacterRotation()
    {
        Vector3 floorNormal = transform.up;
        Vector3 cameraRealForward = camera.transform.forward;
        float angleInterpolator = Mathf.Abs(Vector3.Dot(cameraRealForward, floorNormal));
        Vector3 cameraForward = Vector3.Lerp(cameraRealForward, camera.transform.up, angleInterpolator).normalized;
        Vector3 characterForward = Vector3.ProjectOnPlane(cameraForward, floorNormal);

        Debug.DrawLine(transform.position, transform.position + characterForward * 2, Color.magenta, 5);

        //Quaternion lookRotation = Quaternion.LookRotation(characterForward, floorNormal);
        //targetRotation = Quaternion.RotateTowards(transform.rotation, lookRotation, angularSpeed);
        targetRotation = Quaternion.LookRotation(characterForward, floorNormal);
    }

    private void ApplyCharacterRotation()
    {
        //float motionMagnitude = Mathf.Sqrt(speedX.TargetValue * speedX.TargetValue + speedY.TargetValue * speedY.TargetValue);
        //float rotationSpeed =ParentCharacter.IsAiming? 1 : Mathf.SmoothStep(0, .1f, motionMagnitude);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * rotationSpeed);

        float motionMagnitude = Mathf.Sqrt(speedX.TargetValue * speedX.TargetValue + speedY.TargetValue * speedY.TargetValue);

        // Evita la rotación si no hay entrada de movimiento
        if (motionMagnitude < 0.01f) return;

        float rotationSpeed = ParentCharacter.IsAiming ? 1 : Mathf.SmoothStep(0, .1f, motionMagnitude);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * rotationSpeed);

    }

    private void ApplyCharacterRotationFromAim()
    {
        Vector3 aimForward = Vector3.ProjectOnPlane(aimTarget.forward, transform.up).normalized;
        Vector3 characterForward = transform.forward;
        float angleCos = Vector3.Dot(characterForward, aimForward); //-1 , 1

        float rotationSpeed = Mathf.SmoothStep(0f, 1f, Mathf.Acos(angleCos) * Mathf.Rad2Deg / rotationTreshold);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * rotationSpeed);
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue  = ctx.ReadValue<Vector2>();
        speedX.TargetValue = inputValue.x;
        speedY.TargetValue = inputValue.y;   

    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        speedXHash = Animator.StringToHash(name: "SpeedX");
        speedYHash = Animator.StringToHash(name: "SpeedY");
    }



    private void Update()
    {
        speedX.Update();
        speedY.Update();

        animator.SetFloat(speedXHash, speedX.CurrentValue);
        animator.SetFloat(speedYHash, speedY.CurrentValue);
        SolveCharacterRotation();

        if(!ParentCharacter.IsAiming) ApplyCharacterRotation();
        //else ApplyCharacterRotationFromAim();
   
    }

    public Character ParentCharacter { get; set; }
}
