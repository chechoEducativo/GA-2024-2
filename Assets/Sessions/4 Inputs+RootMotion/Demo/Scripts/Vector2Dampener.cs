using System;
using UnityEngine;

[Serializable]
public struct Vector2Dampener
{
    private Vector2 value;
    private Vector2 target;
    private Vector2 velocity;
    [SerializeField]private float time;

    public Vector2Dampener(float time)
    {
        value = Vector2.zero;
        target = Vector2.zero;
        velocity = Vector2.zero;
        this.time = time;
    }

    public void Update()
    {
        value = Vector2.SmoothDamp(value, target, ref velocity, time);
    }

    public Vector2 Value => value;
    public Vector2 Target
    {
        set => target = value;
    }
}
