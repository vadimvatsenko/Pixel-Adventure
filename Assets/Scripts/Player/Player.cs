using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    
    [Header("Movement details")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    
    [Header("Double Jump details")]
    [SerializeField] private float doubleJumpForce; // 9 - сила двойного прижка
    private bool _canDoubleJump; // 1
    
    [Header("Collision Info")]
    private bool _isGrounded;
    private bool _isAirborne; // 5 - в воздухе ли мы
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    
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
        UpdateAirBornStatus(); // 7
        
        HandleCollisions();
        HandleMovement();
        HandleAnimations();
        HandleFlip(); // метод переворачивания персонажа
    }

    private void UpdateAirBornStatus() // 6 - переключатель состояния персонажа в воздухе
    {
        if (_isGrounded && _isAirborne) HandleLanding(); 
        if (!_isGrounded && !_isAirborne) BecomeAirborn();
    }

    private void BecomeAirborn() // 8
    {
        _isAirborne = true;
    }

    private void HandleLanding() // 7
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
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void HandleInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal"); // GetAxisRaw - строго 1 или -1, тогда как GetAxis - плавает 
        
        // if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) Jump(); // 3 - убираем проверку на землю
        if (Input.GetKeyDown(KeyCode.Space)) JumpButton(); // 4 
        
    }

    private void JumpButton() // 2 - Метод отвечающий за прыжок 
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

    private void DoubleJump() // 10 
    {
        _canDoubleJump = false;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, doubleJumpForce);
    }

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
