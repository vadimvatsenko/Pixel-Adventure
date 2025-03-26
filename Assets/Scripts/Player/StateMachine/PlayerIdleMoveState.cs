using UnityEngine;

public class PlayerIdleMoveState : PlayerGroundState
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
            
            Hero.Rb.linearVelocity = new Vector2(Hero.XInput * Hero.MoveSpeed, Hero.Rb.linearVelocity.y);
            Hero.HandleFlip();
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
