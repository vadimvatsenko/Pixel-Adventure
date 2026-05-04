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
        HandleCharge();
    }
    
    private void HandleCharge()
    {
        if(!CanMove) return;

        HandleSpeedUp();

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

    private void HandleSpeedUp()
    {
        movementSpeed = movementSpeed + (Time.deltaTime * speedUpRate); // ускорение

        if (movementSpeed > maxSpeed)
        {
            maxSpeed = movementSpeed;
        }
    }

    private void TurnAround()
    {
        SpeedReset();
        CanMove = false;
        Rb.linearVelocity = Vector2.zero;
        Flip();
        movementSpeed = _defaulSpeed;
    }

    private void SpeedReset() => movementSpeed = _defaulSpeed;
    
    private void WallHit()
    {
        CanMove = false;
        SpeedReset();
        Anim.SetBool(HitWall, true);
        Rb.linearVelocity = new Vector2(impactPower.x * -FacingDirection, Rb.linearVelocity.y);
        
        Flip();
        SpeedReset();
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
