using UnityEngine;

public class Chicken : Enemy
{
    [Header("Chicken details")] 
    [SerializeField] private float aggroDuration; // продолжительность агрессии 
    
    private float _aggroTimer; // таймер агресивности
    private bool _playerDetection; // обнаружен ли игрок
    private bool _canFlip = true; // можно ли развернутся
    
    protected override void Update()
    {
        base.Update();
        
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
        
        if (!IsGroundInFrontDetected)
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
        
        if(IsWallDetected) Flip();
        
        if (Player)
        {
            float xValue = Player.transform.position.x;
            HandleFlip(xValue);
        }
        
        if (IsGroundInFrontDetected)
        {
            Rb.linearVelocity = new Vector2(movementSpeed * FacingDirection, Rb.linearVelocity.y);
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
        _canFlip = false;
    }

    protected override void HandleCollisions()
    {
        base.HandleCollisions();
        
        _playerDetection = 
            Physics2D.Raycast(
                transform.position, Vector2.right * FacingDirection, detectionRange, whatIsPlayer);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
