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
    
    private Vector3[] _wayPointPositions;
    
    private bool _canMove = true;
    private int _waypointIndex = 1;
    private int _moveDirection = 1; 
    

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        UpdatesWayPointsInfo(); 
        
        transform.position = _wayPointPositions[0];
    }

    private void UpdatesWayPointsInfo() 
    {
        // список который будет хранить все пути пилы
        // наслучай если мы добавим дополнительный путь, что нет в префабе, он добавится автоматом
        List<TrapsSawWayPoint> trapsSawWayPointsList 
            = new List<TrapsSawWayPoint>(GetComponentsInChildren<TrapsSawWayPoint>()); // ++
        
        // будем заполнять список
        if (trapsSawWayPointsList.Count != wayPoints.Length) // ++
        {
            wayPoints = new Transform[trapsSawWayPointsList.Count]; // ++

            for (int i = 0; i < trapsSawWayPointsList.Count; i++) // ++
            {
                wayPoints[i] = trapsSawWayPointsList[i].transform; // ++
            }
        }
        
        _wayPointPositions = new Vector3[wayPoints.Length]; //  инициализируем количеством объектов

        for (int i = 0; i < wayPoints.Length; i++) //  проходимся циклом и записываем мировую позицию 
        {
            _wayPointPositions[i] = wayPoints[i].position;
        }
    }

    private void FixedUpdate()
    {
        _animator.SetBool("active", _canMove);
        
        if(!_canMove) return;
        
        transform.position = 
            Vector3.MoveTowards(
                transform.position, 
                _wayPointPositions[_waypointIndex], 
                speed * Time.fixedDeltaTime);
        
        if (Vector2.Distance(transform.position, _wayPointPositions[_waypointIndex]) < 0.1f) 
        {
            if (_waypointIndex == _wayPointPositions.Length - 1 || _waypointIndex == 0) 
            {
                _moveDirection = _moveDirection * -1; 
                StartCoroutine(StopMovement(cooldown)); 
            }
            _waypointIndex = _waypointIndex + _moveDirection;
            
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
