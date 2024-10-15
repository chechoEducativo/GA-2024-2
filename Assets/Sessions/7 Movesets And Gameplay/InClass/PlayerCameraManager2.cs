using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager2 : MonoBehaviour
{
    [SerializeField] private Camera cam;

    public float GetFocusScore(Transform target)
    {
        Vector3 targetDir = (target.position - cam.transform.position).normalized;
        return Mathf.Clamp01(Vector3.Dot(targetDir, cam.transform.forward) * 0.5f + 0.5f);
    }
    
    public Camera Camera => cam;
}
