using UnityEngine;

public class EnemyMushroom : Enemy
{
    private BoxCollider2D boxCollider; // ++
    
    protected override void Awake()
    {
        base.Awake();
        boxCollider = GetComponent<BoxCollider2D>(); // ++
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        
        if(IsDead) return; // ++
        
        HandleMovement();
        HandleCollisions();

        if (IsGrounded)
        {
            HandleTurnAround();
        }
    }

    public override void Die()
    {
        base.Die();
        boxCollider.enabled = false;
        
    }

    private void HandleTurnAround()
    {
        if (!IsGroundInFrontDetected || IsWallDetected)
        {
            Flip();
            IdleTimer = idleDuration;
            rb.linearVelocity = Vector2.zero; // нужно остановить врага на время
        }
    }

    private void HandleMovement()
    {
        if(IdleTimer > 0) return;

        if (IsGroundInFrontDetected)
        {
            rb.linearVelocity = new Vector2(movementSpeed * facingDirection, rb.linearVelocity.y);
        }
    }
}
