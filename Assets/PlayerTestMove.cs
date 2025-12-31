using UnityEngine;

public class PlayerHorizontalMove : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        // 只控制 X，Y 交给重力
        rb.linearVelocity = new Vector2(h * speed, rb.linearVelocity.y);
    }
}

//Player 可以用 AD / ←→ 移动