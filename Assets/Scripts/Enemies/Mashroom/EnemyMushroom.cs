using UnityEngine;

public class EnemyMushroom : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        
        HandleMovement();
        HandleCollisions();

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
