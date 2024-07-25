using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis;

    [SerializeField] private float angularSpeed;

    void Update()
    {
        transform.RotateAround(transform.position, rotationAxis, angularSpeed * Time.deltaTime);
    }
}
