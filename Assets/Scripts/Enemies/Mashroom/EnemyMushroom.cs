using UnityEngine;

public class EnemyMushroom : Enemy
{
    
    protected override void Awake()
    {
        base.Awake();
        // boxCollider = GetComponent<BoxCollider2D>(); // --
    }

    protected override void Update()
    {
        base.Update();
        
        if(IsDead) return; 
        
        HandleMovement();
        
        if (IsGrounded)
        {
            HandleTurnAround();
        }
    }
    
    private void HandleTurnAround()
    {
        if (!IsGroundInFrontDetected || IsWallDetected)
        {
            Flip();
            IdleTimer = idleDuration;
            Rb.linearVelocity = Vector2.zero; // нужно остановить врага на время
        }
    }

    private void HandleMovement()
    {
        if(IdleTimer > 0) return;

        if (IsGroundInFrontDetected)
        {
            Rb.linearVelocity = new Vector2(movementSpeed * FacingDirection, Rb.linearVelocity.y);
        }
    }
}
