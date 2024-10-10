using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponDamager : MonoBehaviour, IDamageSender
{
    [SerializeField] private float baseDamage = 10f;
    private float multiplier;

    private List<IDamageReceiver> hitReceivers = new List<IDamageReceiver>();
    
    public void Toggle(float multiplier)
    {
        hitReceivers.Clear();
        Collider col = GetComponent<Collider>();
        col.enabled = !col.enabled;
        this.multiplier = multiplier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageReceiver damageReceiver)
             && damageReceiver.Faction != Faction
             && !hitReceivers.Contains(damageReceiver))
        {
            hitReceivers.Add(damageReceiver);
            SendDamage(damageReceiver);
        }
    }

    public int Faction => 0; //Deberia de implementarse un sistema de personajes y jugador
    public void SendDamage(IDamageReceiver target)
    {
        DamagePayload payload = new DamagePayload
        {
            damage = baseDamage * multiplier,
            position = transform.position,
            severity = DamagePayload.DamageSeverity.Light
        };
        target.ReceiveDamage(this, payload);
    }
}
