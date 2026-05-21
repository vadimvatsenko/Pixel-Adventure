using System;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string groundLayerName = "Ground";
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }
    
    public void FlipSprite() => _sr.flipX = !_sr.flipX;
    public void SetVelocity(Vector2 velocity) => _rb.linearVelocity = velocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.Player player = collision.gameObject.GetComponent<Player.Player>();
        if (player != null)
        {
            collision.gameObject.GetComponent<Player.Player>().Knockback(transform.position.x);
            Destroy(this.gameObject);
        }
        
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            Destroy(this.gameObject);
        }
    }
}
