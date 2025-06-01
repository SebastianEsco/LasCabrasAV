using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CambioDeModoDeCombate : MonoBehaviour
{
   public Animator animator;
   public RuntimeAnimatorController animatorLigeros, animatorPesados;
   public GameObject katana, espada;
   public AttacksController ligeros, pesados;
   private bool estadoLigero;

   private void Awake()
   {
      if (animator.runtimeAnimatorController == animatorLigeros)  estadoLigero = true;
      else estadoLigero = false;
   }

   public void OnToggleattackType(InputAction.CallbackContext ctx)
   {
      if (ctx.performed)
      {
         if (!estadoLigero)
         {
            animator.runtimeAnimatorController = animatorLigeros;
            katana.SetActive(true); espada.SetActive(false);
            ligeros.enabled = true; pesados.enabled = false;
            estadoLigero = true;
         }
         else
         {
            animator.runtimeAnimatorController = animatorPesados;
            katana.SetActive(false); espada.SetActive(true);
            ligeros.enabled = false; pesados.enabled = true;
            estadoLigero = false;
         }
      }
      
   }
}
