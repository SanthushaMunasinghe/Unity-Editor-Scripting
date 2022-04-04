using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [SerializeField] private Vector3 followPosition;

    void Update()
    {
        
    }

    private void OnValidate()
    {
        updateCamera();
    }

    private void updateCamera()
    {
        if (Camera == null)
        {
            return;
        }

        Camera.transform.position = transform.position + followPosition;
        Camera.transform.rotation = Quaternion.LookRotation(transform.position - Camera.transform.position, Vector3.up);
    }
}
