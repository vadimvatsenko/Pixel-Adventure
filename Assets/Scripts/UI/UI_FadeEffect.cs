using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeEffect : MonoBehaviour
{
    [SerializeField] private RawImage image;

    public void ScreenFadeIn(float targetAlpha, float duration, Action onComplete =  null) 
        => StartCoroutine(Fadecoroutine(targetAlpha, duration, onComplete));
    
    private IEnumerator Fadecoroutine(float targetAlpha, float duration, Action onComplete)
    {
        float time = 0;
        Color currentCollor = image.color;
        float startAlpha = currentCollor.a;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            
            image.color = new Color(currentCollor.r, currentCollor.g, currentCollor.b, alpha);
            yield return null;
        }
        
        image.color = new Color(currentCollor.r, currentCollor.g, currentCollor.b, targetAlpha);
        onComplete?.Invoke();
    }
}
