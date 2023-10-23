using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _smoothSpeed;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Transform _rotateTransform;
        private Transform _target;
        private Transform _center;
        private int _distance = 30;


        public void Construct(Transform target, Transform center)
        {
            _target = target;
            _center = center;
        }

        public void FixedUpdate()
        {
            Vector3 desiredPosition = _target.position + -transform.forward * _distance;
            transform.position = desiredPosition;
            
            var gravityUp = (transform.position - _center.position).normalized;
            var bodyUp = -transform.forward;
            
            Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                targetRotation, 1);



        }
    }
}