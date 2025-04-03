using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

[RequireComponent(typeof(Animator))]
public class WeaponActionController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OnWeaponAction(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        animator.SetTrigger("WeaponAction");

        //Activar el sistema de disparo
        //Machetazo:
        //Spawn proyectil
        //movel proyectil

        //Robusto:
        //acceder a algun nexo de datos que se refieran al arma (puede ser un componente especifico para el arma que tenga equipada el personaje)
        //con el componente del arma, se activa su funcion de accionarse

        
    }
}
