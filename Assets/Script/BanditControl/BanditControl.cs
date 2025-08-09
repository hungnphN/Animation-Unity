
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
    public float jumpForce = 7f;
    private bool isFacingRight = true;

    [Header("Ground Check Settings")]
    public float groundCheckDistance = 0.1f;

    [Header("State")]
    private bool isGrounded;
    private bool wasGrounded;
    private bool isBlocking;
    private bool isDead;

    void Update()
    {
        if (isDead) return;

        CheckGround();
        HandleMovement();
        HandleJump();
        HandleAttack();
        HandleBlock();
        HandleFlip();
    }

    private void CheckGround()
    {
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
            isGrounded = hit.collider != null;

            animator.SetBool("Grounded", isGrounded);

            // Vừa tiếp đất
            if (isGrounded && !wasGrounded)
            {
                // Reset trạng thái jump để trở về Idle/Run
                animator.ResetTrigger("Jump");
            }
            wasGrounded = isGrounded;
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
        if (Input.GetButtonDown("Jump") && isGrounded && !isBlocking)
        {
          rb.velocity = new Vector2(rb.velocity.x, jumpForce);
          isGrounded = false; 
          animator.SetTrigger("Jump");
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && isGrounded && !isBlocking)
        {
            animator.SetTrigger("Attack");
        }
    }

    private void HandleBlock()
    {
        if (Input.GetMouseButton(1))
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
        if (move > 0 && !isFacingRight) Flip();
        else if (move < 0 && isFacingRight) Flip();
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

    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
    }
}
