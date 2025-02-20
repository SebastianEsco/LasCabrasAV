using System.Collections;
using UnityEngine;

namespace Celeste_Garcia
{
    [DefaultExecutionOrder(-1)]
    public class Character : MonoBehaviour
    {
        [SerializeField] Transform lockTarget;

        public Transform LockTarget
        {
            get => lockTarget; set => lockTarget = value;
        }

        void RegisterComponents()
        {
            foreach (ICharacterComponent characterComponent in GetComponentsInChildren<ICharacterComponent>())
            {
                characterComponent.parentCharacter = this;
            }
        }

        private void Awake()
        {
            RegisterComponents();
        }
    }
}