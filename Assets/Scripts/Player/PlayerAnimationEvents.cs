using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player.Player _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player.Player>();
    }
    public void FinishRespawn() => _player.RespawnFinished(true);
}
