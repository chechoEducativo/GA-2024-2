using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DamageController : MonoBehaviour, IDamageReceiver
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ReceiveDamage(IDamageSender perpetrator, DamagePayload payload)
    {
        bool isAlive = GetComponent<CharacterState>().UpdateHealth(payload.damage);
        Vector3 damageDirection = transform.InverseTransformPoint(payload.position).normalized;
        if (isAlive)
        {
            if (Mathf.Abs(damageDirection.x) >= Mathf.Abs(damageDirection.y))
            {
                anim.SetFloat("DamageX", damageDirection.x * (float)payload.severity);
                anim.SetFloat("DamageY", 0);
            }
            else
            {
                anim.SetFloat("DamageX", 0);
                anim.SetFloat("DamageY", damageDirection.z * (float)payload.severity);
            }
            anim.SetTrigger("Damaged");
        }
        else
        {
            anim.SetTrigger("Die");
        }
    }

    public int Faction => 0;
}
