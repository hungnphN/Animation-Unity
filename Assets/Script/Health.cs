using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private bool isDead = false;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour[] componentsToDisableOnDeath;

    private void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();
        Debug.Log("Health initialized: " + currentHealth);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log($"[DAMAGE] {gameObject.name} took {damage} damage. Current HP: {currentHealth}");

        if (currentHealth > 0f)
        {
            animator.SetTrigger("Hurt");
        }
        else
        {
            Debug.LogWarning($"[DEATH] {gameObject.name} has died.");
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        foreach (var component in componentsToDisableOnDeath)
        {
            if (component != null)
                component.enabled = false;
        }
        Destroy(gameObject, 1.5f);
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log($"[HEAL] {gameObject.name} healed {amount} HP. Current HP: {currentHealth}");
    }


    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}