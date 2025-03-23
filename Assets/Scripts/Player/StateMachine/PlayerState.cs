using UnityEngine;

public  class PlayerState
{
    protected readonly PlayerStateMachine StateMachine;
    protected readonly PlayerS Hero;
    protected Rigidbody2D Rigidbody2D;
    
    private readonly string _animBoolName;
    protected float XInput;

    public PlayerState(PlayerS hero, PlayerStateMachine stateMachine): this(hero, stateMachine, null)
    {
        StateMachine = stateMachine;
        Hero = hero;
    }
    protected PlayerState(PlayerS hero, PlayerStateMachine stateMachine, string animBoolName)
    {
        Hero = hero;
        StateMachine = stateMachine;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        Hero.Animator.SetBool(_animBoolName, true);
        Rigidbody2D = Hero.Rb;
    }

    public virtual void Update()
    {
        Debug.Log($"I am in {_animBoolName}");
        XInput = Input.GetAxisRaw("Horizontal");
        
        SetAnimation();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StateMachine.ChangeState(Hero.JumpFallState);
        }

        if (Hero.IsGroundDetected())
        {
            StateMachine.ChangeState(Hero.IdleMoveState);
        }
    }

    public virtual void FixedUpdate()
    {
        
    }
    
    public virtual void Exit()
    {
        Hero.Animator.SetBool(_animBoolName, false);
    }

    private void SetAnimation()
    {
        Hero.Animator.SetFloat("xVelocity", XInput);
        Hero.Animator.SetFloat("yVelocity", Hero.Rb.velocity.y);
    }
}
