using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAim : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera aimingCamera;
    public void OnAim(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            aimingCamera?.gameObject.SetActive(true);
        }

        if (ctx.canceled)
        {
            aimingCamera?.gameObject.SetActive(false);
        }
    }
}
