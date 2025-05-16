using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour, IDamageReceiver<float>
{
       
    
   public void ReceiveDamage(float damage)
   {
        //reducir vida del personaje
        //accionar muerte si la vida es baja
        Debug.Log("muerto");
   }

    



}
