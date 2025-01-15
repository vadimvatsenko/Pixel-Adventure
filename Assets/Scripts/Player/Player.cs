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
    private float _yInput; // 15
    
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
        
        // 10 - обязательно такой порядок вызовов
        HandleWallSlide(); // 5
        HandleMovement();
        HandleFlip(); // метод переворачивания персонажа
        HandleCollisions();
        HandleAnimations();
    }

    private void HandleWallSlide() // 6 - метод скольжения
    {
        
        bool canWallSlide = _isWallDetected && _rb.linearVelocity.y < 0; // 11 - локальная переменная, можно ли скользить
        float yModifer = _yInput < 0? 1f : 0.05f; // 17 - модификатор скорости скольжения, если нажата кнопка вниз, то скорость модификатора 1
        
        if (!canWallSlide) return; // 12 - прекратить выполненение метода
        
        //if (_isWallDetected && _rb.linearVelocity.y < 0) // 13 - больше нам не нужно это условие
        //{
            // _rb.linearVelocity.y * 0.5f - потому что слад вниз должен быть медленным
            //_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * 0.5f);
        //}
        
        // _rb.linearVelocity.y * 0.5f - потому что слад вниз должен быть медленным
        
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * yModifer); // 14
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
        if (_isWallDetected) return; // 8 - если прикоснулись к стене, то не двигаемся.
        
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
        _yInput = Input.GetAxisRaw("Vertical"); // 16 - получаем вертикальное нажатие
        
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
        _animator.SetBool("isWallDetected", _isWallDetected); // 7 - анимация скольжения
    }

    private void Jump() => _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);

    private void DoubleJump() 
    {
        _canDoubleJump = false;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, doubleJumpForce);
    }

    private void HandleFlip() // метод переворачивания 
    {
        // 9 - тут нам нужно поменять условие, вместо if (_rb.linearVelocity.x < 0 && _isFacingRight || _rb.linearVelocity.x > 0 && !_isFacingRight)
        // на (_xInput < 0 && _isFacingRight || _xInput > 0 && !_isFacingRight) - это для убирание бега при столкновении
        // со стеной, что бы мы могли повернутся и идти в другую сторону
        if (_xInput < 0 && _isFacingRight || _xInput > 0 && !_isFacingRight)
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
