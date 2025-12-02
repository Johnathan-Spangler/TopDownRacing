using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceLogic : MonoBehaviour
{
    public float lapNumber/*, */;
    public float lapTime;
    public bool lapped;

    public GameObject[] lapObject;

    // Start is called before the first frame update
    void Start()
    {
        lapNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (lapped)
        {
            lapObject = GameObject.FindGameObjectsWithTag("Start");
            //lapObject[0].SetActive = true;
        }
        if (other.tag == "Start")
        {
            lapNumber++;
            other.gameObject.SetActive(false);
        }

        if (other.tag == "End")
        {
            lapped = true;
        }
    }
}
