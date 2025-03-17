using System;
using UnityEngine;

public class TrapArrow :TrapTrampoline
{
    [Header("Additional info")] 
    [SerializeField] private float cooldown;
    [SerializeField] private bool rotationRight; // нужно ли двигатися в право
    [SerializeField] private float rotatioSpeed = 120; // скорость вращения
    private int _direction = -1;
    [Space] 
    [SerializeField] private float scaleUpSpeed = 10; // с какой скоростью будет появляться объект
    [SerializeField] private Vector3 targetScale; // целевой размер

    private void Start()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); // стартовый размер 
    }

    private void HandleScaleUp()
    {
        if (transform.localScale.x < targetScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleUpSpeed);
        }
    }

    private void Update()
    {
        HandleScaleUp();
        HandleRotation();
    }

    private void HandleRotation()
    {
        _direction = rotationRight ? -1 : 1;
        transform.Rotate(new Vector3(0, 0, _direction * rotatioSpeed * Time.deltaTime));
    }

    // суть этого метода такова, обьект удаляется и спустя какое-то время появляется
    // фабрика находится в GameManager
    private void OnDestroyMe()
    {
        GameObject arrowPrefab = GameManager.Instance.ArrowPrefab;
        GameManager.Instance.CreateObject(arrowPrefab, transform, cooldown);
        
        Destroy(gameObject);
    }
}
