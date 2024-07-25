
using System;
using UnityEngine;

public class SimpleOscilator : MonoBehaviour
{
    [SerializeField] private float frequency;
    [SerializeField] private float amplitude;
    [SerializeField] private Vector3 axis;

    private Vector3 startPos;

    private void OnValidate()
    {
        startPos = transform.position;
    }

    private void Awake()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + axis.normalized * (Mathf.Sin(Time.time * frequency) * amplitude);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.7f);
        Gizmos.DrawSphere(startPos, 0.1f);        
        Gizmos.DrawLine(startPos - axis.normalized * amplitude * 0.5f, startPos + axis.normalized * amplitude * 0.5f);
    }
#endif
}
