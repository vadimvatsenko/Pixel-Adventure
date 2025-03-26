using UnityEngine;

    public class PlayerGroundState : PlayerState
    {
        protected PlayerGroundState(PlayerS hero, PlayerStateMachine stateMachine, string animBoolName) 
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
        }

        public override void Exit()
        {
            base.Exit();
        }
    }