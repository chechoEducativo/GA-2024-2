using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct TransformSource
{
    [Range(0,1)]public float weight;
    public Transform source;
    public bool copyPosition;
    public bool copyRotation;
    public bool copyScale;
}

public class TransformCopier : MonoBehaviour
{
    [SerializeField] private TransformSource[] sources;

    private Vector3 startPos;
    private Vector3 startScale;
    private Quaternion startRotation;
    
    private void Awake()
    {
        startPos = transform.position;
        startScale = transform.localScale;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 solvedPosition = Vector3.zero;
        Vector3 solvedScale = Vector3.one;
        Quaternion solvedRotation = Quaternion.identity;

        float weightSum = sources.Sum(x => x.weight);
        foreach (TransformSource transformSource in sources)
        {
            float normalizedWeight = transformSource.weight / weightSum + 0.001f;
            
            solvedPosition += transformSource.source.position *
                             (normalizedWeight * (transformSource.copyPosition ? 1 : 0));
            
            solvedScale += transformSource.source.localScale *
                          (normalizedWeight * (transformSource.copyScale ? 1 : 0));

            solvedRotation *= Quaternion.Slerp(Quaternion.identity, transformSource.source.rotation,
                (normalizedWeight * (transformSource.copyRotation ? 1 : 0)));
        }

        transform.position = Vector3.Lerp(startPos, solvedPosition, Mathf.Clamp01(weightSum));
        transform.localScale = Vector3.Lerp(startScale, solvedScale, Mathf.Clamp01(weightSum));
        transform.rotation =  Quaternion.Slerp(startRotation, solvedRotation, Mathf.Clamp01(weightSum));;
    }
}
