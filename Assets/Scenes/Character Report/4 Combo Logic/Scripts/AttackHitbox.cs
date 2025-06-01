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
        damageToApply = damage;
        collider.enabled = damageToApply == 0? false : true;
        
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag(targetTag))
        {
            StartCoroutine(DamageFeedBack());
            Debug.Log("Daño a " + other.gameObject.name);
            other.GetComponentInParent<EnemyHealthSystem>()?.DamageDone(damageToApply);
            other.GetComponentInParent<HealthSystem>()?.DamageDone(damageToApply, transform.position);
        }
        
    }

    public IEnumerator DamageFeedBack()
    {
        Time.timeScale = 0;
        yield  return new WaitForSecondsRealtime(damageToApply > 20? 0.05f: 0.025f);
        Time.timeScale = 1;
    }
}
