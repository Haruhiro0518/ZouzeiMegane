using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text timerText;
    private WaveGenerate waveGenerate;
    private static int limitTime = 120;
    private static int seasonLength = limitTime / 12; // 12(任期3年×季節4つ)
    private static int yearLength = seasonLength * 4;
    [SerializeField]
    private float timecount = 0;
    private int year = 1;
    private string season = "春";

    [SerializeField] ChangeBgColor changeBgColor;
    

    void Start()
    {
        waveGenerate = GameObject.Find("WaveGenerator").GetComponent<WaveGenerate>();
    }


    void Update()
    {
        if(waveGenerate.IsGameOver == true) {
            timerText.SetText("<size=40>解散</size>");
            return;
        } if(timecount >= 120) {
            waveGenerate.IsGameClear = true;
            timerText.SetText("<size=40>任期満了！</size>");
            return;
        }
        
        timecount += Time.deltaTime;
        DecideYearandSeason();

        timerText.SetText("<size=40>"+year.ToString()+"年目："+season+"</size>");
    }

    void DecideYearandSeason()
    {
        int pre_y = 1, pre_s = 0, y, s;
        // (int)は小数切り捨て
        y = (int)(timecount / yearLength) + 1;
        if(y != pre_y) {
            pre_y = y;
            switch (y) {
            case 1:
                year = 1;
                break;
            case 2:
                year = 2;
                break;
            case 3:
                year = 3;
                break;
            }
        }
        

        s = ((int)(timecount / seasonLength)) % 4;
        if(s != pre_s) {
            pre_s = s;
            OnSeasonChanged(s);
        }
    }

    void OnSeasonChanged(int s)
    {
        switch (s) {
            case 0:
                season = "春";
                break;
            case 1:
                season = "夏";
                break;
            case 2:
                season = "秋";
                break;
            case 3:
                season = "冬";
                break;
        }

        changeBgColor.ChangeColor(s);
    }
}
