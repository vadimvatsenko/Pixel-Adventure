using UnityEngine;

    public class PlayerJumpState : PlayerGroundedState
    {
        public PlayerJumpState(PlayerS player, PlayerStateMachine stateMachine, string animBoolName) 
            : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Hero.Rb.linearVelocity = new Vector2(Hero.Rb.linearVelocity.x, Hero.JumpForce);
        }

        public override void Update()
        {
            base.Update();
            
            if (Hero.Rb.linearVelocity.y < 0)
            {
                StateMachine.ChangeState(Hero.AirState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
