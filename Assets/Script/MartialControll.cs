using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MartialControll : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D Rib;
    private Animator anim;
    [Header("Movement")]
    private float moveSpeed = 5f;
    private float Jumpforce = 7f;
    private bool isGrounded = false;
    [Header("Combo Attack")]
    private int comboStep = 0;
    private float lastAttackTime;
    private float comboReset = 0.8f;
    [Header("Groundcheck")]
    public Transform groundcheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private void Start()
    {
        Rib = GetComponent<Rigidbody2D>();
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
        isGrounded = Physics2D.OverlapCircle(groundcheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("isGrounded", isGrounded);
    }
    private void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        Rib.velocity = new Vector2(move * moveSpeed, Rib.velocity.y);

        if (move != 0)
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);

        anim.SetBool("isRunning", move != 0);
    }
}
