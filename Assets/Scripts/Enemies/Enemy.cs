using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    protected Animator Anim;
    protected Rigidbody2D Rb;
    protected Collider2D Col; // ++
    [CanBeNull] protected Transform Player; // ++
    
    [SerializeField] protected GameObject damageTrigger; 
    
    [Header("General Info")]
    [SerializeField] protected float movementSpeed = 2f;
    protected bool CanMove = true; // ++
    [SerializeField] protected float idleDuration = 1.5f; 
    protected float IdleTimer;

    [Header("Death Details")] 
    [SerializeField] private float deathImpactSpeed = 5; 
    [SerializeField] private float deathRotationSpeed = 150; 
    private int _deathRotationDirection = 1; 
    protected bool IsDead; 
        
    [Header("Basic collision")] 
    [SerializeField] protected float groundCheckDistance = 1.1f;
    [SerializeField] protected float wallCheckDistance = 0.7f;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask whatIsPlayer; // ++
    
    protected bool IsGrounded;
    protected bool IsWallDetected;
    protected bool IsGroundInFrontDetected;

    protected int facingDirection = -1;
    protected bool IsFacingRight = false;
    
    protected virtual void Awake()
    {
        Anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        Col = GetComponent<Collider2D>();
    }

    protected virtual void Start() // ++
    {
        
        //Player = GameObject.FindObjectOfType<Player>().transform;
        //InvokeRepeating(nameof(UpdatePlayer), 0, 1);
        GameManager.Instance.OnPlayerRespawned += UpdatePlayer;
    }

    protected void OnDisable()
    {
        GameManager.Instance.OnPlayerRespawned += UpdatePlayer;
    }

    private void UpdatePlayer() // ++
    {
        if (!Player)
        {
            Player = GameManager.Instance.Player.transform;
        }
    }

    protected virtual void FixedUpdate()
    {
        IdleTimer -= Time.fixedDeltaTime;
        
        if(IsDead) HandleDeathRotation(); 
    }

    public virtual void Die() 
    {
        Col.enabled = false; // ++
        damageTrigger.SetActive(false); 
        Anim.SetTrigger("hit"); 
        Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, deathImpactSpeed); 
        
        IsDead = true; 

        if (Random.Range(0f, 100f) < 50) 
        {
            _deathRotationDirection = _deathRotationDirection * -1; 
        }
        
        Destroy(this.gameObject, 5f); 
    }

    private void HandleDeathRotation() 
    {
        transform.Rotate(0,0,(_deathRotationDirection * deathRotationSpeed) * Time.fixedDeltaTime); 
    }

    protected virtual void HandleCollisions()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        IsGroundInFrontDetected = Physics2D.Raycast
            (groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        IsWallDetected = Physics2D.Raycast
            (transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround); // проверка на стену
    }
    
    protected virtual void HandleFlip(float xValue) // метод переворачивания 
    {
        // тут нам нужно поменять условие, вместо if (_rb.linearVelocity.x < 0 && _isFacingRight || _rb.linearVelocity.x > 0 && !_isFacingRight)
        // на (_xInput < 0 && _isFacingRight || _xInput > 0 && !_isFacingRight) - это для убирание бега при столкновении
        // со стеной, что бы мы могли повернутся и идти в другую сторону
        
        //if (xValue < 0 && IsFacingRight || xValue > 0 && !IsFacingRight) // --
        if (xValue < transform.position.x && IsFacingRight || xValue > transform.position.x && !IsFacingRight) // ++
        {
            Flip();
        }
    }
    protected virtual void Flip() 
    {
        facingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
        IsFacingRight = !IsFacingRight;
    }
    
    protected virtual void OnDrawGizmos() // ++ был private
    {
        Gizmos.DrawLine
            (transform.position, new Vector2(groundCheck.position.x, transform.position.y - groundCheckDistance)); // луч на пол
        Gizmos.DrawLine
            (groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance)); // луч на пол
        Gizmos.DrawLine
            (groundCheck.position, new Vector2(transform.position.x + (wallCheckDistance * facingDirection), transform.position.y)); // луч на стену
    }
}
