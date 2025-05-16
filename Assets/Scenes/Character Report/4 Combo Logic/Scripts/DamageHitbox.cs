using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class DamageHitbox : MonoBehaviour, IDamageReceiver<DamageMessage>
{
    [Serializable]
    public class AttackQueueEvent  : UnityEvent<DamageMessage>
    {

    }

    [SerializeField] private float defenseMultiplier = 1.0f;

    public AttackQueueEvent OnHit;
    
   public void ReceiveDamage(DamageMessage damage)
    {
        if(damage.sender == transform.root.gameObject)
        {
            return;
        }

        damage.amount= damage.amount * defenseMultiplier;
        OnHit?.Invoke(damage);
        Debug.Log($"received damage ({damage.amount})");

    }
}
