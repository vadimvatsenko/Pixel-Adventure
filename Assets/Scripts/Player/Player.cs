using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    
    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    
    [Header("Collision Info")]
    private bool _isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    
    private float _xInput;
    
    private bool _isFacingRight = true; // 1 - смотрит ли персонаж на право
    private int _facingDir = 1; // 2 - если смотрит в право (1), на лево (-1)
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleCollisions();
        HandleMovement();
        HandleAnimations();
        HandleFlip(); // 3 - метод переворачивания персонажа
    }

    private void HandleMovement()
    {
        _rb.linearVelocity = new Vector2(_xInput * moveSpeed, _rb.linearVelocity.y);
        
    }

    private void HandleCollisions()
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void HandleInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal"); // GetAxisRaw - строго 1 или -1, тогда как GetAxis - плавает 
        
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) Jump();
        
    }

    private void HandleAnimations()
    {
        _animator.SetFloat("xVelocity", _rb.linearVelocity.x); 
        _animator.SetFloat("yVelocity", _rb.linearVelocity.y); // анимация прыжка и падения
        _animator.SetBool("isGrounded", _isGrounded); // проверка земли, для выполнения анимации
    }

    private void Jump() => _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);

    private void HandleFlip() // 4 - метод переворачивания
    {
        if (_rb.linearVelocity.x < 0 && _isFacingRight || _rb.linearVelocity.x > 0 && !_isFacingRight)
        {
            Flip();
        }
    }
    private void Flip() // 5
    {
        _facingDir *= -1;
        transform.Rotate(0f, 180f, 0f);
        _isFacingRight = !_isFacingRight;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
