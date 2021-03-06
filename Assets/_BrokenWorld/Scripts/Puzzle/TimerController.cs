using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    private TextMeshProUGUI timeCounter;

    public GameController gameController;

    private TimeSpan timePlaying;
    private bool timerGoing;

    private float elapsedTime;
    private string timeLimit;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeCounter = CanvasElementsObjectScript.playerCanvas.timerText;

        Debug.Log(PlayerSettingsData.instance.timerValue);
        if (PlayerSettingsData.instance.timerValue != 0)
        {
            // timeLimit = getTimeFormat(PlayerSettingsData.instance.timerValue);
            //Debug.Log(timeLimit);

            timeCounter.text = "Time: 00:00:00";
            timerGoing = false;
            BeginTimer();
        }
        else
        {
            timeCounter.text = "";
        }
    }

    private String GetTimeFormat(int timer_value)
    {
        TimeSpan t = TimeSpan.FromSeconds(timer_value);
        string answer = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Minutes, t.Seconds, t.Milliseconds);

        return answer;
    }

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = PlayerSettingsData.instance.timerValue;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime -= Time.deltaTime;
            //Debug.Log(elapsedTime);
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            //Debug.Log(timePlaying);

            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timePlayingStr;

            if (elapsedTime <= 0)
            {
                EndTimer();
                gameController.LoseAction();

            }
            yield return null;
        }
    }
}
