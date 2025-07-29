using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("speed", speed);
        animator.SetBool("isMoving", speed > 0.1f);
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("ATK");
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            animator.SetTrigger("Dizzy");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetBool("IsDead", true);
        }
    }
}
