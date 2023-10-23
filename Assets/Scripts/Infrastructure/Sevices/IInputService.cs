using System;
using UnityEngine;

namespace Serivces
{
    public interface IInputService : IUpdatableService
    {
        public event Action<float> ChangeAxis; 
        public float AxisX { get; } 
    }
    
    public class InputService : IInputService
    {
        public event Action<float> ChangeAxis; 
        
        public float AxisX { get; private set; } 
        
        public void Update()
        {
            AxisX = SimpleInput.GetAxis("Horizontal");
            ChangeAxis?.Invoke(AxisX);
        }
    }
}