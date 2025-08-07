using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditControl : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 14f;
    public bool isFacingRight = true;

    [Header("State")]
    private bool isGrounded;
    private bool isBlocking;
    private bool isDead;

    void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleJump();
        HandleAttack();
        HandleBlock();
        HandleFlip();
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (!isBlocking)
        {
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        }

        animator.SetBool("isRunning", Mathf.Abs(move) > 0.1f);
    }

    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded && !isBlocking)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
        if (isGrounded)
        {
            animator.ResetTrigger("Jump");
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.J) && isGrounded && !isBlocking)
        {
            animator.SetTrigger("Attack");
        }
    }

    private void HandleBlock()
    {
        if (Input.GetKey(KeyCode.K))
        {
            isBlocking = true;
            animator.SetBool("Block", true);
        }
        else
        {
            isBlocking = false;
            animator.SetBool("Block", false);
        }
    }

    private void HandleFlip()
    {
        float move = Input.GetAxisRaw("Horizontal");
        if (move > 0 && !isFacingRight)
            Flip();
        else if (move < 0 && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Hurt()
    {
        animator.SetTrigger("Hurt");
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        rb.velocity = Vector2.zero;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Grounded", isGrounded);
    }
}
