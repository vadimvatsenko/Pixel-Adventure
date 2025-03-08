using UnityEngine;
public class Fruit : MonoBehaviour
{
    [SerializeField] private FruitType fruitType; // тип фрукта с enum
    [SerializeField] private GameObject fruitVfx; // 1
    
    private GameManager _gameManager;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>(); // InChildren 
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        SetRandomFruit();
    }

    private void SetRandomFruit()
    {
        // если в менеджере не стоит буловое значение, то генерем фрукт, которій мы выбрали в enam в менеджере
        if (!GameManager.Instance.FruitsHaveRandomLook()) 
        {
            UpdateFruitVisuals();
            return;
        }
            
       
        int randomIndex = Random.Range(0, 8); // рандом для фрукта
        _animator.SetFloat("FruitIndex", randomIndex); // какое число такой и фрукт
    }

    
    private void UpdateFruitVisuals() => _animator.SetFloat("FruitIndex", (int)fruitType); 
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            _gameManager.AddFruit();
            Destroy(this.gameObject);
            
            GameObject newVfx = Instantiate(fruitVfx, this.transform.position, Quaternion.identity); // 2
        }
    }
}
