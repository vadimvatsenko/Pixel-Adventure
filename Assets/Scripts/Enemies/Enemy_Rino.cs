using UnityEngine;

public class Enemy_Rino : Enemy
{
    private static readonly int HitWall = Animator.StringToHash("hit_wall");
    

    [Header("Rino Details")] 
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedUpRate = 0.6f;
    private float _defaulSpeed;
    [SerializeField] private Vector2 impactPower;
    
    private bool _canCharge = true;
    
    override protected void Start()
    {
        base.Start();
        _defaulSpeed = movementSpeed;
    }

    protected override void Update()
    {
        base.Update();
        
        HandleCollisions();
        HandleCharge();
    }
    
    private void HandleCharge()
    {
        if(!CanMove) return;

        movementSpeed = movementSpeed + (Time.deltaTime * speedUpRate); // ускорение

        if (movementSpeed > maxSpeed)
        {
            maxSpeed = movementSpeed;
        }
        
        if (IsPlayerDetection)
        {
            Rb.linearVelocity = new Vector2(movementSpeed * FacingDirection, Rb.linearVelocity.y);
        }

        if (!IsGroundInFrontDetected)
        {
            TurnAround();
        }
        
        if (IsWallDetected)
        {
            WallHit();
        }
    }
    
    private void TurnAround()
    {
        movementSpeed = _defaulSpeed;
        CanMove = false;
        Rb.linearVelocity = Vector2.zero;
        Flip();
        movementSpeed = _defaulSpeed;
    }

    private void WallHit()
    {
        CanMove = false;
        movementSpeed = _defaulSpeed;
        Anim.SetBool(HitWall, true);
        Rb.linearVelocity = new Vector2(impactPower.x * -FacingDirection, Rb.linearVelocity.y);
        
        Flip();
        movementSpeed = _defaulSpeed;
    }

    // вызывается по окончании анимации удара головой об стену
    private void ChargeIsOver()
    {
        Anim.SetBool(HitWall, false);
        Invoke(nameof(Flip), 1f);
    }

    protected override void HandleCollisions()
    {
        base.HandleCollisions();
        
        if (IsPlayerDetection && IsGrounded)
        {
            CanMove = true;
        }
    }
}
