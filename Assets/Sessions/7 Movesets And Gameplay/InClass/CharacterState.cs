using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    //Estas variables iniciales deberian ser parte de un sistema de progresion
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegen = 10f;
    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private float stamina;
    private float currentHealth;
    private Transform lockedTarget;

    private void Awake()
    {
        stamina = maxStamina;
        currentHealth = maxHealth;
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

    public bool UpdateHealth(float healthDelta)
    {
        if (currentHealth >= healthDelta)
        {
            currentHealth += healthDelta;
            return true;
        }
        //Morir
        Debug.Log($"Character ({gameObject.name} is dead");
        return false;
    }

    public float Stamina => stamina;

    public Transform LockedTarget
    {
        get => lockedTarget;
        set => lockedTarget = value;
    }

    public bool IsLocked => lockedTarget != null;
}
