using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Button newGameButton;

    private void OnEnable()
    {
        newGameButton.onClick.AddListener(NewGame);
    }

    private void OnDisable()
    {
        newGameButton.onClick.RemoveAllListeners();
    }
    
    public void NewGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
