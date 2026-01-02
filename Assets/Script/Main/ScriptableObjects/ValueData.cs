using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: アイテムとブロックの設定、ゲームモードの値を別のSOに分ける.
//		特に、アイテムは型を一つ用意しておき、そこからアイテムごとにアセットを作り分けるべき

// Assets->Create->ScriptableObj->Create ValueData でインスタンス化して使用する
[CreateAssetMenu(menuName = "ScriptableObj/Create ValueData")]
public class ValueData : ScriptableObject
{
	[SerializeField] private float _ishibaSpawnChance;
	public float IshibaSpawnChance => _ishibaSpawnChance;
    // peopleItemに関するパラメータ処理
    public int maxItemHP;
    public int minItemHP;
	/// <summary>
	/// 税率とプレイヤーが無敵かどうかで、パラメータ変更
	/// アイテムのHP上限下限、ブロックのHP確率分布が対象
	/// </summary>
	/// <param name="rate"></param>
	/// <param name="isInv"></param>
	public void UpdateParamsByTaxRate(float rate, bool isInv)
	{
		ChangeItemHPminmax(rate, isInv);
		ChangeBlockHPDistribution(rate);
	}

	public void ChangeItemHPminmax(float rate, bool isInv)
	{
		// タプル (rate, isinv) を使ってパターンマッチング。switch式
		(int min, int max) = (rate, isInv) switch
		{
			// 通常. 150% より大きくなることがない
			(0f, false)   => (8, 12),
			(0.5f, false)  => (6, 10),
			(1.0f, false) => (4, 7),
			(1.5f, false)  => (1, 2),

			// 無敵. 200% が上限で、マイナスになる
			(0f, true)  => (-2, -1),
			(0.5f, true) => (-3, -2),
			(1.0f, true) => (-4, -3),
			(1.5f, true) => (-7, -8),
			(2.0f, true) => (-12, -9),
			_             => isInv ? (0, 0) : (0, 0) // default(未定義)
		};

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
            SetBlockHPRate(80, 100, 0, 0);
        }
        else if (rate == 0.5)
        {
            // 1-4:60% | 5-9:35% | 10-19:5% | 20-29:0% | 30-51:0%
            SetBlockHPRate(60, 95, 100, 0);
        }
        else if (rate == 1.0)
        {
            // 1-4:20% | 5-9:30% | 10-19:30% | 20-29:15% | 30-51:5%
            SetBlockHPRate(20, 50, 80, 95);
        }
        else
        {
            // 1-4:0% | 5-9:0% | 10-19:5% | 20-29:35% | 30-51:65%
            SetBlockHPRate(0, 0, 5, 40);
        }
    }
    private void SetBlockHPRate(int r1to4, int r5to9, int r10to19, int r20ro29)
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
