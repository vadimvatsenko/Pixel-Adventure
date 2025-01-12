using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    
    [Header("Movement details")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    
    [Header("Double Jump details")]
    [SerializeField] private float doubleJumpForce; // сила двойного прижка
    private bool _canDoubleJump; 
    
    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance; // 2 - дистанция до стены
    [SerializeField] private LayerMask whatIsGround;
    private bool _isGrounded;
    private bool _isAirborne; // в воздухе ли мы
    private bool _isWallDetected; // 1 - коснулись ли мы стены
    
    private float _xInput;
    
    private bool _isFacingRight = true; // смотрит ли персонаж на право
    private int _facingDir = 1; // если смотрит в право (1), на лево (-1)
    
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
        UpdateAirBornStatus();
        HandleWallSlide(); // 5
        HandleCollisions();
        HandleMovement();
        HandleAnimations();
        HandleFlip(); // метод переворачивания персонажа
    }

    private void HandleWallSlide() // 6 - метод скольжения
    {
        if (_isWallDetected && _rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * 0.5f);
        }
    }

    private void UpdateAirBornStatus() // переключатель состояния персонажа в воздухе
    {
        if (_isGrounded && _isAirborne) HandleLanding(); 
        if (!_isGrounded && !_isAirborne) BecomeAirborn();
    }

    private void BecomeAirborn() 
    {
        _isAirborne = true;
    }

    private void HandleLanding() 
    {
        _isAirborne = false;
        _canDoubleJump = true;
    }

    private void HandleMovement()
    {
        _rb.linearVelocity = new Vector2(_xInput * moveSpeed, _rb.linearVelocity.y);
        
    }

    private void HandleCollisions()
    {
        _isGrounded = Physics2D.Raycast
            (transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        _isWallDetected = Physics2D.Raycast
            (transform.position, Vector2.right * _facingDir, wallCheckDistance, whatIsGround); // 3 - проверка на стену
    }

    private void HandleInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal"); // GetAxisRaw - строго 1 или -1, тогда как GetAxis - плавает 
        
        if (Input.GetKeyDown(KeyCode.Space)) JumpButton(); 
        
    }

    private void JumpButton() // Метод отвечающий за прыжок 
    {
        if (_isGrounded)
        {
            Jump();
        }
        else if (_canDoubleJump)
        {
            DoubleJump(); 
        }
    }

    private void HandleAnimations()
    {
        _animator.SetFloat("xVelocity", _rb.linearVelocity.x); 
        _animator.SetFloat("yVelocity", _rb.linearVelocity.y); // анимация прыжка и падения
        _animator.SetBool("isGrounded", _isGrounded); // проверка земли, для выполнения анимации
    }

    private void Jump() => _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);

    private void DoubleJump() 
    {
        _canDoubleJump = false;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, doubleJumpForce);
    }

    private void HandleFlip() // метод переворачивания
    {
        if (_rb.linearVelocity.x < 0 && _isFacingRight || _rb.linearVelocity.x > 0 && !_isFacingRight)
        {
            Flip();
        }
    }
    private void Flip() 
    {
        _facingDir *= -1;
        transform.Rotate(0f, 180f, 0f);
        _isFacingRight = !_isFacingRight;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine
            (transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine
            (transform.position, new Vector2(transform.position.x + (wallCheckDistance * _facingDir), transform.position.y)); // 4 - лучь на стену
    }
}
