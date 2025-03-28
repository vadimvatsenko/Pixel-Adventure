﻿

    using UnityEngine;

    public class PlayerJumpFallState : PlayerAirState
    {
        public PlayerJumpFallState(PlayerS hero, PlayerStateMachine stateMachine, string animBoolName) : base(hero,
            stateMachine, animBoolName)
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
