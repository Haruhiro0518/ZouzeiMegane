using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Assets->Create->ScriptableObj->Create ValueData でインスタンス化して使用する

[CreateAssetMenu(menuName = "ScriptableObj/Create ValueData")]
public class ValueData : ScriptableObject
{
    // Itemに関するパラメータ処理
    public int maxItemHP;
    public int minItemHP;
    public int ItemHPCoefficient;

    public void ChangeItemHPminmax(float rate)
    {
        if (rate == 0)
        {
            if (ItemHPCoefficient == 1)
            {
                SetItemHPminmax(8, 12);
            }
            else
            {
                SetItemHPminmax(-2, -1);
            }
        }
        else if (rate == 0.5)
        {
            if (ItemHPCoefficient == 1)
            {
                SetItemHPminmax(6, 10);
            }
            else
            {
                SetItemHPminmax(-3, -2);
            }
        }
        else if (rate == 1.5)
        {
            if (ItemHPCoefficient == 1)
            {
                SetItemHPminmax(1, 2);
            }
            else
            {
                SetItemHPminmax(-12, -8);
            }
        }
        else
        {
            if (ItemHPCoefficient == 1)
            {
                SetItemHPminmax(4, 7);
            }
            else
            {
                SetItemHPminmax(-4, -3);
            }
        }
    }

    private void SetItemHPminmax(int min, int max)
    {
        minItemHP = min;
        maxItemHP = max;
    }

    // WaveRandomがwaveを生成するとき、それぞれのオブジェクトを生成する確率
    public float percentBlockAndItem = 45;
    public float percentBlock = 60;
    public float percentBar = 20;

    // Blockに関するパラメータ処理
    public int GlassesPercent = 20;

    //  HP分布の初期値 1-4:20% | 5-9:30% | 10-19:30% | 20-29:15% | 30-51:5%  
    public int hpRange1to4 = 20;
    public int hpRange5to9 = 50;
    public int hpRange10to19 = 80;
    public int hpRange20to29 = 95;

    public void ChangeBlockHPDistribution(float rate)
    {
        if (rate == 0)
        {
            // 1-4:80% | 5-9:20% | 10-19:0% | 20-29:0% | 30-51:0%
            SetHPRange(80, 100, 0, 0);
        }
        else if (rate == 0.5)
        {
            // 1-4:60% | 5-9:35% | 10-19:5% | 20-29:0% | 30-51:0%
            SetHPRange(60, 95, 100, 0);
        }
        else if (rate == 1.0)
        {
            // 1-4:20% | 5-9:30% | 10-19:30% | 20-29:15% | 30-51:5%
            SetHPRange(20, 50, 80, 95);
        }
        else
        {
            // 1-4:0% | 5-9:0% | 10-19:5% | 20-29:35% | 30-51:65%
            SetHPRange(0, 0, 5, 40);
        }
    }
    private void SetHPRange(int r1to4, int r5to9, int r10to19, int r20ro29)
    {
        hpRange1to4 = r1to4;
        hpRange5to9 = r5to9;
        hpRange10to19 = r10to19;
        hpRange20to29 = r20ro29;
        return;
    }

    // Game Mode に関するパラメータ
    public enum GameMode
    {
        Normal,
        Endless
    }
    public GameMode currentGameMode;

}
