using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [HideInInspector] public int minutes;
    [HideInInspector] public int seconds;

    private float timer;

    void Start()
    {
        minutes = 0;
        seconds = 0;
        timer = 0;
    }

    void FixedUpdate()
    {
        if(minutes <= 60) {
            timer += Time.deltaTime;
            minutes = (int)((timer / 60f));
            seconds = (int)(timer % 60f);
        }
    }

    public void ResetTimer() {
        timer = 0;
        minutes = 0;
        seconds = 0;
    }

    public string GetString() {
        string time = "";
        if (minutes <= 9) {
            time += "0" + minutes;
        } else {
            time += minutes;
        }

        time += " : ";

        if (seconds <= 9) {
            time += "0" + seconds;
        } else {
            time += seconds;
        }

        return time;
    }
}
