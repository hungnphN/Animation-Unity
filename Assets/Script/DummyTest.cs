using UnityEngine;

public class DummyTest : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator.Play("Idle");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Dummy took {damage} damage. Remaining HP: {currentHealth}");

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
        animator.SetTrigger("Die");
        Debug.Log("Dummy died.");
    }
}