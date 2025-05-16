using UnityEngine;
[System.Serializable]
public class Attack
{
    public string attackName;
    public AnimationClip animation;
    public float comboWindow = 0.5f;
    public AttackType attackType;
    public string[] possibleNextAttacks;
    public string[] requiredPreviousAttacks;
    [Range(0.7f, 1f)] public float minExitTime = 0.95f;
    public float recoveryTime = 0.2f;
    

    public enum AttackType { Light, Heavy }
}
