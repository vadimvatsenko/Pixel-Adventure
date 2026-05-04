using System;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string groundLayerName = "Ground";
    private Rigidbody2D _rb;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    public void SetVelocity(Vector2 velocity) => _rb.velocity = velocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            collision.gameObject.GetComponent<Player>().Knockback(transform.position.x);
            Destroy(this.gameObject);
        }
        
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            Destroy(this.gameObject);
        }
    }
}
