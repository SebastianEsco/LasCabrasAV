using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Character : MonoBehaviour
{
    Transform lockTarget;
    [SerializeField] private bool isAiming;
    [SerializeField] private bool isJumping;

    public Transform LockTarget
    {
        get => lockTarget;
        set => lockTarget =value;
    }

    public bool IsAiming
    {
        get => isAiming;
        set => isAiming = value;
    }
    public bool IsJumping
    {
        get => isJumping;
        set
        {
            isJumping = value;
            Debug.Log($"IsJumping set to: {isJumping}");
        }
    }


    private void RegisterComponents()
    {
        foreach (ICharacterComponent characterComponent in GetComponentsInChildren<ICharacterComponent>())
        {
            characterComponent.ParentCharacter = this;
        }
    }

    private void Awake()
    {
        RegisterComponents();
        Cursor.lockState = CursorLockMode.Locked;
    }
}
