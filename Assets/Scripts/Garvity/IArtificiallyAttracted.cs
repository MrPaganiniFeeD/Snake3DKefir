using UnityEngine;

namespace Garvity
{
    public interface IArtificiallyAttracted
    {
        Transform Transform { get; }
        Rigidbody Rigidbody { get; }
    }
}