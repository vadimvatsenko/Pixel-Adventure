using UnityEngine;

public class PlayerState
{
    protected readonly PlayerStateMachine StateMachine;
    protected PlayerS Player;
    protected Rigidbody2D Rigidbody2D;
    
    private string _animBoolName;
    protected float XInput;

    public PlayerState(PlayerS player, PlayerStateMachine stateMachine, string animBoolName)
    {
        Player = player;
        StateMachine = stateMachine;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        Player.Animator.SetBool(_animBoolName, true);
        Rigidbody2D = Player.Rb;
    }

    public virtual void Update()
    {
        XInput = Input.GetAxis("Horizontal");
    }
    
    public virtual void Exit()
    {
        Player.Animator.SetBool(_animBoolName, false);
    }
}
