using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Ramon Samano
 * 11-18-25
 * slows down player when colliding with hazard
 */

public class Hazards : MonoBehaviour
{
    public float penalty = 7f;

    private void OnTriggerEnter(Collider other)
    {
        RacerMovement racer = other.GetComponentInParent<RacerMovement>();

        if (racer != null)
        {
            racer.currentSpeed = Mathf.Max(0f, racer.currentSpeed - penalty);
            //Debug.Log("Hazard hit! Speed reduced.");
        }
    }







}
