using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerS player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        HandleInput();
    }

    
    public override void Exit()
    {
        base.Exit();
    }

    private void HandleInput()
    {
        if (Hero.Xinput != 0)
        {
            StateMachine.ChangeState(Hero.MoveState);
        }
    }
    
    
}
