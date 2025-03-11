using System.Collections;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Objects")]
    [SerializeField] private GameObject playerPrefab; // 1
    [SerializeField] private Transform respawnPoint; // 2
    [SerializeField] private float respawnDelay; // 3
    private Player player;
    
    [Header("Fruits Management")] 
    [SerializeField] private bool fruitsHaveRandomLook;
    private int _fruitsCollected = 0;
    
    
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

    public void RespawnPlayer() => StartCoroutine(RespawnCourutine()); // 4
    
    private IEnumerator RespawnCourutine() // 5
    {
        yield return new WaitForSeconds(respawnDelay);
        
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
    }
    public void AddFruit() => _fruitsCollected++;
    public bool FruitsHaveRandomLook() => fruitsHaveRandomLook;
}
