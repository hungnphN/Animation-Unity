using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrakeMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Strike / Roll")]
    [SerializeField] private float rollForce = 8f;
    [SerializeField] private float rollDuration = 0.4f;
    [SerializeField] private float rollCooldown = 1f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;

    private Rigidbody2D rb;
    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;
    private bool isGrounded;
    private bool isRolling = false;
    private float rollTimer = 0f;
    private float rollCooldownTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateGroundCheck();
        HandleMovement();
        HandleJump();
        HandleAttack();
        HandleRoll();
        UpdateAnimation();
    }

    private void UpdateGroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void HandleMovement()
    {
        if (isRolling) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = rb.velocity;
        velocity.x = horizontalInput * moveSpeed;
        rb.velocity = velocity;

        if (horizontalInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1f, 1f);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isRolling)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("jump");
        }
    }

    private void HandleAttack()
    {
        cooldownTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && cooldownTimer >= attackCooldown && !isRolling)
        {
            cooldownTimer = 0;
            if (!isGrounded)
            {
                anim.SetTrigger("jumpAttack");
            }
            else
            {
                anim.SetTrigger("attack");
            }

            GameObject fb = fireBalls[FindFireball()];
            fb.transform.position = firePoint.position;
            fb.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }

    private void HandleRoll()
    {
        rollCooldownTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && rollCooldownTimer >= rollCooldown && isGrounded && !isRolling)
        {
            isRolling = true;
            rollTimer = 0f;
            rollCooldownTimer = 0f;

            float direction = transform.localScale.x;
            rb.velocity = new Vector2(direction * rollForce, rb.velocity.y);
            anim.SetTrigger("roll");
            // Tạm thời bật "invincible" tại đây nếu cần
        }

        if (isRolling)
        {
            rollTimer += Time.deltaTime;
            if (rollTimer >= rollDuration)
            {
                isRolling = false;
                // Tắt invincible nếu có
            }
        }
    }

    private void UpdateAnimation()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isRolling", isRolling);
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("isFalling", rb.velocity.y < 0 && !isGrounded);
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}

