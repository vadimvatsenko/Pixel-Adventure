

    using UnityEngine;

    public class PlayerJumpFallState : PlayerState
    {
        public PlayerJumpFallState(PlayerS hero, PlayerStateMachine stateMachine, string animBoolName) : base(hero,
            stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Hero.Rb.linearVelocity = new Vector2(XInput * Hero.MoveSpeed, Hero.JumpForce);
        }

        public override void Update()
        {
            
            base.Update();

            if (Hero.IsGroundDetected())
            {
                StateMachine.ChangeState(Hero.IdleMoveState);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
