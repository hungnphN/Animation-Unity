using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float heavyAttackCooldown = 2f;
    [SerializeField] private float heavyDashDistance = 5f;
    [SerializeField] private float heavyDashDuration = 0.2f;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;

    [Header("Heavy Attack Settings")]
    [SerializeField] private GameObject dashWindVFX;
    [SerializeField] private Transform vfxSpawnpoint;

    private Animator anim;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    private float cooldownTimer = Mathf.Infinity;
    private float heavyCooldownTimer = Mathf.Infinity;
    private bool isDashing = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        heavyCooldownTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement)
        {
            if (!playerMovement.IsGrounded())
                JumpAttack();
            else
                Attack();
        }

        if (Input.GetMouseButtonDown(1) && heavyCooldownTimer > heavyAttackCooldown && playerMovement && playerMovement.IsGrounded() && !isDashing)
        {
            StartCoroutine(HeavyAttack());
        }
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
        cooldownTimer = 0;

        fireBalls[FindFireball()].transform.position = firePoint.position;
        fireBalls[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private void JumpAttack()
    {
        anim.SetTrigger("jumpAttack");
        cooldownTimer = 0;

        fireBalls[FindFireball()].transform.position = firePoint.position;
        fireBalls[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private IEnumerator HeavyAttack()
    {
        isDashing = true;
        heavyCooldownTimer = 0;
        anim.SetTrigger("Flykick");

        // Spawn VFX
        if (dashWindVFX != null && vfxSpawnpoint != null)
        {
            GameObject vfx = Instantiate(dashWindVFX, vfxSpawnpoint.position, Quaternion.identity);
            vfx.transform.localScale = transform.localScale;
            Destroy(vfx, 1f);
        }

        // Dash movement
        float startTime = Time.time;
        float direction = Mathf.Sign(transform.localScale.x);

        while (Time.time < startTime + heavyDashDuration)
        {
            rb.velocity = new Vector2(direction * heavyDashDistance / heavyDashDuration, rb.velocity.y);
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isDashing = false;
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