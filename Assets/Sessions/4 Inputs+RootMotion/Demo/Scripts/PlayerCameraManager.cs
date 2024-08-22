using System;
using System.Linq;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject[] cameraRigs;
    [SerializeField] private int defaultSoloIndex;

    [SerializeField] private GameObject previousCameraRig;
    private GameObject currentCameraRig;
    
    public Camera Cam => cam;

    private void OnValidate()
    {
        if (defaultSoloIndex > cameraRigs.Length - 1)
        {
            defaultSoloIndex = cameraRigs.Length - 1;
        }
        if (defaultSoloIndex < 0 || cameraRigs == null)
        {
            defaultSoloIndex = 0;
        }
    }

    private void Start()
    {
        print(cameraRigs);
        SetSolo(cameraRigs[defaultSoloIndex]);
    }

    public void ReturnToPreviousSoloSolo()
    {
        SetSolo(previousCameraRig);
    }

    public void SetSolo(GameObject cameraRig)
    {
        if (!cameraRigs.Contains(cameraRig) || currentCameraRig == cameraRig || cameraRig == null) return;
        previousCameraRig = currentCameraRig;
        if (previousCameraRig != null)
        {
            cameraRig.transform.rotation = previousCameraRig.transform.rotation;
        }

        currentCameraRig = cameraRig;
        foreach (GameObject rig in cameraRigs)
        {
            rig.SetActive(rig == cameraRig);
        }
    }
}
