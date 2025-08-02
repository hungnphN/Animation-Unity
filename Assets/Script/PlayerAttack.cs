using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls; 
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement)
            Attack();   
        cooldownTimer += Time.deltaTime;
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement)
        {
            if (!playerMovement.IsGrounded())
                JumpAttack();
            else
                Attack();
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
    private int FindFireball()
    {
        for(int i =0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].activeInHierarchy)
                return i;
        }
        return 0;
    }    
}
