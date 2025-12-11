using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/* Johnathan Spangler
 * 12/09/25
 * Controls the logic of the timer and lap number
 */

public class RaceLogic : MonoBehaviour
{
    public int lapNumber, lapMins, lapSecs;
    private float timer = 0f;
    public bool lapped;

    public GameObject startObject;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lapNumber = -1;
        lapped = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Set the Start/Finish line object
        if (startObject == null)
        {
            var starts = GameObject.FindGameObjectsWithTag("Start");
            foreach (var s in starts)
            {
                if (s.activeInHierarchy)
                {
                    startObject = s;
                    break;
                }
            }
        }
        //Set timer logic
        if (lapped)
        {
            timer += Time.deltaTime;
        }
        while (timer >= 1f)
        {
            timer -= 1f;
            lapSecs++;
            if (lapSecs >= 60)
            {
                lapMins++;
                lapSecs = 0;
            }
        }
    }

    /// <summary>
    /// Reset timer that waits a moment to ensure reset happens after best time is logged
    /// </summary>
    public IEnumerator StopAndResetTimer()
    {
        yield return new WaitForSeconds(0.1f);
        lapped = false;
        timer = 0f;
        lapSecs = 0;
        lapMins = 0;
    }

    /// <summary>
    /// Control Laps with the finish line
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (lapped)
        {
            startObject.SetActive(true);
        }
        if (other.tag == "Start")
        {
            other.gameObject.SetActive(false);
            lapped = false;
        }
        if (other.tag == "End" && !lapped)
        {
            lapNumber++;
            lapped = true;
        }
    }
}