
    using Unity.VisualScripting;
    using UnityEngine;

    public class PlayerStateController
    {
        private readonly PlayerStateMachine _stateMachine;
        private readonly PlayerS _hero;
        private readonly Rigidbody2D _rb;
        
        //private float _xInput;
        private float _yVelocity;

        public PlayerStateController(PlayerS hero, PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _hero = hero;
            _rb = hero.Rb;
        }

        public void Update()
        {
            PlayerStatus();
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _stateMachine.ChangeState(_hero.JumpFallState);
            }
            else if (_hero.IsGroundDetected() && _rb.linearVelocity.y == 0)
            {
                _stateMachine.ChangeState(_hero.IdleMoveState);
            }
            else if(_rb.linearVelocity.y < 0)
            {
                _stateMachine.ChangeState(_hero.AirState);
            }
        }

        private void FixedUpdate()
        {
            
        }

        private void PlayerStatus()
        {
            //_xInput = Input.GetAxisRaw("Horizontal");
            _hero.Animator.SetFloat("xVelocity", _hero.XInput);
            _hero.Animator.SetFloat("yVelocity", _rb.linearVelocity.y);
        }
    }
