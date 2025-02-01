using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    
    [Header("Movement details")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;

    // Buffer Jump - это механизм, который позволяет игроку выполнить прыжок,
    // даже если он нажал кнопку "прыжок" немного раньше, чем персонаж коснулся земли
    [Header("Buffer Jump")] // 1
    [SerializeField] private float _bufferJumpWindow = 0.25f; // 6 - Окно буфера (сколько секунд допустимо)
    private float _bufferJumpActivated = -1; // 2 - Фиксирует момент, когда нажата клавиша прыжка, хранит момент времени (в секундах), когда была нажата клавиша

    [Header("Wall interaction")] // Взаимодействие со стеной
    [SerializeField] private float wallJumpDuration = 0.6f; // задержка прыжка от стены
    [SerializeField] private Vector2 wallJumpForce; // сила прыжка от стены
    private bool _isWallJumping; 
    
    [Header("Knockback")] 
    [SerializeField] private float knockbackDuration = 1; 
    [SerializeField] private Vector2 knockbackPower; 
    private bool _isKnocked; 
    private bool _canBeKnocked; 
    
    [Header("Double Jump details")]
    [SerializeField] private float doubleJumpForce; // сила двойного прижка
    private bool _canDoubleJump; 
    
    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance; // дистанция до стены
    [SerializeField] private LayerMask whatIsGround;
    
    private bool _isGrounded;
    private bool _isAirborne; // в воздухе ли мы
    private bool _isWallDetected; // коснулись ли мы стены
    
    private float _xInput;
    private float _yInput; 
    
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
        
        if(Input.GetKeyDown(KeyCode.F)) // временно для проверки урона
        {
            Knockback();
        }
    }

    private void FixedUpdate()
    {
        UpdateAirBornStatus();
        
        if(_isKnocked) return; // мы не хотим ничего делать, если нас ударили
        // обязательно такой порядок вызовов
        HandleWallSlide(); 
        HandleMovement();
        HandleFlip(); // метод переворачивания персонажа
        HandleCollisions();
        HandleAnimations();
    }

    public void Knockback() 
    {
        if(_isKnocked) return; // если ударили, то не вызывать снова метод
        StartCoroutine(KnockbackRoutine()); 
        _animator.SetTrigger("knockback"); 
        
        _rb.linearVelocity = new Vector2(knockbackPower.x * -_facingDir, knockbackPower.y); 
    }

    private void HandleWallSlide() // метод скольжения
    {
        
        bool canWallSlide = _isWallDetected && _rb.linearVelocity.y < 0; // локальная переменная, можно ли скользить
        float yModifer = _yInput < 0? 1f : 0.05f; // модификатор скорости скольжения, если нажата кнопка вниз, то скорость модификатора 1
        
        if (!canWallSlide) return; // прекратить выполненение метода
        
        //if (_isWallDetected && _rb.linearVelocity.y < 0) // больше нам не нужно это условие
        //{
            // _rb.linearVelocity.y * 0.5f - потому что слад вниз должен быть медленным
            //_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * 0.5f);
        //}
        
        // _rb.linearVelocity.y * 0.5f - потому что слад вниз должен быть медленным
        
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * yModifer); 
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
        
        AttemtBufferJump(); // 8
    }

    private void HandleMovement()
    {
        if (_isWallDetected) return; // если прикоснулись к стене, то не двигаемся.
        if(_isWallJumping) return; // мы не ходим, если нам нужно отпрыгнуть от стены 
        
        _rb.linearVelocity = new Vector2(_xInput * moveSpeed, _rb.linearVelocity.y);
    }

    private void HandleCollisions()
    {
        _isGrounded = Physics2D.Raycast
            (transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        _isWallDetected = Physics2D.Raycast
            (transform.position, Vector2.right * _facingDir, wallCheckDistance, whatIsGround); // проверка на стену
    }

    private void HandleInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal"); // GetAxisRaw - строго 1 или -1, тогда как GetAxis - плавает 
        _yInput = Input.GetAxisRaw("Vertical"); // получаем вертикальное нажатие

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
            RequestBufferJump(); // 5
        } 
    }

    // 3 - Запрос на буферизированный прыжок: Когда игрок нажимает пробел в воздухе, фиксируется момент нажатия
    private void RequestBufferJump() 
    {
        if(_isAirborne) _bufferJumpActivated = Time.time; // 4 - Time.time фиксирует текущее время, когда игрок нажал кнопку прыжка.
    }

    // 7 - Когда персонаж приземляется (HandleLanding() вызывает AttemtBufferJump()),
    // проверяется, прошло ли менее _bufferJumpWindow секунд с момента нажатия клавиши.
    // Если условие выполняется, игрок выполняет прыжок автоматически.
    private void AttemtBufferJump() 
    {
        if (Time.time < _bufferJumpActivated + _bufferJumpWindow)
        {
            _bufferJumpActivated = 0; // Сбрасываем буфер
            Jump();
        }
    }

    private void JumpButton() // Метод отвечающий за прыжок 
    {
        if (_isGrounded)
        {
            Jump();
        }
        
        else if (_isWallDetected && !_isGrounded) // обязательно тут проверка 
        {
            WallJump();
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
        _animator.SetBool("isWallDetected", _isWallDetected); // анимация скольжения
    }

    private void Jump() => _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);

    private void DoubleJump()
    {
        _isWallJumping = false; 
        _canDoubleJump = false;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, doubleJumpForce);
    }

    private void WallJump() 
    {
        // wallJumpForce.x * -_facingDir - это сила прыжка по x умножена на противоположную сторону лица персонажа
        // так как когда мы скользим по стене персонаж смотрит в другую сторону, но направление персонажа остается
        // смотреть на стену

        _canDoubleJump = true; 
        _rb.linearVelocity = new Vector2(wallJumpForce.x * -_facingDir, wallJumpForce.y);

        Flip();  
        StopAllCoroutines(); 
        StartCoroutine(WallJumpRoutine()); 
    }

    private IEnumerator WallJumpRoutine() 
    {
        _isWallJumping = true;
        yield return new WaitForSeconds(wallJumpDuration);
        _isWallJumping = false;
    }

    private IEnumerator KnockbackRoutine() 
    {
        _canBeKnocked = false;
        _isKnocked = true;
        
        yield return new WaitForSeconds(knockbackDuration);
        
        _canBeKnocked = true;
        _isKnocked = false;
    }

    private void HandleFlip() // метод переворачивания 
    {
        // тут нам нужно поменять условие, вместо if (_rb.linearVelocity.x < 0 && _isFacingRight || _rb.linearVelocity.x > 0 && !_isFacingRight)
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
            (transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance)); // лучь на пол
        Gizmos.DrawLine
            (transform.position, new Vector2(transform.position.x + (wallCheckDistance * _facingDir), transform.position.y)); // лучь на стену
    }
}
