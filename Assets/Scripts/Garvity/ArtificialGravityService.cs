using System.Collections.Generic;
using Serivces;
using UnityEngine;

namespace Garvity
{
    public class ArtificialGravityService : IArtificialGravityService
    {
        private List<IArtificiallyAttracted> _artificiallyAttracted = new List<IArtificiallyAttracted>();


        private Transform _centerGravity;

        private float _gravity = -10;
        private int _speedRotation = 50;
        
        public void SetCenterOfGravity(Transform centerGravity) => 
            _centerGravity = centerGravity;

        public void AddAttracted(IArtificiallyAttracted attracted) => 
            _artificiallyAttracted.Add(attracted);

        public void Update() => 
            UpdateArtificiallyAttracted();

        private void UpdateArtificiallyAttracted()
        {
            foreach (var attracted in _artificiallyAttracted)
            {
                var attractedTransform = attracted.Transform;
                var attractedRigidbody = attracted.Rigidbody;
                
                var gravityUp = (attractedTransform.position - _centerGravity.position).normalized;
                var bodyUp = attractedTransform.up;
                
                attractedRigidbody.AddForce(_gravity * bodyUp);
                
                Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * attractedTransform.rotation;
                attractedTransform.rotation = Quaternion.Slerp(attractedTransform.rotation, 
                    targetRotation, 
                    _speedRotation * Time.deltaTime);
                
                
                RaycastHit hit;
                if (Physics.Raycast(attractedTransform.position, -1 * attractedTransform.up, out hit, 10, LayerMask.NameToLayer("Surface")))
                {
                    attractedTransform.position = hit.point;
                }
            }
        }
    }
}