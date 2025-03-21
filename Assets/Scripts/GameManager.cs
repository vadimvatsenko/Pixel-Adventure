using System;
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
    public Player Player { get; private set; }
    
    [Header("Fruits Management")] 
    [SerializeField] private bool fruitsHaveRandomLook;
    private int _fruitsCollected = 0;
    private int _totalFruits;

    [Header("CheckPoints")] 
    [SerializeField] private bool canReactivate;
    
    [Header("Traps")]
    [SerializeField] private GameObject arrowPrefab; // ++
    public GameObject ArrowPrefab => arrowPrefab; // ++ свойство вернёт префаб
    public bool CanReactivate => canReactivate;
    public event Action OnPlayerRespawned; // ++
    
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

    // ++ метод обвертка для курутины.
    public void CreateObject(GameObject prefab, Transform target, float delay)
    {
        StartCoroutine(CreateObjectRoutine(prefab, target, delay));
    }

    // ++ курутина создания объектов
    private IEnumerator CreateObjectRoutine(GameObject prefab, Transform target, float delay) 
    {
        Vector3 position = target.position;
        
        yield return new WaitForSeconds(delay);
        
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
    }

    // метод который будет обновлять позицию респавна игрока
    public void UpdateRespawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;
    
    // обвёртка для курутины, теперь ее можно вызвать из другого скрипта
    public void RespawnPlayer()
    {
        OnPlayerRespawned?.Invoke();
        StartCoroutine(RespawnCourutine());
    }
    
    private IEnumerator RespawnCourutine() 
    {
        yield return new WaitForSeconds(respawnDelay);
        
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        Player = newPlayer.GetComponent<Player>();
    }
    public void AddFruit() => _fruitsCollected++;
    public bool FruitsHaveRandomLook() => fruitsHaveRandomLook;
}
