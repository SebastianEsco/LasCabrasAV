using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour, IDamageSender<DamageMessage>
{

    [SerializeField] private DamageMessage damageMessage;


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamageReceiver<DamageMessage> receiver))
        {
            SendDamage(receiver);
        }
    }
    public void SendDamage(IDamageReceiver<DamageMessage> receiver)
    {
        
        receiver.ReceiveDamage(damageMessage);
    }
 
}
