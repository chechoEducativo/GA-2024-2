using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamagePayload
{
    public enum DamageSeverity
    {
        Light = 1,
        Strong = 2,
        Critical = 3
    }
    
    public float damage;
    public Vector3 position;
    public DamageSeverity severity;
#pragma warning "TODO: implement poise system"
}
