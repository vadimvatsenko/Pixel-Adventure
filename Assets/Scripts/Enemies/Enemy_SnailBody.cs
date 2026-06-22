using UnityEngine;

public class Enemy_SnailBody : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private float _zRotation;

    public void SetupBody(float yVelocity, float zRotation, int facingDirection)
    {
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, yVelocity);
        _zRotation = zRotation;
        
        if(facingDirection == 1) _sr.flipX = true;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, _zRotation * Time.deltaTime));
    }
}
