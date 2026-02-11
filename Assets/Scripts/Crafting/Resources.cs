using UnityEngine;

public class Resources : MonoBehaviour
{
    [Header("Resource Settings")]
    public ResourceType resourceType;
    public Item resourceDrop;
    public int minDropAmount = 3;
    public int maxDropAmount = 6;

    [Header("Health")]
    public int maxHits = 4; // Cantidad de golpes necesarios
    private int currentHits = 0;

    [Header("Audio")]
    public AudioClip hitSound; // Sonido al golpear
    public AudioClip breakSound; // Sonido al romper

    [Header("Visual")]
    public Sprite destroyedSprite;

    private bool isDestroyed = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public bool CanHarvest(ToolType tool)
    {
        if (isDestroyed) return false;

        switch (resourceType)
        {
            case ResourceType.Tree:
                return tool == ToolType.Axe;
            case ResourceType.Rock:
                return tool == ToolType.Pickaxe;
            default:
                return false;
        }
    }

    public void Hit()
    {
        if (isDestroyed) return;

        currentHits++;

        // Reproducir sonido de golpe
        if (hitSound != null)
        {
            AudioManager.instance.PlayAudio(hitSound);
        }

        // Efecto visual de golpe
        StartCoroutine(HitFlash());

        // Verificar si se rompió
        if (currentHits >= maxHits)
        {
            Break();
        }
    }

    private void Break()
    {
        isDestroyed = true;

        // Reproducir sonido de rotura
        if (breakSound != null)
        {
            AudioManager.instance.PlayAudio(breakSound);
        }

        // Calcular cantidad random de drops
        int dropAmount = Random.Range(minDropAmount, maxDropAmount + 1);

        // Crear los items en el mundo
        for (int i = 0; i < dropAmount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector3 dropPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            GameObject drop = new GameObject(resourceDrop.objectName + "_Drop");
            drop.transform.position = dropPosition;

            SpriteRenderer sr = drop.AddComponent<SpriteRenderer>();
            sr.sprite = resourceDrop.image;
            sr.sortingOrder = 6;

            CircleCollider2D col = drop.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.3f;

            ItemPickUp pickup = drop.AddComponent<ItemPickUp>();
            pickup.item = resourceDrop;
        }

        Debug.Log($"¡{resourceType} destruido! Soltó {dropAmount}x {resourceDrop.objectName}");

        // Cambiar sprite o destruir
        if (destroyedSprite != null)
        {
            spriteRenderer.sprite = destroyedSprite;
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }
        else
        {
            Destroy(gameObject, 0.1f);
        }
    }

    // Efecto visual de flash blanco al golpear
    private System.Collections.IEnumerator HitFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }
}

public enum ResourceType
{
    Tree,
    Rock
}
