using UnityEngine;

public class Chicken : Enemy
{
    [Header("Chicken details")] 
    [SerializeField] private float aggroDuration;
    [SerializeField] private float detectionRange;
    
    private float _aggroTimer;
    private bool _playerDetection;
    private bool _canFlip = true;
    
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        Anim.SetFloat("xVelocity", Rb.linearVelocity.x);
        _aggroTimer -= Time.fixedDeltaTime;
        
        if(IsDead) return;

        if (_playerDetection)
        {
            CanMove = true;
            _aggroTimer = aggroDuration;
        }
        
        if(_aggroTimer <= 0) CanMove = false;
        
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
            CanMove = false;
            Rb.linearVelocity = Vector2.zero; // нужно остановить врага на время
        }
    }

    private void HandleMovement()
    {
        if(!CanMove) return;

        //if(Player) HandleFlip(Player.position.x);

        if (Player)
        {
            float xValue = Player.transform.position.x;
            HandleFlip(xValue);
        }
        
        if (IsGroundInFrontDetected)
        {
            Rb.linearVelocity = new Vector2(movementSpeed * facingDirection, Rb.linearVelocity.y);
        }
    }

    protected override void HandleFlip(float xValue)
    {
        if (xValue < transform.position.x && IsFacingRight || xValue > transform.position.x && !IsFacingRight)
        {
            if (_canFlip)
            {
                _canFlip = false;
                Invoke(nameof(Flip), 0.3f); // будет задержка поворота, когда персонаж перепрыгивает врага
            }
        }
    }
    

    protected override void Flip()
    {
        base.Flip();
        _canFlip = true;
    }

    protected override void HandleCollisions()
    {
        base.HandleCollisions();
        
        _playerDetection = 
            Physics2D.Raycast(
                transform.position, transform.right * facingDirection, detectionRange, whatIsPlayer);
    }
    protected override void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            transform.position, 
            new Vector2(transform.position.x + (detectionRange * facingDirection), transform.position.y));
    }
}
