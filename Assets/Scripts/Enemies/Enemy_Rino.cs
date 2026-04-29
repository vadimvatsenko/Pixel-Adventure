using UnityEngine;

public class Enemy_Rino : Enemy
{
    private static readonly int HitWall = Animator.StringToHash("hit_wall");
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");

    [Header("Rino Details")] 
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedUpRate = 0.6f;
    private float _defaulSpeed;
    [SerializeField] private Vector2 impactPower;
    
    [Header("RiniDetection")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float radiusWallDetection;
    [SerializeField] private Transform wallDetectionTransform;
    private bool _playerDetection;
    private bool _canCharge = true;
    
    override protected void Start()
    {
        base.Start();
        _defaulSpeed = movementSpeed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        Anim.SetFloat(XVelocity, Rb.linearVelocity.x);
        
        HandleCollisions();
        HandleCharge();
        Debug.Log(_playerDetection);
    }
    
    private void HandleCharge()
    {
        if(!CanMove) return;

        movementSpeed = movementSpeed + (Time.deltaTime * speedUpRate); // ускорение

        if (movementSpeed > maxSpeed)
        {
            maxSpeed = movementSpeed;
        }
        
        if (_playerDetection)
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

    protected override void WallDetected()
    {
        IsWallDetected = Physics2D.OverlapCircle(wallDetectionTransform.position, radiusWallDetection, whatIsGround);
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
        
        _playerDetection = 
            Physics2D.Raycast(
                transform.position, transform.right * FacingDirection, detectionRange, whatIsPlayer);

        if (_playerDetection && IsGrounded)
        {
            CanMove = true;
        }
    }
    protected override void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            transform.position, 
            new Vector2(transform.position.x + (detectionRange * FacingDirection), transform.position.y));
        
        Gizmos.DrawWireSphere(wallDetectionTransform.position, radiusWallDetection);
    }
}
