using UnityEngine;

public class PlayerS : MonoBehaviour
{
    #region Components
    public Animator Animator { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    #endregion
    
    #region Direction
    public float Xinput { get; private set; }
    public float Yinput { get; private set; }
    private bool _isFacingRight = true; // смотрит ли персонаж на право
    private int _facingDir = 1; // если смотрит в право (1), на лево (-1)
    #endregion
    
    #region Properties
    public float MoveSpeed => moveSpeed;
    public float JumpForce => jumpForce;
    #endregion
    
    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleMoveState IdleMoveState { get; private set; }
    public PlayerJumpFallState JumpFallState { get; private set; }
    
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
        IdleMoveState = new PlayerIdleMoveState(this, StateMachine, "MoveIdle");
        JumpFallState = new PlayerJumpFallState(this, StateMachine, "JumpFall");
    }

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Rb  = GetComponent<Rigidbody2D>();
        
        StateMachine.Initialize(IdleMoveState);
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.FixedUpdate();
    }
    
    public void HandleFlip() // 4 - метод переворачивания  
    {  
        if (Rb.linearVelocity.x < 0 && _isFacingRight || Rb.linearVelocity.x > 0 && !_isFacingRight)  
        {            
            Flip();  
        }    
    }    
    
    private void Flip() // 5  
    {  
        _facingDir *= -1;  
        transform.Rotate(0f, 180f, 0f);  
        _isFacingRight = !_isFacingRight;  
    }  
    
    public bool IsGroundDetected() => 
        Physics2D.Raycast(this.transform.position, Vector2.down, groundCheckDistance, whatIsGround);

    
}
