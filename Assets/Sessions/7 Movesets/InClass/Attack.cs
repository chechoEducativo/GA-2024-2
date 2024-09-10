using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Attack : MonoBehaviour
{
    private Animator anim;

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

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
