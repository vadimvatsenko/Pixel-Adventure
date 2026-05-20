using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Button newGameButton;
    [SerializeField] private UI_FadeEffect fadeEffect;
    
    private void Start()
    {
        fadeEffect.ScreenFadeIn(0f, 1.5f);
    }

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
        fadeEffect.ScreenFadeIn(1f, 1.5f, LoadLevelScene);
    }
    
    private void LoadLevelScene() => SceneManager.LoadScene(sceneName);
    
}
