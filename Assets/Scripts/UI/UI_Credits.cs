using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Credits : MonoBehaviour
{
    [SerializeField] private RectTransform rectT;
    [SerializeField] private float speed = 200f;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private float offScreenPosition = 1500f; 
    
    private bool isCreditsSkiped = false;

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

    private void GoToMainMenu() => SceneManager.LoadScene(mainMenuSceneName);
}
