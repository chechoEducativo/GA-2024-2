using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponDamager : MonoBehaviour
{
    private float multiplier;
    public void Toggle(float multiplier)
    {
        Collider col = GetComponent<Collider>();
        col.enabled = !col.enabled;
        this.multiplier = multiplier;
    }
}
