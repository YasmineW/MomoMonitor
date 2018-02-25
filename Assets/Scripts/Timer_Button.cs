﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer_Button : MonoBehaviour {

    public Text textTitle, textTime;
    public string titleTotal, titleTiming,saveStartTime;

    private  bool timing = false;
    private int intTiming;
    private Timer timer = new Timer();
    private TimeSpan duration;
    private TimeSpan totalDuration;

    // Use this for initialization
    void Start () {

        if (PlayerPrefs.HasKey(titleTiming))
        {
            timing = Convert.ToBoolean(PlayerPrefs.GetInt(titleTiming));

            if (timing)
            {
                //Grab the last start time from the player prefs as a long
                long temp = Convert.ToInt64(PlayerPrefs.GetString(saveStartTime));
                //Convert the last start time from binary to a DataTime variable
                timer.StartTime = DateTime.FromBinary(temp);
            }
        }

        if (PlayerPrefs.HasKey(titleTotal))
        totalDuration = TimeSpan.Parse(PlayerPrefs.GetString(titleTotal));

        textTitle.text = titleTotal;
        textTime.text = FormatTimeSpan(totalDuration);
    }

    public void Click()
    {

        if (timing)
        {
            timer.EndTime = DateTime.Now;

            textTitle.text = titleTotal;
            totalDuration += duration;
            //save total duration into PlayerPrefs
            PlayerPrefs.SetString(titleTotal, totalDuration.ToString());


            textTime.text = FormatTimeSpan(totalDuration);
            timing = false;
            //save is timing or not into PlayerPrefs
            intTiming = Convert.ToInt32(timing);
            PlayerPrefs.SetInt(titleTiming, intTiming);

        }
        else
        {
            timer.StartTime = DateTime.Now;
            
            //Save the start time as a string in the player prefs class
            PlayerPrefs.SetString(saveStartTime, timer.StartTime.ToBinary().ToString());

            textTitle.text = titleTiming;
            timing = true;
            //save is timing or not into PlayerPrefs
            intTiming = Convert.ToInt32(timing);
            PlayerPrefs.SetInt(titleTiming, intTiming);
        }


    }

    string FormatTimeSpan(TimeSpan timeSpan)
    {
        string h, m, s;


        if (timeSpan.Hours == 0)
        {
            h = "";
            s = timeSpan.Seconds + "s";
        }
        else
        {
            h = timeSpan.Hours + "h";
            s = "";
        }


        if (timeSpan.Minutes == 0)
        {
            m = "";
            s = timeSpan.Seconds + "s";
        }
        else
            m = timeSpan.Minutes + "m";

        if (timeSpan.Seconds == 0)
            s = "";

        string hms = h + m + s;
        return hms;
    }


    // Update is called once per frame
    void Update()
    {

        if (timing)
        {
            duration = DateTime.Now.Subtract(timer.StartTime);
            textTime.text = FormatTimeSpan (duration);
        }
    }

    //private void OnApplicationQuit()
    //{
    //    //save is timing or not into PlayerPrefs
    //    intTiming = Convert.ToInt32(timing);
    //    PlayerPrefs.SetInt(titleTiming, intTiming);

    //    //save total duration into PlayerPrefs
    //    PlayerPrefs.SetString(titleTotal, totalDuration.ToString());

    //}
}

public class Timer
{

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public TimeSpan CalculateDuration()
    {
        TimeSpan duration = EndTime.Subtract(StartTime);
        return duration;
    }
}
