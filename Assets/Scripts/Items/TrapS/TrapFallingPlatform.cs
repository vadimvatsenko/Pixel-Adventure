using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrapFallingPlatform : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private BoxCollider2D[] _colliders;
    
    [SerializeField] private float speed = 0.75f;
    [SerializeField] private float travelDistance;
    private Vector3[] wayPoints;
    private int wayPointIndex;
    private bool canMove = false;

    [Header("Trap Falling Platform Details")] 
    [SerializeField] private float impactSpeed = 3f;
    [SerializeField] private float impactDuration = 0.1f;
    private float _impactTimer;
    private bool _impactHappend;
    [Space]
    [SerializeField] private float fallDelay = 0.5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _colliders = GetComponents<BoxCollider2D>();
    }
    private void Start()
    {
        SetupWayPoints();
        
        // метод в котором будет создаватся рандомная стартовая точка для платформы,
        // в случай если у нас будет много платформ, чтобы они не выглядели одинаково
        float randomDelay = Random.Range(0f, travelDistance); 
        Invoke(nameof(ActivatePlatform), randomDelay);
    }
    
    private void ActivatePlatform() => canMove = true;
    
    private void SetupWayPoints()
    {
        wayPoints = new Vector3[2]; // инициализируем 2 точки, по которым будет двигаться платформа

        float yOffset = travelDistance / 2; // половинка нашего пути
        
        wayPoints[0] = transform.position + new Vector3(0, yOffset, 0); // точка 1, верх + половинка пути
        wayPoints[1] = transform.position + new Vector3(0, -yOffset, 0); // точка 2, низ + половинка пути
        // для того, что бы платформа начала двигатся со свого положения
        // на половину пути вверх и них от начала своей позиции
    }
    private void FixedUpdate()
    {
        HandleMovement();
        HandleImpact();
    }

    private void HandleMovement()
    {
        if (!canMove) return;
        
        transform.position = 
            Vector3.MoveTowards(
                transform.position, wayPoints[wayPointIndex], speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, wayPoints[wayPointIndex]) < 0.1f)
        {
            wayPointIndex++;
            if (wayPointIndex >= wayPoints.Length)
            {
                wayPointIndex = 0;
            }
        }
    }

    private void HandleImpact()
    {
        if(_impactTimer < 0) return;
        _impactTimer -= Time.fixedDeltaTime;
        
        transform.position = 
            Vector2.MoveTowards(
                transform.position, 
                transform.position + (Vector3.down * 10), 
                impactSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_impactHappend) return;
        Player player = other.GetComponent<Player>();
        if (player)
        {
            Invoke(nameof(SwitchOffPlatform), fallDelay); // вызывает метод с задержкой
            _impactTimer = impactDuration;
            _impactHappend = true;
        }
    }

    private void SwitchOffPlatform()
    {
        _animator.SetTrigger("deactivate");

        canMove = false;
        _rb.isKinematic = false;
        _rb.linearDamping = 0.5f;
        _rb.gravityScale = 3.5f;

        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
    }
}
