using System.Collections;
using System.Linq;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Objects")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay; 
    private Player player;
    
    [Header("Fruits Management")] 
    [SerializeField] private bool fruitsHaveRandomLook;
    private int _fruitsCollected = 0;
    private int _totalFruits;

    [Header("CheckPoints")] // ++
    [SerializeField]
    private bool canReactivate; // ++

    public bool CanReactivate // ++
    {
        get => canReactivate;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() 
    {
        CollectFruitsInfo();
    }

    private void CollectFruitsInfo() 
    {
        Fruit[] _allFruitsArray = FindObjectsOfType<Fruit>();
        _totalFruits = _allFruitsArray.Length;
    }

    // метод который будет обновлять позицию респавна игрока
    public void UpdateRespawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;
    
    
    // обвёртка для курутины, теперь ее можно вызвать из другого скрипта
    public void RespawnPlayer() => StartCoroutine(RespawnCourutine()); 
    
    private IEnumerator RespawnCourutine() 
    {
        yield return new WaitForSeconds(respawnDelay);
        
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
    }
    public void AddFruit() => _fruitsCollected++;
    public bool FruitsHaveRandomLook() => fruitsHaveRandomLook;
}
