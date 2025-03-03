using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Player _player;
    private int _score = 0;
    
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

    public void AddFruit()
    {
        _score++;
    }
}
