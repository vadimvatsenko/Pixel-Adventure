using UnityEngine;

// Нужно создать куб 3d, текстуры которые будем использовать, нужно сделать WrapMode => Repit
// Нужно создать текстуру на текстуру передать спрайт, Shader => Unit/Texture

public enum BackgroundType
{
    Blue,
    Brown,
    Gray,
    Green,
    Pink,
    Purple,
    Yellow
}

public class AnimatedBackground : MonoBehaviour
{
    [SerializeField] private Vector2 moveDir;
    private MeshRenderer _meshRenderer;
    
    [Header("Color")]
    [SerializeField] private BackgroundType backgroundType;
    [SerializeField] private Texture2D[] backgroundTexture;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        UpdateBackgroundTexture();
    }

    private void Update()
    {
        _meshRenderer.material.mainTextureOffset += moveDir * Time.deltaTime;
    }
    
    [ContextMenu("Update Background Texture")]
    private void UpdateBackgroundTexture()
    {
        _meshRenderer.material.mainTexture = backgroundTexture[(int)backgroundType];
    }
}
