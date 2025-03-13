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
    
    private bool _canMove = true;
    public int waypointIndex = 1;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        transform.position = wayPoints[0].position;
    }

    private void FixedUpdate()
    {
        _animator.SetBool("active", _canMove);
        
        if(!_canMove) return;
        
        transform.position = Vector3.MoveTowards(
                transform.position, 
                wayPoints[waypointIndex].position, 
                speed * Time.fixedDeltaTime);

        if (Vector2.Distance(transform.position, wayPoints[waypointIndex].position) < 0.1f)
        {
            waypointIndex++;
            _spriteRenderer.flipX = !_spriteRenderer.flipX;

            if (waypointIndex >= wayPoints.Length)
            {
                waypointIndex = 0;
                StartCoroutine(StopMovement(cooldown));
            }
        }
    }

    private IEnumerator StopMovement(float delay)
    {
        _canMove = false;
        yield return new WaitForSeconds(delay);
        _canMove = true;
    }
}
