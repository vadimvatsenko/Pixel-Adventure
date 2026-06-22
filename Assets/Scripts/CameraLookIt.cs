using UnityEngine;

public class CameraLookIt : MonoBehaviour
{
    [SerializeField] private GameObject lookTarget;
    
    private void LateUpdate()
    {
        transform.position = new Vector3(lookTarget.transform.position.x, lookTarget.transform.position.y, transform.position.z);
    }
}
