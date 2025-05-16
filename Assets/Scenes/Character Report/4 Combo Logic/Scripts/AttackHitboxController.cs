using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxController : MonoBehaviour
{
    [SerializeField] private GameObject[] hitboxes;

    public void ToggleHitboxes(int attackId)
    {
        for(int hitboxId = 0; hitboxId < hitboxes.Length; hitboxId++)
        {
            GameObject hitbox = hitboxes[hitboxId];
            hitbox.SetActive(!hitbox.activeSelf);
        }
    }

    public void CleanupHitboxes()
    {
        foreach (GameObject colliders in hitboxes)
        {
            colliders.SetActive(false);
        }
    }
}
