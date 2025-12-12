using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveVector;
    private Rigidbody2D rb;
    private Animator animator;

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const string LastVertical = "LastVertical";
    private const string LastHorizontal = "LastHorizontal";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        moveVector.Set(InputManager.movement.x, InputManager.movement.y);
        rb.linearVelocity = moveVector * moveSpeed;

        animator.SetFloat(Horizontal, moveVector.x);
        animator.SetFloat(Vertical, moveVector.y);

        if(moveVector != Vector2.zero)
        {
            animator.SetFloat (LastHorizontal, moveVector.x);
            animator.SetFloat (LastVertical, moveVector.y);
        }
    }
}
