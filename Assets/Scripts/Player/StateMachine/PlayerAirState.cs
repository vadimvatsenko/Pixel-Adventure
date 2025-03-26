
    public class PlayerAirState : PlayerState
    {
        public PlayerAirState(PlayerS hero, PlayerStateMachine stateMachine, string animBoolName) : base(hero,
            stateMachine, animBoolName)
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

            /*if (Hero.Rb.linearVelocity.y == 0)
            {
                StateMachine.ChangeState(Hero.IdleMoveState);
            }*/
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
