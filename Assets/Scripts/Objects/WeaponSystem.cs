using Mono.Cecil;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public static WeaponSystem Instance { get; private set; }

    [Header("Shooting Settings")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireRate = 0.5f;
    public Item staffWeapon;

    [Header("Harvesting Settings")]
    public float harvestRange = 1.5f; // Rango para recolectar
    public LayerMask resourceLayer; // Layer de los recursos

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
        Item equippedItem = EquipmentManager.Instance.GetCurrentEquippedItem();

        if (equippedItem == null) return;

        // Sistema de disparo (para el bastón)
        if (equippedItem == staffWeapon && Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        // Sistema de recolección (para hacha y pico)
        else if (equippedItem.toolType == ToolType.Axe || equippedItem.toolType == ToolType.Pickaxe)
        {
            if (Input.GetMouseButtonDown(0)) // Click izquierdo
            {
                TryHarvest(equippedItem.toolType);
            }
        }
    }

    private void TryHarvest(ToolType tool)
    {
        // Buscar recursos cercanos
        Collider2D[] nearbyResources = Physics2D.OverlapCircleAll(transform.position, harvestRange, resourceLayer);

        Resources closestResource = null;
        float closestDistance = float.MaxValue;

        // Encontrar el recurso más cercano
        foreach (Collider2D col in nearbyResources)
        {
            Resources resource = col.GetComponent<Resources>();
            if (resource != null && resource.CanHarvest(tool))
            {
                float distance = Vector2.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestResource = resource;
                }
            }
        }

        // Golpear el recurso más cercano
        if (closestResource != null)
        {
            closestResource.Hit();// Cambiar de Harvest() a Hit()
            Debug.Log($"Golpeando {closestResource.resourceType} con {tool}");
        }
        else
        {
            Debug.Log("No hay recursos cerca o herramienta incorrecta");
        }
    }

    //private bool CanShoot()
    //{
    //    Item equippedItem = EquipmentManager.Instance.GetCurrentEquippedItem();

    //    if (equippedItem != null && equippedItem == staffWeapon)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, harvestRange);
    }
}
