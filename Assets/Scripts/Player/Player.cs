using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    [SerializeField] private float moveSpeed;
    
    private float xInput;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal"); // GetAxisRaw - строго 1 или -1, тогда как GetAxis - плавает 
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleAnimations();
    }

    private void HandleMovement()
    {
        _rb.linearVelocity = new Vector2(xInput * moveSpeed, _rb.linearVelocity.y);
        
    }

    private void HandleAnimations()
    {
        //_animator.SetBool("isRunning", _rb.linearVelocity.x != 0); - удалить
        _animator.SetFloat("xVelocity", _rb.linearVelocity.x); // 1
    }
}
