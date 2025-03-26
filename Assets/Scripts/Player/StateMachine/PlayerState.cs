using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState
{
    protected readonly PlayerStateMachine StateMachine;
    protected readonly PlayerS Hero;
    protected readonly Rigidbody2D Rb;
    private readonly string _animBoolName;
    
    protected PlayerState(PlayerS hero, PlayerStateMachine stateMachine, string animBoolName)
    {
        Hero = hero;
        StateMachine = stateMachine;
        _animBoolName = animBoolName;
        Rb = hero.Rb;
    }

    public virtual void Enter()
    {
        Hero.Animator.SetBool(_animBoolName, true);
    }

    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }
    
    public virtual void Exit()
    {
        Hero.Animator.SetBool(_animBoolName, false);
    }
    
}
