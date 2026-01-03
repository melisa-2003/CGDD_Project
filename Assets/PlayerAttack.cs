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
        RaycastHit2D hit = Physics2D.Raycast(
        transform.position,
        transform.right,
        1f
    );

    if (hit.collider != null)


    Invoke(nameof(EndAttack), attackDuration);
}


    private void EndAttack()
    {
        anim.SetBool("isAttacking", false);
        isAttacking = false;
    }
}
