using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;


[RequireComponent(typeof(Animator))]
public class AttackController : MonoBehaviour
{

    private Animator anim;

    private void Awake()
    {
        anim= GetComponent<Animator>();
    }
    public void OnLightAttack(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            anim.SetTrigger("Attack");
        }
    }

    public void OnHeavyAttack(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            anim.SetTrigger("HeavyAttack");
        }
    }
}
