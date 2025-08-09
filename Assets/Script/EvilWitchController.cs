using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilWitchController : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D rb;
    private Animator anim;
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    private bool isGrounded = false;
    private bool isAttacking = false;
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float lightAttackRange = 0.5f;
    public float heavyAttackRange = 0.8f;
    public LayerMask enemyLayers;
    public int lightAttackDamage = 15;
    public int heavyAttackDamage = 30;

    [Header("Attack Cooldown")]
    public float lightAttackCooldown = 0.3f;
    public float heavyAttackCooldown = 0.8f;
    private float lastLightAttackTime;
    private float lastHeavyAttackTime;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleJump();
        HandleAttack();
    }
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void HandleMovement()
    {
        if (isAttacking) return;

        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (move != 0)
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);

        anim.SetBool("isRunning", move != 0);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isAttacking)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }
    }

    private void HandleAttack()
    {
        if (isAttacking) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastLightAttackTime >= lightAttackCooldown)
            {
                PerformLightAttack();
                lastLightAttackTime = Time.time;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (Time.time - lastHeavyAttackTime >= heavyAttackCooldown)
            {
                PerformHeavyAttack();
                lastHeavyAttackTime = Time.time;
            }
        }
    }
    private void PerformLightAttack()
    {
        isAttacking = true;
        anim.SetTrigger("LightAttack");

        Debug.Log("Light Attack performed!");
    }

    private void PerformHeavyAttack()
    {
        isAttacking = true;
        anim.SetTrigger("HeavyAttack");

        Debug.Log("Heavy Attack performed!");
    }

    public void OnAttackComplete()
    {
        Debug.Log("Attack animation completed, resetting isAttacking.");
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {        anim.SetTrigger("Hurt");

        Debug.Log($"Hero took {damage} damage!");
    }

    public void Die()
    {
        anim.SetTrigger("Die");
        this.enabled = false;
        rb.velocity = Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, lightAttackRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, heavyAttackRange);
        }
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

