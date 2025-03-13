using System;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private Player _player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _player = other.gameObject.GetComponent<Player>();
        _player?.Knockback(this.transform.position.x); // _player? если есть игрок 
    }
}
