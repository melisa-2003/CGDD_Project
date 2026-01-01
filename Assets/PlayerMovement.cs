using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        // CONTROL ANIMATOR PARAMETER
        anim.SetBool("isRunning", Mathf.Abs(moveX) > 0.1f);

        // Flip sprite
        if (moveX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1);
        }
    }
}
