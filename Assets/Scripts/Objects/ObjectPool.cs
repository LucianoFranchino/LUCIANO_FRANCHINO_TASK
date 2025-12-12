using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [Header("Pool Settings")]
    public GameObject projectilePrefab;
    public int poolSize = 20;

    private Queue<GameObject> projectilePool;

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

        InitializePool();
    }

    private void InitializePool()
    {
        projectilePool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject proj = Instantiate(projectilePrefab);
            proj.SetActive(false);
            proj.transform.SetParent(transform);
            projectilePool.Enqueue(proj);
        }
    }

    public GameObject GetProjectile()
    {
        if (projectilePool.Count > 0)
        {
            GameObject proj = projectilePool.Dequeue();
            proj.SetActive(true);
            return proj;
        }
        else
        {
            GameObject proj = Instantiate(projectilePrefab);
            return proj;
        }
    }

    public void ReturnProjectile(GameObject proj)
    {
        proj.SetActive(false);
        proj.transform.SetParent(transform);
        projectilePool.Enqueue(proj);
    }
}
