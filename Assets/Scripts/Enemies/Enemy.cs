using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");
    private static readonly int Hit = Animator.StringToHash("hit");
    
    private SpriteRenderer _sr => GetComponent<SpriteRenderer>();
    protected Animator Anim;
    protected Rigidbody2D Rb;
    protected Collider2D[] Col; 
    [CanBeNull] protected Transform Player; 
    
    [SerializeField] protected GameObject damageTrigger; 
    
    [Header("General Info")]
    [SerializeField] protected float movementSpeed = 2f;
    protected bool CanMove = true;
    [SerializeField] protected float idleDuration = 1.5f; 
    protected float IdleTimer;

    [Header("Death Details")] 
    [SerializeField] protected float deathImpactSpeed = 5; 
    [SerializeField] protected float deathRotationSpeed = 150; 
    protected int DeathRotationDirection = 1; 
    protected bool IsDead; 
        
    [Header("Basic collision")] 
    [SerializeField] protected float groundCheckDistance = 1.1f;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask whatIsPlayer; // ++
    [SerializeField] protected float detectionRange;
    
    [Header("Wall Detection")]
    [SerializeField] private float radiusWallDetection;
    [SerializeField] private Transform wallDetectionTransform;
    
    protected bool IsPlayerDetection;
    protected bool IsGrounded;
    protected bool IsWallDetected;
    protected bool IsGroundInFrontDetected;

    protected int FacingDirection = -1;
    protected bool IsFacingRight = false;
    
    protected virtual void Awake()
    {
        Anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        Col = GetComponentsInChildren<Collider2D>();
    }

    protected virtual void Start() // ++
    {
        InvokeRepeating(nameof(UpdatePlayer), 0, 1);
        GameManager.Instance.OnPlayerRespawned += UpdatePlayer;

        if (_sr.flipX && !IsFacingRight)
        {
            _sr.flipX = false;
            Flip();
        }
    }

    protected void OnDisable()
    {
        GameManager.Instance.OnPlayerRespawned -= UpdatePlayer;
    }

    private void UpdatePlayer() // ++
    {
        if (!Player)
        {
            Player = GameManager.Instance.Player.transform;
        }
    }

    protected virtual void Update()
    {
        IdleTimer -= Time.deltaTime;
        if(IsDead) HandleDeathRotation();
        HandleAnimator();
        HandleCollisions();
    }

    protected virtual void HandleAnimator()
    {
        Anim.SetFloat(XVelocity, Rb.linearVelocity.x);
    }

    public virtual void Die() 
    {
        foreach (var c in Col)
        {
            c.enabled = false;
        }
        
        damageTrigger.SetActive(false); 
        Anim.SetTrigger(Hit); 
        Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, deathImpactSpeed); 
        
        IsDead = true; 

        if (Random.Range(0f, 100f) < 50) 
        {
            DeathRotationDirection = DeathRotationDirection * -1; 
        }
        
        Destroy(this.gameObject, 5f); 
    }

    private void HandleDeathRotation() 
    {
        transform.Rotate(0,0,(DeathRotationDirection * deathRotationSpeed) * Time.deltaTime); 
    }

    protected virtual void HandleCollisions()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        IsGroundInFrontDetected = Physics2D.Raycast
            (groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        
        IsPlayerDetection = 
            Physics2D.Raycast(
                transform.position, Vector2.right * FacingDirection, detectionRange, whatIsPlayer);
        
        WallDetected();
    }

    protected virtual void WallDetected()
    {
        IsWallDetected = Physics2D.OverlapCircle(wallDetectionTransform.position, radiusWallDetection, whatIsGround);
    }

    protected virtual void HandleFlip(float xValue) // метод переворачивания 
    {
        if (xValue < transform.position.x && IsFacingRight || xValue > transform.position.x && !IsFacingRight) // ++
        {
            Flip();
        }
    }
    protected virtual void Flip() 
    {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
        IsFacingRight = !IsFacingRight;
    }

    [ContextMenu("Change Facing Direction")]
    public void FlipDefaultFacingDirection()
    {
        _sr.flipX = !_sr.flipX;
    }
    
    protected virtual void OnDrawGizmos() // ++ был private
    {
        Gizmos.DrawLine
            (transform.position, new Vector2(groundCheck.position.x, transform.position.y - groundCheckDistance)); // луч на пол
        Gizmos.DrawLine
            (groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance)); // луч на пол
        
        Gizmos.DrawLine(
            transform.position, 
            new Vector2(transform.position.x + (detectionRange * FacingDirection), transform.position.y));
        
        if (wallDetectionTransform != null)
        {
            Gizmos.color = IsWallDetected ? Color.green : Color.red;
            Gizmos.DrawWireSphere(wallDetectionTransform.position, radiusWallDetection);
        }
    }
}
