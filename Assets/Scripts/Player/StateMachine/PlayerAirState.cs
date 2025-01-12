using UnityEngine;

    public class PlayerAirState : PlayerGroundedState
    {
        public PlayerAirState(PlayerS player, PlayerStateMachine stateMachine, string animBoolName) 
            : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            if (Hero.IsGroundDetected())
            {
                StateMachine.ChangeState(Hero.IdleState);
            }
            
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
