using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageSender : IFactionMember
{
    void SendDamage(IDamageReceiver target);
}
