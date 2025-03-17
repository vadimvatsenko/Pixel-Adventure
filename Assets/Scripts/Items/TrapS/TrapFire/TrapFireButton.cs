using UnityEngine;

public class TrapFireButton : MonoBehaviour
{
    private Animator _animator;
    private TrapFire _trapFire;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _trapFire = GetComponentInParent<TrapFire>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            _animator.SetTrigger("active");
            _trapFire.SwitchOffFire();
        }
    }
}
