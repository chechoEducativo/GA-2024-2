using System;
using UnityEngine;

[Serializable]
public struct Vector2Dampener
{
    private Vector2 value;
    private Vector2 target;
    private Vector2 velocity;
    [SerializeField]private float time;
    [SerializeField] private float clampMagnitude;
    private bool clamp;
    
    public Vector2Dampener(float time)
    {
        value = Vector2.zero;
        target = Vector2.zero;
        velocity = Vector2.zero;
        clampMagnitude = 1;
        clamp = true;
        this.time = time;
    }

    public void Update()
    {
        value = Vector2.SmoothDamp(value, clamp ? Vector2.ClampMagnitude(target, clampMagnitude) : target, ref velocity, time);
    }

    public Vector2 Value => value;
    public Vector2 Target
    {
        set => target = value;
    }
    public bool Clamp
    {
        set => clamp = value;
    }

    public float ClampMagnitude => clampMagnitude;
}
