using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{
    public float slowAmount = 0.5f;
    public float slowDuration = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        RacerMovement racer = collision.gameObject.GetComponent<RacerMovement>();
        if (racer)
        {
            racer.ApplySlow(slowAmount, slowDuration);

        }
    }



    public void ApplySlow(float slowAmount, float duration)
    {
        // Example: cut speed and maybe reduce maxSpeed temporarily
        currentSpeed = Mathf.Max(currentSpeed * slowAmount, 0.5f);









    }
