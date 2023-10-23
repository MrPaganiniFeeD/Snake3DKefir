using Garvity;
using UnityEngine;

namespace Serivces
{
    public interface IArtificialGravityService : IUpdatableService
    {
        void SetCenterOfGravity(Transform centerGravity);
        void AddAttracted(IArtificiallyAttracted attracted);
        
    }
}