using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange = 5f; 
    [SerializeField] private float attackRange = 1.5f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Attack")]
    public float damage = 1f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Health")]
    public float health = 1f;
    private bool isDead = false;

    [Header("Animation")]
    public string horizontalParameter = "Horizontal";

    private PlayerHealth playerHealth;
    private Transform player;
    private Animator animator;
    private float lastAttackTime;

    private enum State
    {
        Idle,
        Chase,
        Attack
    }

    private State currentState = State.Idle;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer <= detectionRange)
                {
                    currentState = State.Chase;
                }
                UpdateAnimation(0f);
                break;

            case State.Chase:
                if (distanceToPlayer <= attackRange)
                {
                    currentState = State.Attack;
                }
                else if (distanceToPlayer > detectionRange)
                {
                    currentState = State.Idle;
                }
                else
                {
                    ChasePlayer();
                }
                break;

            case State.Attack:
                if (distanceToPlayer > attackRange)
                {
                    currentState = State.Chase;
                }
                else
                {
                    AttackPlayer();
                }
                break;

        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        UpdateAnimation(direction.x);
    }

    private void AttackPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        UpdateAnimation(direction.x);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (playerHealth != null && playerHealth.IsAlive())
            {
                playerHealth.TakeDamage(damage);
                lastAttackTime = Time.time;
            }
        }
    }

    private void UpdateAnimation(float horizontal)
    {
        if (animator != null)
        {
            animator.SetFloat(horizontalParameter, horizontal);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        if (animator != null)
        {
            animator.Play("Enemy_Die");
        }
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        // Rango de detección (amarillo)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Rango de ataque (rojo)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
