using UnityEngine;

public class DamageVolume : MonoBehaviour, IDamageSender
{
    [SerializeField] private float damageAmount;
    [SerializeField] private DamagePayload.DamageSeverity severity;
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageReceiver receiver))
        {
            SendDamage(receiver);
        }
    }

    public void SendDamage(IDamageReceiver target)
    {
        target.ReceiveDamage(this, new DamagePayload{damage = -damageAmount, severity = severity, position = transform.position});
    }

    public int Faction => 1;
}
