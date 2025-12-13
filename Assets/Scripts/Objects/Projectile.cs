using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;

    private Vector2 direction;
    private float timer;

    public void Initialize(Vector2 shootDirection)
    {
        direction = shootDirection.normalized;
        timer = lifetime;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1f);
            }
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ObjectPool.Instance.ReturnProjectile(gameObject);
    }
}
