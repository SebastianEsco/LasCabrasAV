using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageSender<TDamage> where TDamage : struct
{
    void SendDamage(IDamageReceiver<TDamage> receiver);
    
}
