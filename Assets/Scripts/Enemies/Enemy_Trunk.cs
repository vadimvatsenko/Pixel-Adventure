using UnityEngine;

public class Enemy_Trunk : Enemy
{
    private static readonly int Attack1 = Animator.StringToHash("attack");

    [Header("Trunk Details")] 
    [SerializeField]  private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 7f;
    [SerializeField]  private Transform gunPoint;
    [SerializeField] private float attackCooldDown = 1.5f;
    [SerializeField] private float lastTimeAttacked;
    
    protected override void Update()
    {
        base.Update();
        
        if(IsDead) return; 
        
        bool canAttack = Time.time > lastTimeAttacked + attackCooldDown;

        if (IsPlayerDetection && canAttack)
        {
            Attack();
        }
        
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
    
    private void Attack()
    {
        IdleTimer = idleDuration;
        
        lastTimeAttacked = Time.time;
        Anim.SetTrigger(Attack1);
    }

    protected override void HandleAnimator()
    {
        // пустой, чтобы не вызывался xVelocity. Его нету на Plant
    }

    private void CreateBullet()
    {
        Enemy_Bullet bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity).GetComponent<Enemy_Bullet>();
        Vector2 bulletVelocity = new Vector2(bulletSpeed * FacingDirection, 0);
        bullet.SetVelocity(bulletVelocity);
          
        Destroy(bullet.gameObject, 10f);
    }
}
