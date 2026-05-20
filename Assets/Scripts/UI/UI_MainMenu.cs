using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Button newGameButton;
    [SerializeField] private UI_FadeEffect fadeEffect;
    
    [SerializeField] private GameObject[] uiElements;
    
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

    public void SwitchUI(GameObject uiToEnable)
    {
        foreach (var go in uiElements)
        {
            go.SetActive(false);
        }
        uiToEnable.SetActive(true);
    }
    
    public void NewGame()
    {
        fadeEffect.ScreenFadeIn(1f, 1.5f, LoadLevelScene);
    }
    
    private void LoadLevelScene() => SceneManager.LoadScene(sceneName);
    
}
