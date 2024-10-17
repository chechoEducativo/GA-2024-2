using InClass;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class CombatController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Character character;
    [SerializeField] private WeaponDamager weaponDamager;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private float detectionRadius;

    public UnityEvent onLockTarget;
    public UnityEvent onUnlockTarget;

    private bool AttackActive()
    {
        return anim.GetFloat("ActiveAttack") > 0.5f;
    }
    
    public void LightAttack(InputAction.CallbackContext ctx)
    {
        if (!gameObject.scene.IsValid()) return;
        if (!ctx.ReadValueAsButton()) return;
        if (AttackActive()) return;
        if(!GetComponent<CharacterState>().UpdateStamina(-20)) return;
        anim.SetTrigger("Attack");
        anim.SetBool("HeavyAttack", false);
    }

    public void HeavyAttack(InputAction.CallbackContext ctx)
    {
        bool clicked = ctx.ReadValueAsButton();
        if(AttackActive()) return;
        CharacterState state = GetComponent<CharacterState>();
        if(state.Stamina < -40) return;
        if (clicked)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("HeavyAttack", true);
            anim.SetFloat("Charging", 1);
        }
        else
        {
            anim.SetFloat("Charging", 0);
            //state.UpdateStamina(-40);
        }
    }
    
    public void Lock(InputAction.CallbackContext ctx)
    {
        if (!gameObject.scene.IsValid()) return;
        if (!ctx.performed) return;
        if (character.State.IsLocked)
        {
            character.State.LockedTarget = null;
            onUnlockTarget?.Invoke();
            return;
        }
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);
        if (detectedColliders.Length == 0) return;
        int bestFocusedTarget = 0;
        var cameraManager = character.Player.GetComponent<PlayerCameraManager2>();
        for (int i = 0; i < detectedColliders.Length; i++)
        {
            float focusScore = cameraManager.GetFocusScore(detectedColliders[i].transform);
            float currentBestScore = cameraManager.GetFocusScore(detectedColliders[bestFocusedTarget].transform);
            if (1 - focusScore < 1 - currentBestScore) bestFocusedTarget = i;
        }
        character.State.LockedTarget = detectedColliders[bestFocusedTarget].transform;
        onLockTarget?.Invoke();
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ToggleDamageDetector(float motionValue)
    {
        weaponDamager.Toggle(motionValue);
    }
}
