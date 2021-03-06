﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Calendar : MonoBehaviour {
    //variable of panel_Canlendar
    public Text month;
    public List<GameObject> days;
    public GameObject calendarDayPrefab, gameObjectDays, panelLogs;
    public GameObject buttonBackToToday, buttonSeeAllLogs;

    private DateTime dateTime_babyBirth;
    private int  monthTemp, daysInMonths, yearTemp, firstDayIndex;
    private string babyBirth, weekOfFirstDay;

    List<Log> logs = new List<Log>();    

    public void Start()
    {
        BackToCertainDay(DateTime.Today);
        gameObjectDays.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height / 10 * 6);
        //print(Screen.width);
        gameObjectDays.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Screen.width / 7, Screen.height / 10);
    }

    public void OnEnable()
    {        
        logs = Main_Menu.menu.logList;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameObject.activeInHierarchy)Application.Quit();
        }
    }

    public void BackToCertainDay(DateTime date)
    {
        babyBirth = PlayerPrefs.GetString("babyBirth");
        OnEnable();
        //conver birthday formate from string to datetime
        if (babyBirth != "")
            dateTime_babyBirth = DateTime.ParseExact(babyBirth,
                "ddMMyyyy HHmm",
                CultureInfo.InvariantCulture, DateTimeStyles.None);
        else dateTime_babyBirth = new DateTime(1900, 02, 02);//if no DOB input，set the year of birth at 1900，and hide special age 
        SetDays(date.Year,date.Month);
        monthTemp = date.Month;
        yearTemp = date.Year;
    }

    public int ChangeWeekIntoNum(string week)
    {
        int num = 0;
        if(week == DayOfWeek.Sunday.ToString())num = 0;
        if (week == DayOfWeek.Monday.ToString()) num = 1;
        if (week == DayOfWeek.Tuesday.ToString()) num = 2;
        if (week == DayOfWeek.Wednesday.ToString()) num = 3;
        if (week == DayOfWeek.Thursday.ToString()) num = 4;
        if (week == DayOfWeek.Friday.ToString()) num = 5;
        else if(week == DayOfWeek.Saturday.ToString()) num = 6;
        return num;
    }   
  
    public void ToNextMonth() //button click ToNextMonth
    {
        if (monthTemp == 12)
        {
            yearTemp++;
            monthTemp = 1;
        }
        else monthTemp++;
        SetDays(yearTemp, monthTemp);
    }

    public void ToPreviousMonth() //button click ToPreviousMonth
    {
        if (monthTemp == 1)
        {
            yearTemp--;
            monthTemp = 12;
        }
        else monthTemp--;
        SetDays(yearTemp, monthTemp);
    }

    public void ToNextYear()
    {
        yearTemp++;
        SetDays(yearTemp, monthTemp);
    }

    public void ToPreviousYear()
    {
        yearTemp--;
        SetDays(yearTemp, monthTemp);
    }

    public void SeeLogsByTypes(GameObject logTypes)
    {
        panelLogs.SetActive(true);
        if (logTypes.name == "Button_SeeAllLogs") panelLogs.GetComponent<Panel_Logs>().ShowLogsByType("all");
        else if (logTypes.name == "Button_SeeAllNotes") panelLogs.GetComponent<Panel_Logs>().ShowLogsByType("note");
        else if (logTypes.name == "Button_SeeAllWeights") panelLogs.GetComponent<Panel_Logs>().ShowLogsByType("weight");
        else if (logTypes.name == "Button_SeeAllHeights") panelLogs.GetComponent<Panel_Logs>().ShowLogsByType("height");
    }
    public void SetDays(int thisYear, int thisMonth)
    {
        int daysInThisMonths, firstDayIndex, lastMonth, lastYear, nextMonth, nextYear; //lastMonth, lastYear are the inputs of last month in calendar; nextMonth, nextYear are the input of the next month
        daysInThisMonths = DateTime.DaysInMonth(thisYear, thisMonth);
        weekOfFirstDay = new DateTime(thisYear, thisMonth, 1).DayOfWeek.ToString();
        firstDayIndex = ChangeWeekIntoNum(weekOfFirstDay);
        if (thisYear != DateTime.Now.Year) month.text = new DateTime(thisYear, thisMonth, 1).ToString("MMMM", new CultureInfo("en-us")) //if it's not the month in current year, show the year after month
                  + " " + thisYear;
        else month.text = new DateTime(thisYear, thisMonth, 1).ToString("MMMM", new CultureInfo("en-us")); //change month into characters
        if (thisMonth == 1)  //set inputs of last month. When this month is Jan, last month is Dec, year -1.
        {
            lastYear = thisYear - 1;
            lastMonth = 12;
        }
        else
        {
            lastMonth = thisMonth - 1;
            lastYear = thisYear;
        }
        if (thisMonth == 12) //set inputs of next month. When this month is Dec, next month is Jan, year +1
        {
            nextYear = thisYear + 1;
            nextMonth = 1;
        }
        else
        {
            nextMonth = thisMonth + 1;
            nextYear = thisYear;
        }
        int j = DateTime.DaysInMonth(lastYear, lastMonth) - firstDayIndex + 1;
        int j2 = 1;
        int j3 = 1;
        if (days != null)
        {            
            foreach (GameObject days in days) Destroy(days);
            days.Clear();
        }
        GameObject gob;
        for (int i = 0; i < 42; i++)
        {
            gob = Instantiate(calendarDayPrefab, gameObjectDays.transform);
            days.Add(gob);
            if (i <= firstDayIndex - 1) //fullfill last couple days of last month
            {
                gob.GetComponent<Button_CalendarDay>().ShowDate(lastYear, lastMonth, j, thisMonth, dateTime_babyBirth, panelLogs);
                ShowLogTag(lastYear, lastMonth, j, gob);
                j++;
            }
            
            if (i >= firstDayIndex && i < daysInThisMonths + firstDayIndex) //fullfill days in current month, j is the day
            {
                gob.GetComponent<Button_CalendarDay>().ShowDate(thisYear, thisMonth, j2, thisMonth, dateTime_babyBirth, panelLogs);
                ShowLogTag(thisYear, thisMonth, j2, gob);
                if (new DateTime(thisYear, thisMonth, j2) == DateTime.Today) days[i].gameObject.GetComponent<Image>().color = new  Color(0.784f, 0.784f, 0.784f);
                j2++;
            }
            if (i >= daysInThisMonths + firstDayIndex) //fullfill the first couple days of next month
            {
                gob.GetComponent<Button_CalendarDay>().ShowDate(nextYear, nextMonth, j3, thisMonth, dateTime_babyBirth, panelLogs);
                ShowLogTag(nextYear, nextMonth, j3, gob);                
                j3++;
            }
        }
    }

    public void ShowLogTag(int year, int month, int day,GameObject buttonDay)
    {
        for (int i = 0; i < logs.Count; i++)
        {
            if (logs[i].Date.Date == new DateTime(year,month, day).Date)
            {
                buttonDay.GetComponent<Button_CalendarDay>().SetLogTag();
            }
        }
    }
}
