using System;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame Instance { get; private set; }
    public UI_FadeEffect fadeEffect;

    private void Awake()
    {
        Instance = this;
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        fadeEffect.ScreenFadeIn(0,1f, null);
    }
}
