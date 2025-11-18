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
    public float slowAmount = 0.5f;
    public float slowDuration = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        RacerMovement racer = collision.gameObject.GetComponent<RacerMovement>();
        if (racer)
        {
           // racer.ApplySlow(slowAmount, slowDuration);

        }
    }



    // reduces maxSpeed temporarily
    public void ApplySlow(float slowAmount, float duration)
    {
       // currentSpeed = Mathf.Max(currentSpeed * slowAmount, 0f);
    }






}
