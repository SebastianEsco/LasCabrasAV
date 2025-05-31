using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public Collider collider;
    private int damageToApply;
    public string targetTag;
    public Transform emisorDeDaño;
    private void Awake()
    {
        collider.isTrigger = true;
    }

    public void ToggleAttackHitbox(int damage)
    {
        collider.enabled = !collider.enabled;
        damageToApply = damage;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            other.GetComponentInParent<EnemyHealthSystem>()?.DamageDone(damageToApply);
            other.GetComponentInParent<HealthSystem>()?.DamageDone(damageToApply, transform.position);
        }
        
    }
}
