using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraLogic : MonoBehaviour
{
    public Transform car;
    public float smoothSpeed = 0.125f; // Adjust for smoothness
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (car != null)
        {
            Vector3 desiredPosition = car.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            //transform.LookAt(car);
        }
    }
}
