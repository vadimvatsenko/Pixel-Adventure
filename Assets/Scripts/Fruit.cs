using UnityEngine;
public class Fruit : MonoBehaviour
{
    private GameManager _gameManager;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            _gameManager.AddFruit();
            Destroy(this.gameObject);
        }
    }
}
