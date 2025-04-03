using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class CharacterJump : MonoBehaviour, ICharacterComponent
{
    private Rigidbody rb;
    private Animator anim;
    [SerializeField] private float jumpForce = 5f;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (isGrounded)
        {
            anim.ResetTrigger("Jump"); // Resetea el trigger para que vuelva a Locomotion
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && isGrounded)
        {
            Debug.Log("Jumping!");
            anim.SetTrigger("Jump"); // Usamos SetTrigger en vez de SetBool
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public Character ParentCharacter { get; set; }
}
