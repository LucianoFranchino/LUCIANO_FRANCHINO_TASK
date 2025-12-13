using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 5f;
    public float currentHealth;

    [Header("UI (Solo para jugador)")]
    public Slider healthSlider;

    [Header("Death Settings")]
    public float restartDelay = 2f;

    private Animator animator;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        isDead = true;
        if (CompareTag("Player"))
        {
            StartCoroutine(PlayerDeathSequence());
        }
    }

    private IEnumerator PlayerDeathSequence()
    {
        if (animator != null)
        {
            animator.Play("Die");
        }
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsAlive()
    {
        return currentHealth > 0f && !isDead;
    }
}
