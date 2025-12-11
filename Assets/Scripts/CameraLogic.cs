using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using TMPro;

/* Johnathan Spangler
 * 12/09/25
 * Controls the player's camera
 */

public class CameraLogic : MonoBehaviour
{
    public Transform car;
    public float rotationSmoothTime = 0.2f, rotationSmooth = 0.25f;
    public Vector3 velocity = Vector3.zero, offset;

    //Camera Controller
    void FixedUpdate()
    {
        if (car == null) return;

        //Camera follow the car
        Vector3 desiredPos = car.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, rotationSmoothTime);

        //Camera face the car
        Quaternion targetRot = Quaternion.LookRotation(car.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSmooth * Time.deltaTime);
    }
}
