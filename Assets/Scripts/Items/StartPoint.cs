using UnityEngine;
public class StartPoint : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player.Player player = collision.GetComponent<Player.Player>();
        if (player) _animator.SetTrigger("activate");
    }
}
