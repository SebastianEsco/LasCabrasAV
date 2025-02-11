using UnityEngine;
using System;

namespace Celeste_Garcia
{
    [Serializable]
    public struct FloatDampener
    {
        [SerializeField] private float smoothTime;
        private float currentVelocity;
        public float TargetValue {get; set;}
        public float CurrentValue { get; private set;}

        public void Update()
        {
            CurrentValue = Mathf.SmoothDamp(CurrentValue, TargetValue, ref currentVelocity, smoothTime);
        }
    }
}