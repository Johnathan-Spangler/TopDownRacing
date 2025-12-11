using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* Johnathan Spangler
 * 12/09/25
 * Manages the UI
 */

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI UI, lvl1Info, lvl2Info, lvl3Info;
    public GameObject menu, panel, track1, track2, track3;
    public string template1, template2, template3, template4, tempSecs, lastUp1, lastUp2, lvl1Time, lvl1Best, lvl2Time, lvl2Best, lvl3Time, lvl3Best;
    public bool lvl1, lvl2, lvl3;

    public RaceLogic raceLogic;
    public RacerMovement racerMovement;

    private int best1Seconds = int.MaxValue;
    private int best2Seconds = int.MaxValue;
    private int best3Seconds = int.MaxValue;

    // Start is called before the first frame update
    void Start()
    {
        template1 = UI.text;
        template2 = lvl1Info.text;
        template3 = lvl2Info.text;
        template4 = lvl3Info.text;
        lvl1Best = "N/A";
        lvl2Best = "N/A";
        lvl3Best = "N/A";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMenu();
        UpdateUI();
    }

    /// <summary>
    /// Update the player's UI
    /// </summary>
    public void UpdateUI()
    {
        //Timer
        if (raceLogic.lapSecs < 10)
        {
            tempSecs = "0" + raceLogic.lapSecs;
        }
        else
        {
            tempSecs = raceLogic.lapSecs.ToString("00");
        }

        if (raceLogic.lapNumber > 0)
        {
            lastUp2 = raceLogic.lapNumber.ToString("0");
        }
        else 
        {
            lastUp2 = "0";
        }

        lastUp1 = (raceLogic.lapMins + ":" + tempSecs);

        UI.text = string.Format(template1, lastUp1, lastUp2);

        //Hide start message
        if (!racerMovement.brakeLocked)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }
    }

    /// <summary>
    /// Format time for usability
    /// </summary>
    /// <param name="totalSeconds"></param>
    /// <returns></returns>
    private string FormatTime(int totalSeconds)
    {
        if (totalSeconds <= 0) return "N/A";
        int mins = totalSeconds / 60;
        int secs = totalSeconds % 60;
        return $"{mins}:{secs:00}";
    }

    /// <summary>
    /// Updates the Menu and resets player on menu open
    /// </summary>
    public void UpdateMenu()
    {
        //Time set
        int currentSeconds = raceLogic.lapMins * 60 + raceLogic.lapSecs;

        //Open menu, reset player, and set high score on level end
        if (raceLogic.lapNumber >= 3)
        {
            if (lvl1 && currentSeconds > 0 && currentSeconds < best1Seconds)
            {
                best1Seconds = currentSeconds;
                lvl1Best = FormatTime(best1Seconds);
            }
            if (lvl2 && currentSeconds > 0 && currentSeconds < best2Seconds)
            {
                best2Seconds = currentSeconds;
                lvl2Best = FormatTime(best2Seconds);
            }
            if (lvl3 && currentSeconds > 0 && currentSeconds < best3Seconds)
            {
                best3Seconds = currentSeconds;
                lvl3Best = FormatTime(best3Seconds);
            }

            menu.SetActive(true);

            // Stop and reset the timer properly
            raceLogic.StartCoroutine(raceLogic.StopAndResetTimer());

            // Reset the character
            racerMovement.SetBrakeLock(true);
            racerMovement.braking = true;
            transform.position = Vector3.zero;
            racerMovement.ResetRotationImmediate();
            raceLogic.lapNumber = -1;
        }

        //Update menu text
        lvl1Info.text = string.Format(template2, lvl1Best);
        lvl2Info.text = string.Format(template3, lvl2Best);
        lvl3Info.text = string.Format(template4, lvl3Best);
    }

    /// <summary>
    /// Changes current level to level 1
    /// </summary>
    public void Level1()
    {
        raceLogic.lapped = false;
        lvl1 = true;
        lvl2 = false;
        lvl3 = false;
        menu.SetActive(false);
        track1.SetActive(true);
        track2.SetActive(false);
        track3.SetActive(false);
    }

    /// <summary>
    /// Changes current level to level 2
    /// </summary>
    public void Level2()
    {
        raceLogic.lapped = false;
        lvl1 = false;
        lvl2 = true;
        lvl3 = false;
        menu.SetActive(false);
        track1.SetActive(false);
        track2.SetActive(true);
        track3.SetActive(false);
    }

    /// <summary>
    /// Changes current level to level 3
    /// </summary>
    public void Level3()
    {
        raceLogic.lapped = false;
        lvl1 = false;
        lvl2 = false;
        lvl3 = true;
        menu.SetActive(false);
        track1.SetActive(false);
        track2.SetActive(false);
        track3.SetActive(true);
    }

    /// <summary>
    /// Quits the game (and exits editor play mode)
    /// </summary>
    public void Quit()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
