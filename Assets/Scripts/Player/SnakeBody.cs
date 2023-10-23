using Garvity;
using UnityEngine;

namespace Player
{
    public class SnakeBody : MonoBehaviour, IArtificiallyAttracted
    {
        public Transform Transform { get; private set; }
        public Rigidbody Rigidbody { get; private set;}

        private void Awake()
        {
            Transform = GetComponent<Transform>();
            Rigidbody = GetComponent<Rigidbody>();
        }
    }
}