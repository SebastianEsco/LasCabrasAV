using System;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponentInParent<HealthSystem>().DamageDone(damage, transform.position);
        }
    }

}
