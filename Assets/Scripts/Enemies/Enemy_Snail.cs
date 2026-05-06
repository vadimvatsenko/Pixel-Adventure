using UnityEngine;

public class Enemy_Snail : Enemy
{
    private static readonly int Hit1 = Animator.StringToHash("hit");
    
    [Header("Snail Details")]
    [SerializeField] private Enemy_SnailBody bodyPrefab;
    
    private bool _hasBody = true;
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

    public override void Die()
    {
        if (_hasBody)
        {
            Anim.SetTrigger(Hit1);
            Rb.linearVelocity = Vector2.zero;
            idleDuration = 0;
            
            CanMove = false;
            _hasBody = false;
        }
        else
        {
            base.Die();
        }
    }

    private void HandleMovement()
    {
        if(IdleTimer > 0) return;
        
        if(!CanMove)  return;

        if (IsGroundInFrontDetected)
        {
            Rb.linearVelocity = new Vector2(movementSpeed * FacingDirection, Rb.linearVelocity.y);
        }
    }

    private void CreateBody()
    {
        Enemy_SnailBody newBody = Instantiate(bodyPrefab, transform.position, transform.rotation);
        
        if (Random.Range(0f, 100f) < 50) 
        {
            DeathRotationDirection = DeathRotationDirection * -1; 
        }
        
        newBody.SetupBody(deathImpactSpeed, deathRotationSpeed * DeathRotationDirection);
        Destroy(newBody.gameObject, 10f);
    }
}
