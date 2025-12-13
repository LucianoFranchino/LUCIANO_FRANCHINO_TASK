using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public static WeaponSystem Instance { get; private set; }

    [Header("Shooting Settings")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireRate = 0.5f;
    public Item staffWeapon;

    [SerializeField] private AudioClip shootSound;
    private float nextFireTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (CanShoot() && Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private bool CanShoot()
    {
        Item equippedItem = EquipmentManager.Instance.GetCurrentEquippedItem();

        if (equippedItem != null && equippedItem == staffWeapon)
        {
            return true;
        }

        return false;
    }

    private void Shoot()
    {
        GameObject projectile = ObjectPool.Instance.GetProjectile();
        projectile.transform.position = shootPoint.position;
        AudioManager.instance.PlayAudio(shootSound);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector2 direction = (mousePosition - shootPoint.position).normalized;

        Projectile proj = projectile.GetComponent<Projectile>();
        proj.Initialize(direction);
    }
}
