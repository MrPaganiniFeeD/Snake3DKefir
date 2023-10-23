using System;
using UnityEngine;

public class Eat : MonoBehaviour
{
    public event Action<Eat> Collected;

    public void Collect()
    {
        Collected?.Invoke(this);
        Destroy(gameObject);
    }
    
}