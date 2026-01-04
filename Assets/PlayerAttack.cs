using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private float attackDuration = 0.5f;

    private Animator anim;
    private bool isAttacking;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        attackAction.action.Enable();
        attackAction.action.performed += OnAttack;
    }

    void OnDisable()
    {
        attackAction.action.performed -= OnAttack;
        attackAction.action.Disable();
    }

    private void OnAttack(InputAction.CallbackContext context)
{
    if (isAttacking) return;

    isAttacking = true;
    anim.SetBool("isAttacking", true);

    // Detect enemies in a small radius in front of the player
    Vector2 attackPos = (Vector2)transform.position + (Vector2.right * 1f); // 1 unit in front
    Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos, 0.5f); // radius 0.5

    foreach (var hit in hits)
    {
        CorruptedLegend legend = hit.GetComponent<CorruptedLegend>();
        if (legend != null)
        {
	    Debug.Log("Hit legend!");
            legend.TakeHit();
        }
    }

    Invoke(nameof(EndAttack), attackDuration);
}


    private void EndAttack()
    {
        anim.SetBool("isAttacking", false);
        isAttacking = false;
    }

	void OnDrawGizmosSelected()
	{
    	Gizmos.color = Color.red;
    	Vector2 attackPos = (Vector2)transform.position + (Vector2.right * 1f);
    	Gizmos.DrawWireSphere(attackPos, 0.5f);
	}
}
