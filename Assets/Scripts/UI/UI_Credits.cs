using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Credits : MonoBehaviour
{
    [SerializeField] private RectTransform rectT;
    [SerializeField] private float speed = 200f;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private float offScreenPosition = 1500f;
    [SerializeField] private UI_FadeEffect fadeEffect;
    
    private bool isCreditsSkiped = false;

    private void Awake()
    {
        fadeEffect.ScreenFadeIn(0,1.5f);
    }

    private void Update()
    {
        rectT.anchoredPosition += Vector2.up * (speed * Time.deltaTime);

        if (rectT.anchoredPosition.y >= offScreenPosition)
        {
            GoToMainMenu();
        }
    }

    public void SkipCredits()
    {
        if (!isCreditsSkiped)
        {
            speed *= 10;
            isCreditsSkiped = true;
        }
        else
        {
            GoToMainMenu();
        }
    }
    
    private void GoToMainMenu() => fadeEffect.ScreenFadeIn(1,1, GoToMainMenuScene);

    private void GoToMainMenuScene() => SceneManager.LoadScene(mainMenuSceneName);
}
