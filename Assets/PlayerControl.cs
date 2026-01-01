using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private InputActionReference moveActionToUse;
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Animator anim;

    private float moveX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        moveActionToUse.action.Enable();
    }

    void OnDisable()
    {
        moveActionToUse.action.Disable();
    }

    void Update()
    {
        // Read Vector2 from Input System
        Vector2 moveInput = moveActionToUse.action.ReadValue<Vector2>();

        // Only care about left/right
        moveX = moveInput.x;

        // Animator
        anim.SetBool("isRunning", Mathf.Abs(moveX) > 0.1f);

        // Flip sprite
        if (moveX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1);
        }
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        // Move using physics (recommended for Rigidbody2D)
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
    }
}
