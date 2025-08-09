using UnityEngine;

public class HeroKnightControl : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    private bool isGrounded = false;
    private bool isBlocking = false;
    private bool isRolling = false;

    [Header("Combo Attack")]
    private int comboStep = 0;
    private float lastAttackTime;
    public float comboResetTime = 0.8f;

    [Header("IFrame")]
    public float rollDuration = 0.5f;
    private float rollTimer = 0f;
    private bool isInvincible = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Attack Settings")]
    public Transform attackPoint;     
    public float attackRange = 0.5f;    
    public LayerMask enemyLayers;       
    public int attackDamage = 20;       

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleJump();
        HandleAttack();
        HandleBlock();
        HandleRoll();
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void HandleMovement()
    {
        if (isRolling || isBlocking) return;

        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (move != 0)
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);

        animator.SetBool("isRunning", move != 0);
    }

    private void HandleJump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded && !isRolling)
        {

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
    }
    private void HandleAttack()
    {
        if (isRolling || isBlocking) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastAttackTime > comboResetTime)
                comboStep = 0;

            comboStep++;
            comboStep = Mathf.Clamp(comboStep, 1, 3);

            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");

            animator.SetTrigger("Attack" + comboStep);

            lastAttackTime = Time.time;
        }
    }
    public void DealDamage()
    //{
    //    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f, LayerMask.GetMask("Enemy"));

    //    foreach (Collider2D enemy in hits)
    //    {
    //        BanditHealth bh = enemy.GetComponent<BanditHealth>();
    //        if (bh != null)
    //        {
    //            bh.TakeDamage(20); // Gây 20 damage
    //        }
    //    }

    //    Debug.Log("HeroKnight gây damage!");
    //}
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D hit in hits)
        {
            DummyTest dummy = hit.GetComponent<DummyTest>();
            if (dummy != null)
            {
                dummy.TakeDamage(attackDamage);
                Debug.Log("Gây damage lên DummyTest: " + attackDamage);
                continue;
            }
        }
        Debug.Log("HeroKnightDealDamage");
    }

    private void HandleBlock()
    {
        isBlocking = Input.GetMouseButtonDown(1);
        animator.SetBool("isBlocking", isBlocking);
    }

    private void HandleRoll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isRolling && isGrounded)
        {
            isRolling = true;
            isInvincible = true;
            rollTimer = rollDuration;

            animator.SetTrigger("Roll");
        }

        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0)
            {
                isRolling = false;
                isInvincible = false;
            }
        }

        animator.SetBool("isRolling", isRolling);
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

        animator.SetTrigger("Hurt");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        this.enabled = false;
        rb.velocity = Vector2.zero;
    }
}