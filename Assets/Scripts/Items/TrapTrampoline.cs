using UnityEngine;

public class TrapTrampoline : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private int pushPower;
    [SerializeField] private float pushDuration = 0.5f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log(player);
            player.Push(transform.up * pushPower, pushDuration);
            // почему transform.up? если мы прикрепим объект на стену, transform.up - будет смотреть в лево,
            // соответственно игрок отскочит в лево
            _animator.SetTrigger("active");
        }
    }
}
