using UnityEngine;
using System.Collections;

public class BanditHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Components")]
    public Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"Bandit Spawned with HP: {currentHealth}");
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"Bandit HP: {currentHealth}");

        if (currentHealth > 0)
        {
            animator.SetTrigger("Hurt");
        }
        else
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;
        Debug.Log("Bandit Died!");
        animator.SetTrigger("Die");
        StartCoroutine(ReviveAfterDelay(3f));
    }
    public void OnDieAnimationEnd()
    {
        StartCoroutine(ReviveAfterDelay(3f));
    }

    private IEnumerator ReviveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isDead = false;
        currentHealth = maxHealth / 2;
        animator.ResetTrigger("Die");
        animator.SetTrigger("Revive");
    }
    private void Revive()
    {
        isDead = false;
        currentHealth = maxHealth / 2; 
        Debug.Log($"Bandit Revived with HP: {currentHealth}");
        animator.SetTrigger("Revive");
    }
}