using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;


[RequireComponent(typeof(Animator))]
public class AttackController : MonoBehaviour
{

    [SerializeField] private float lightAttackCost;
    [SerializeField] private float heavyAttackCost;

    AttackHitboxController hitboxController;

    private Animator anim;

    private void Awake()
    {
        anim= GetComponent<Animator>();
        hitboxController = GetComponent<AttackHitboxController>();
    }
    public void OnLightAttack(CallbackContext ctx)
    {
        if (ctx.performed)
        {
           //if (Game.Instance.PlayerOne.CurrentStamina > 0)
           //{
                anim.SetTrigger("LightAttack");
           //}
               
        }
    }

    public void OnHeavyAttack(CallbackContext ctx)
    {
        if (ctx.performed || ctx.canceled)
        {

            //if (Game.Instance.PlayerOne.CurrentStamina > 0)
            //{
                anim.SetTrigger("HeavyAttack");
            //}
                
        }
    }

    public void DepleteStamina(float amount)
    {
        Game.Instance.PlayerOne.DepleteStamina(amount);
    }

    public void DepleteStaminaWithParameter(string parameter)
    {
        float motionValue = GetComponent<Animator>().GetFloat(parameter);
        DepleteStamina(motionValue);
    }

    public void ToggleAttackHitbox(int hitboxId)
    {
        hitboxController.ToggleHitboxes(hitboxId);
    }

    public void CleanupAttackHitbox()
    {
       hitboxController.CleanupHitboxes();
    }
}
