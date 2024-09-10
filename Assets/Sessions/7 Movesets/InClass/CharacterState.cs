using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    //Estas variables iniciales deberian ser parte de un sistema de progresion
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegen = 10f;

    [SerializeField] private float stamina;

    private void Awake()
    {
        stamina = maxStamina;
    }

    private void Update()
    {
        stamina += Time.deltaTime * staminaRegen;
        stamina = Mathf.Min(stamina, maxStamina);
    }

    public bool UpdateStamina(float staminaDelta)
    {
        if (stamina >= Mathf.Abs(staminaDelta))
        {
            stamina += staminaDelta;
            return true;
        }
        
        return false;
    }

    public float Stamina => stamina;
}
