using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))] 
public class CharacterJump : MonoBehaviour, ICharacterComponent
{
    private Animator anim;
    private bool isGrounded;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Verifica si está en el suelo usando un Raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        anim.SetBool("isGrounded", isGrounded);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && isGrounded)
        {
            Debug.Log("Jump animation triggered!");
            anim.SetTrigger("Jump"); // Activa la animación de salto
        }
    }

    public Character ParentCharacter { get; set; }
}
