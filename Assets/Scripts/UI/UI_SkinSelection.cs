using UnityEngine;

public class UI_SkinSelection : MonoBehaviour
{
    [SerializeField] private int currentIndex;
    [SerializeField] private int maxIndex = 3;
    [SerializeField] private Animator skinAnimator;

    public void SelectSkin() => SkinManager.instance.SetSkinId(currentIndex);
    
    public void NextSkin()
    {
        currentIndex++;
        if (currentIndex > maxIndex)
        {
            currentIndex = 0;
        }
        
        UpdateSkinDisplay();
    }

    public void PrevSkin()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = maxIndex;
        }
        UpdateSkinDisplay();
    }

    private void UpdateSkinDisplay()
    {
        for (int i = 0; i < skinAnimator.layerCount; i++)
        {
            skinAnimator.SetLayerWeight(i, 0);
        }
        skinAnimator.SetLayerWeight(currentIndex, 1);
    }
}
