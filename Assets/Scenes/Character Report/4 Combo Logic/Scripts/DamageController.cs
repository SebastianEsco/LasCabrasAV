using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    private List <DamageMessage> damageList = new List<DamageMessage>();

    [SerializeField] private bool ignoreDamage;

    
    private Animator animator;


    public void EnqueueDamage ( DamageMessage damage )
    {
        if (ignoreDamage || damageList.Any(dmg => dmg.sender == damage.sender)) return;
      
        damageList.Add(damage);
    }

    public void IFrameStart()
    {
        ignoreDamage = true;
    }

    public void IFrameEnd()
    {
        ignoreDamage = false;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 damageDirection = Vector3.zero;
        int damageLevel = 0;
        bool isDead = false;
       foreach(DamageMessage message in damageList)
       {
           Game.Instance.PlayerOne.DepleteHealth(message.amount, out isDead); // -BAD hardcoded player 1
            damageDirection += (message.sender.transform.position - transform.position).normalized;
            damageLevel = (int)Mathf.Max(damageLevel, (int)message.damageLevel);

       }

        if (damageList.Count == 0) return;

        damageDirection = Vector3.ProjectOnPlane(damageDirection.normalized, transform.up);
        float damageAngle = Vector3.SignedAngle(transform.forward, damageDirection, transform.up);
        animator.SetFloat("DamageDirection", (damageAngle/180) * 0.5f + 0.5f);
        animator.SetInteger("DamageLevel",  damageLevel);
        animator.SetTrigger("Damage");

        if (isDead)
            animator.SetTrigger("Die");
    
        damageList.Clear();

    }
}
