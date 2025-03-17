using System.Collections;
using UnityEngine;

public class TrapFire : MonoBehaviour
{
    [SerializeField] private float offDuration;
    [SerializeField] private TrapFireButton trapFireButton;
    
    private Animator _animator;
    private Collider2D _collider;
    private bool _isActive;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
    }
    private void Start()
    {
        if (!trapFireButton)
        {
            Debug.LogError("TrapFireButton not found");
        }
        SetFire(true);
    }

    public void SwitchOffFire()
    {   
        if(!_isActive) return;
        StartCoroutine(FireCorutine());
    }

    private IEnumerator FireCorutine()
    {
        SetFire(false);
        yield return new WaitForSeconds(offDuration);
        SetFire(true);
    }
    private void SetFire(bool active)
    {
        _animator.SetBool("active", active);
        _collider.enabled = active;
        _isActive = active;
    }
}
