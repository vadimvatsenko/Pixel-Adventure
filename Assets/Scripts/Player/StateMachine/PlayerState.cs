using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine StateMachine;
    protected PlayerS Hero;
    
    private string _animBoolName;

    public PlayerState(PlayerS player, PlayerStateMachine stateMachine, string animBoolName)
    {
        Hero = player;
        StateMachine = stateMachine;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        Hero.Animator.SetBool(_animBoolName, true);
    }

    public virtual void Update()
    {
        
    }
    
    public virtual void Exit()
    {
        Hero.Animator.SetBool(_animBoolName, false);
    }
}
