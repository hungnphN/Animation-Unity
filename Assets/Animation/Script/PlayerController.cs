using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    private bool isGrounded = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = rb.velocity;

        // Di chuyển trái/phải
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Flip sprite trái/phải
        if (moveX != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1);

        // Nhảy
        {
            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger("Jump");
                isGrounded = false;
            }
        }

        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }

        // Crouch
        animator.SetBool("Iscrouching", Input.GetKey(KeyCode.S));

        // Animation di chuyển
        animator.SetFloat("speed", Mathf.Abs(moveX));
        animator.SetBool("isMoving", Mathf.Abs(moveX) > 0.1f);

        // Tấn công
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("ATK");
        }

        // Dizzy
        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetTrigger("Dizzy");
        }

        // Chết
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetBool("IsDead", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}