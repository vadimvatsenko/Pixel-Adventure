using UnityEngine;

    public class PlayerGroundedState : PlayerState
    {
        public PlayerGroundedState(PlayerS player, PlayerStateMachine stateMachine, string animBoolName) 
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

            if (Input.GetKeyDown(KeyCode.Space) && Hero.IsGroundDetected())
            {
                StateMachine.ChangeState(Hero.JumpState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
