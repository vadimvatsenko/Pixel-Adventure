using UnityEngine;
public class FinishPoint : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.Player player = collision.GetComponent<Player.Player>();

        if (player)
        {
            _animator.SetTrigger("activate");
            GameManager.Instance.LevelFinished();
        }
    }
}
