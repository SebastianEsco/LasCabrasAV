using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speedX, speedY;
    private int speedXHash, speedYHash;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
       // speedXHash = animator.StringToHash("SpeedX");
    }

#if UNITY_EDITOR
    private void Update()
    {
        animator.SetFloat("SpeedX", speedX);
        animator.SetFloat("SpeedY", speedY);

    }
#endif
}
