using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeDash : MonoBehaviour
{
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private bool isStriking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !playerMovement.IsGrounded() && !isStriking)
        {
            Strike();
        }
    }

    private void Strike()
    {
        isStriking = true;
        animator.SetTrigger("Strike");
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        rb.velocity = new Vector2(direction * dashForce, rb.velocity.y);
        Invoke(nameof(ResetStrike), 0.5f);
    }

    private void ResetStrike()
    {
        isStriking = false;
    }
}
