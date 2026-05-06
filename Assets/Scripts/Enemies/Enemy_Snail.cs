using UnityEngine;

public class Enemy_Snail : Enemy
{
    private static readonly int Hit1 = Animator.StringToHash("hit");
    private static readonly int Hit3 = Animator.StringToHash("hit_3");

    [Header("Snail Details")]
    [SerializeField] private Enemy_SnailBody bodyPrefab;

    [SerializeField] private float maxSpeed = 10f;
    
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
        bool canFlipFromLendge = !IsGroundInFrontDetected && _hasBody;
        
        if ( canFlipFromLendge || IsWallDetected)
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
        else if (!CanMove && !_hasBody)
        {
            Anim.SetTrigger(Hit1);
            CanMove = true;
            movementSpeed = maxSpeed;
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
            DeathRotationDirection *= -1; 
        }
        
        newBody.SetupBody(deathImpactSpeed, deathRotationSpeed * DeathRotationDirection, FacingDirection);
        Destroy(newBody.gameObject, 10f);
    }

    protected override void Flip()
    {
        base.Flip();

        if (!_hasBody)
        {
            Anim.SetTrigger(Hit3);
        }
    }
}
