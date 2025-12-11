using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedBoost = 7f;

    private void OnTriggerEnter(Collider other)
    {
        RacerMovement racer = other.GetComponentInParent<RacerMovement>();

        if (racer != null)
        {
            racer.currentSpeed = Mathf.Max(0f, racer.currentSpeed + speedBoost);
            //Debug.Log("panel hit! Speed increased!");
        }
    }
}
