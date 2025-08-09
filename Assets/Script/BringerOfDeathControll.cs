using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathControll : MonoBehaviour
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
    private bool isCasting;
    private bool isDead;

    [Header("Cast Spell Settings")]
    public float castSpellCooldown = 1.5f;
    private float lastCastTime;

    void Update()
    {
        if (isDead) return;

        CheckGround();
        HandleMovement();
        HandleJump();
        HandleAttack();
        HandleFlip();
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;

        animator.SetBool("Grounded", isGrounded);

        if (isGrounded && !wasGrounded)
        {
            animator.ResetTrigger("Jump");
        }
        wasGrounded = isGrounded;
    }

    private void HandleMovement()
    {
        if (isCasting) return;

        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        animator.SetBool("isRunning", Mathf.Abs(move) > 0.1f);
    }

    private void HandleJump()
    {
        if (isCasting) return;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            animator.SetTrigger("Jump");
        }
    }

    private void HandleAttack()
    {
        if (isCasting) return;

        if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            animator.SetTrigger("Attack");
        }

        if (Input.GetMouseButtonDown(1) && isGrounded && Time.time - lastCastTime >= castSpellCooldown)
        {
            StartCoroutine(CastSpell());
            lastCastTime = Time.time;
        }
    }

    private IEnumerator CastSpell()
    {
        isCasting = true;
        animator.SetTrigger("CastSpell");
        yield return new WaitForSeconds(1f);
        isCasting = false;
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

    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
    }
}