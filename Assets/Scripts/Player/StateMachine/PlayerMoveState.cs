using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerS player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (Hero.Xinput == 0)
        {
            StateMachine.ChangeState(Hero.IdleState);
        }

        HandleMovement();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void HandleMovement()
    {
        Hero.Rb.linearVelocity = new Vector2(Hero.Xinput * Hero.MoveSpeed, Hero.Rb.linearVelocity.y);
    }
}
