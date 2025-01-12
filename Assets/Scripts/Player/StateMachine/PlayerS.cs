using UnityEngine;

public class PlayerS : MonoBehaviour
{
    #region Components
    public Animator Animator { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    #endregion
    
    #region Direction
    public float Xinput { get; private set; }
    private bool _isFacingRight = true; // смотрит ли персонаж на право
    private int _facingDir = 1; // если смотрит в право (1), на лево (-1)
    #endregion
    
    #region Properties
    public float MoveSpeed => moveSpeed;
    public float JumpForce => jumpForce;
    #endregion
    
    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    #endregion
    
    [Header("Movement details")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    
    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState = new PlayerAirState(this, StateMachine, "Jump");
    }

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Rb  = GetComponent<Rigidbody2D>();
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        HandelInput();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.Update();
        Flip();
        
        this.Animator.SetFloat("yVelocity", Rb.linearVelocity.y);
        
    }

    private void HandelInput()
    {
        Xinput = Input.GetAxisRaw("Horizontal");
    }

    private void Flip()
    {
        if (Rb.linearVelocity.x < 0 && _isFacingRight || Rb.linearVelocity.x > 0 && !_isFacingRight)
        {
            _isFacingRight = !_isFacingRight;
            this.transform.Rotate(0f, 180f, 0f);
            _facingDir *= -1;
        }
    }
    
    public bool IsGroundDetected() => 
        Physics2D.Raycast(this.transform.position, Vector2.down, groundCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, 
            new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(this.transform.position, 
            new Vector2(transform.position.x + wallCheckDistance * _facingDir, transform.position.y));
    }
}
