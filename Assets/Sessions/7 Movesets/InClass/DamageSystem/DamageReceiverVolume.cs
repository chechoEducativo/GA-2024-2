using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageReceiverVolume : MonoBehaviour, IDamageReceiver
{
    public int Faction => 1;
    public void ReceiveDamage(IDamageSender perpetrator, DamagePayload payload)
    {
        Debug.Log("Ay!");
        Debug.DrawLine(transform.position, payload.position, Color.blue, 1.0f);
    }
}
