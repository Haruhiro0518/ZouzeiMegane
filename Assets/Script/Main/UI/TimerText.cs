using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text timerText;
    private WaveGenerate waveGenerate;
    [SerializeField] private int limitTime;
    // private static int seasonLength = limitTime / 12; // 12(任期3年×季節4つ)
    private static int seasonLength = 10; // 1シーズン10秒
    private static int yearLength = seasonLength * 4;
    [SerializeField]
    private float timecount = 0;
    private int year = 1;
    private string season = "春";

    [SerializeField] ChangeBgColor changeBgColor;
    [SerializeField] SeasonParticle seasonParticle;
    [SerializeField] ValueData valueData;
    

    void Start()
    {
        if (valueData.currentGameMode == ValueData.GameMode.Normal) {
            limitTime = 120; // Normalモードでは2分
        } else {
            limitTime = 99999; // Endlessモードでは時間制限なし
        }

        waveGenerate = GameObject.Find("WaveGenerator").GetComponent<WaveGenerate>();

        seasonParticle.Play(0, seasonLength);
    }

    void Update()
    {
        if(waveGenerate.IsGameOver == true) {
            timerText.SetText("<size=30>解散</size>");
            return;
        } if(timecount >= limitTime) {
            waveGenerate.IsGameClear = true;
            timerText.SetText("<size=30>任期満了！</size>");
            return;
        }
        
        timecount += Time.deltaTime;
        DecideYearandSeason();

        timerText.SetText("<size=30>"+year.ToString()+"年目："+season+"</size>");
    }

    int previous_y = 1, previous_s = 0;
int y, s;

void DecideYearandSeason()
{
    y = (int)(timecount / yearLength) + 1;

    if (y != previous_y)
    {
        previous_y = y;
        year = y; 
    }

    s = ((int)(timecount / seasonLength)) % 4;

    if (s != previous_s)
    {
        previous_s = s;
        OnSeasonChanged(s);
        seasonParticle.Play(s, seasonLength);
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
