using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float idleDuration;
    protected float IdleTimer; 
        
    [Header("Basic collision")] 
    [SerializeField] protected float groundCheckDistance = 1.1f;
    [SerializeField] protected float wallCheckDistance = 0.7f;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheck;
    
    protected bool IsGrounded;
    protected bool IsWallDetected;
    protected bool IsGroundInFrontDetected;

    protected int facingDirection = -1;
    protected bool IsFacingRight = false;
    
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        IdleTimer -= Time.fixedDeltaTime;
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
        if (xValue < 0 && IsFacingRight || xValue > 0 && !IsFacingRight)
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
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine
            (transform.position, new Vector2(groundCheck.position.x, transform.position.y - groundCheckDistance)); // луч на пол
        Gizmos.DrawLine
            (groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance)); // луч на пол
        Gizmos.DrawLine
            (groundCheck.position, new Vector2(transform.position.x + (wallCheckDistance * facingDirection), transform.position.y)); // луч на стену
    }
}
