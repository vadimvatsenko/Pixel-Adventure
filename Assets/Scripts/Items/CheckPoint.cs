using UnityEngine;
public class CheckPoint : MonoBehaviour
{
    private Animator _animator;
    private bool _active;

    [SerializeField] private bool canBeReactivated; // можно ли переактивировать чекпоинт

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start() // ++
    {
        canBeReactivated = GameManager.Instance.CanReactivate; // ++
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_active && !canBeReactivated) return;
        
        Player player = other.GetComponent<Player>();
        Debug.Log(player);
        
        if(player != null) ActivateCheckPoint();
    }

    private void ActivateCheckPoint()
    {
        _active = true;
        _animator.SetTrigger("activate");
        GameManager.Instance.UpdateRespawnPosition(transform);
    }
}
