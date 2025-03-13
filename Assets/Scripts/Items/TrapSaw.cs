using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSaw : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    [SerializeField] private float speed;
    [SerializeField] private float cooldown = 1;
    [SerializeField] private Transform[] wayPoints;
    
    private Vector3[] _wayPointPositions; // ++
    
    private bool _canMove = true;
    private int _waypointIndex = 1;
    private int _moveDirection = 1; // ++ 
    

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        UpdatesWayPointsInfo(); // ++
        
        //transform.position = wayPoints[0].position; // --
        transform.position = _wayPointPositions[0];
    }

    private void UpdatesWayPointsInfo() // ++
    {
        _wayPointPositions = new Vector3[wayPoints.Length]; // ++ инициализируем количеством объектов

        for (int i = 0; i < wayPoints.Length; i++) // ++ проходимся циклом и записываем мировую позицию 
        {
            _wayPointPositions[i] = wayPoints[i].position; // ++
        }
    }

    private void FixedUpdate()
    {
        _animator.SetBool("active", _canMove);
        
        if(!_canMove) return;
        
        //transform.position = Vector3.MoveTowards(transform.position, wayPoints[_waypointIndex].position, speed * Time.fixedDeltaTime); // --
        transform.position = Vector3.MoveTowards(transform.position, _wayPointPositions[_waypointIndex], speed * Time.fixedDeltaTime); // ++

        //if (Vector2.Distance(transform.position, wayPoints[_waypointIndex].position) < 0.1f) // --
        if (Vector2.Distance(transform.position, _wayPointPositions[_waypointIndex]) < 0.1f) // ++
        {
            //_waypointIndex++; // --

            //if (_waypointIndex == wayPoints.Length - 1 || _waypointIndex == 0) // ++ реверс в другую сторону // --
            if (_waypointIndex == _wayPointPositions.Length - 1 || _waypointIndex == 0) // ++
            {
                _moveDirection = _moveDirection * -1; // ++
                StartCoroutine(StopMovement(cooldown)); // ++
            }
            _waypointIndex = _waypointIndex + _moveDirection;
            

            /*if (_waypointIndex >= wayPoints.Length) // --
            {
                _waypointIndex = 0;
                StartCoroutine(StopMovement(cooldown));
            }*/
        }
    }

    private IEnumerator StopMovement(float delay)
    {
        _canMove = false;
        yield return new WaitForSeconds(delay);
        _canMove = true;
        
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }
}
