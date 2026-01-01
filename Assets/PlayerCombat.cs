using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Animator anim;
    bool isAttacking;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;

        // CONTROL ANIMATOR PARAMETER
        anim.SetBool("isAttacking", true);

        // Reset after animation finishes
        Invoke(nameof(EndAttack), 0.5f);
    }

    void EndAttack()
    {
        anim.SetBool("isAttacking", false);
        isAttacking = false;
    }
}
