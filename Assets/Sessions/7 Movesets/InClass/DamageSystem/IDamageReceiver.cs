using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageReceiver : IFactionMember
{
    void ReceiveDamage(IDamageSender perpetrator, DamagePayload payload);
}
