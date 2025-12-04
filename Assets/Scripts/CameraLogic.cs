using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using TMPro;

public class CameraLogic : MonoBehaviour
{
    public Transform car;
    public CPUController AiControl;
    public float rotationSmoothTime = 0.2f, rotationSmooth = 0.25f;
    public Vector3 velocity = Vector3.zero, offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    void LateUpdate()
    {
        if (car == null || AiControl == null) return;

        if (AiControl.controller == Controller.Player)
        {
            //Camera follow the car
            Vector3 desiredPos = car.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, rotationSmoothTime);

            //Camera face the car
            Quaternion targetRot = Quaternion.LookRotation(car.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSmooth * Time.deltaTime);

        }
        else if (AiControl.controller == Controller.Computer)
        {
            gameObject.SetActive(false);
        }
    }
}
