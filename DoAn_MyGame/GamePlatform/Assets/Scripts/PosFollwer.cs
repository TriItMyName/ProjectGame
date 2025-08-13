using UnityEngine;

public class PosFollwer : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 offset;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            offset = transform.position - mainCamera.transform.position;
        }
    }

    void Update()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
        {
            transform.position = mainCamera.transform.position + offset;
        }
    }

    void OnDrawGizmos()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(mainCamera.transform.position, transform.position);
        }
    }
}
