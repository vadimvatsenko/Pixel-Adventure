using System;
using UnityEngine;

public class SpikedBall : MonoBehaviour
{
    [SerializeField] private Rigidbody2D spikeRb;
    [SerializeField] private float pushForce;

    private void Start()
    {
        Vector2 pushForceVector = new Vector2(pushForce, 0);
        spikeRb.AddForce(pushForceVector, ForceMode2D.Impulse);
    }
}
