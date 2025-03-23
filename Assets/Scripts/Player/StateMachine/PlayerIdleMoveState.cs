using UnityEngine;

public class PlayerIdleMoveState : PlayerState
    {
        public PlayerIdleMoveState(PlayerS hero, PlayerStateMachine stateMachine, string animBoolName) 
            : base(hero, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            Hero.Rb.linearVelocity = new Vector2(XInput * Hero.MoveSpeed, Hero.Rb.linearVelocity.y);
            Hero.HandleFlip();
            
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
