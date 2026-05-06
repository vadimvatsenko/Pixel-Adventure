using UnityEngine;

public class Enemy_SnailBody : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _zRotation;

    public void SetupBody(float yVelocity, float zRotation)
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, yVelocity);

        _zRotation = zRotation;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, _zRotation * Time.deltaTime));
    }
}
