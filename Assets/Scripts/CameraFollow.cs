using System.Net.NetworkInformation;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    private Vector3 offset;
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if(offset == Vector3.zero)
        {
            offset = new Vector3(0f,0f,-10f);
        }
    }

    private void LateUpdate()
    {
        Vector3 target = playerPosition.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }
}
