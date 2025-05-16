using UnityEngine;
using System.Collections.Generic;
public class AttacksController : MonoBehaviour
{
    private enum BufferedAttackType { None, Light, Heavy }
    
    private bool comboWindowOpen = false;
    
    [Header("Configuración")]
    public List<Attack> allAttacks = new List<Attack>();
    public float attackBufferTime = 0.25f;

    [Header("Controles")]
    public KeyCode lightAttackKey = KeyCode.Mouse0;
    public KeyCode heavyAttackKey = KeyCode.Mouse1;

    private Animator animator;
    private Attack currentAttack;
    private float comboTimer;
    private List<string> attackHistory = new List<string>();
    private float lastInputTime;
    private BufferedAttackType bufferedAttack = BufferedAttackType.None;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        HandleInput();
        TryBufferedAttack();
        CheckComboTimeout();
    }
    
    
    public void EnableComboWindow()
    {
        comboWindowOpen = true;
    }

    public void EndComboWindow()
    {
        comboWindowOpen = false;
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(lightAttackKey))
        {
            bufferedAttack = BufferedAttackType.Light;
            lastInputTime = Time.time;
        }
        else if (Input.GetKeyDown(heavyAttackKey))
        {
            bufferedAttack = BufferedAttackType.Heavy;
            lastInputTime = Time.time;
        }
    }

    void TryBufferedAttack()
    {
        if (bufferedAttack == BufferedAttackType.None) return;

        if (comboWindowOpen && !animator.IsInTransition(0))
        {
            Attack.AttackType attackType = bufferedAttack == BufferedAttackType.Light ? 
                Attack.AttackType.Light : 
                Attack.AttackType.Heavy;
            TryAttack(attackType);
            bufferedAttack = BufferedAttackType.None;
        }
    }

    void CheckComboTimeout()
    {
        if (currentAttack != null && comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            
            // Si se agotó el tiempo del combo y no hay input bufferizado
            if (comboTimer <= 0 && bufferedAttack == BufferedAttackType.None)
            {
                ResetCombo();
            }
        }
    }
    
    void ForceReturnToIdle()
    {
        ResetCombo();
        animator.Play("Idle"); // Fuerza el estado Idle
    }

    void TryAttack(Attack.AttackType attackType)
    {
        Attack nextAttack = FindNextValidAttack(attackType);
        
        if (nextAttack != null)
        {
            ExecuteAttack(nextAttack);
        }
        else if (CanStartNewCombo())
        {
            nextAttack = FindStartingAttack(attackType);
            if (nextAttack != null)
            {
                ExecuteAttack(nextAttack);
            }
        }
    }

    bool CanStartNewCombo()
    {
        return currentAttack == null || 
               (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= currentAttack.minExitTime && 
               !animator.IsInTransition(0));
    }
    
    Attack FindNextValidAttack(Attack.AttackType attackType)
    {
        if (currentAttack == null) return null;
        
        foreach (var attack in allAttacks)
        {
            // Verificar tipo de ataque
            if (attack.attackType != attackType) continue;
            
            // Verificar si este ataque puede seguir al actual
            bool canFollowCurrent = System.Array.Exists(currentAttack.possibleNextAttacks, 
                x => x == attack.attackName);
            
            // Verificar requisitos de historial de ataques
            bool meetsHistoryRequirements = true;
            foreach (var req in attack.requiredPreviousAttacks)
            {
                if (!attackHistory.Contains(req))
                {
                    meetsHistoryRequirements = false;
                    break;
                }
            }
            
            if (canFollowCurrent && meetsHistoryRequirements)
            {
                return attack;
            }
        }
        
        return null;
    }
    
    Attack FindStartingAttack(Attack.AttackType attackType)
    {
        foreach (var attack in allAttacks)
        {
            // Buscar ataques iniciales (sin requisitos o con requisitos cumplidos)
            if (attack.attackType == attackType && 
                (attack.requiredPreviousAttacks.Length == 0 || 
                 RequirementsMet(attack.requiredPreviousAttacks)))
            {
                return attack;
            }
        }
        return null;
    }
    
    bool RequirementsMet(string[] requirements)
    {
        foreach (var req in requirements)
        {
            if (!attackHistory.Contains(req)) return false;
        }
        return true;
    }
    
    void ExecuteAttack(Attack attack)
    {
        currentAttack = attack;
        comboTimer = attack.comboWindow;
        
        // Registrar en el historial
        attackHistory.Add(attack.attackName);
        
        // Limitar el historial para no consumir mucha memoria
        if (attackHistory.Count > 10)
        {
            attackHistory.RemoveAt(0);
        }
        
        // Disparar la animación
        animator.Play(attack.attackName);
    }
    
    void ResetCombo()
    {
        Debug.Log("Reiniciando a idle");
        currentAttack = null;
        comboTimer = 0;
    }
    
    public void Die()
    {
        if (isDead) return;
    
        isDead = true;
        animator.SetTrigger("Die");
        ResetCombo();
    }

    [ContextMenu( "Revivir" )]
    void Revivir()
    {
        isDead = false;
    }
    
    // Llamado por eventos de animación
    public void OnAttackStart()
    {
        // Bloquear movimiento u otras acciones durante el ataque
    }
    
    public void OnAttackEnd()
    {
        if (comboTimer <= 0 || bufferedAttack == BufferedAttackType.None)
        {
            ResetCombo();
            animator.Play("Idle");
        }
    }
}
