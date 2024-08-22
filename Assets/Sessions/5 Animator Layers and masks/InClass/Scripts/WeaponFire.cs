using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponFire : MonoBehaviour
{
    private Animator anim;
    public Animator Anim
    {
        get
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }

            return anim;
        }
    }

    public void Fire(InputAction.CallbackContext ctx)
    {
        bool state = ctx.ReadValueAsButton();
        Anim.SetBool("Firing", state);
    }
    
    public void OnShoot()
    {
        //TODO: Toda la logica de disparo
        //Spawnear proyectil
        //Lanzar
    }
}
