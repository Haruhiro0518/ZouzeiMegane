using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave/Tax")]
public class TaxWavePattern : WavePattern
{
	float scale_y = 3.3f;
	public override void Generate(Transform parent, ManageWave mw)
	{
		// TaxAreaを右と左に配置する。どちらが増税か減税かはランダムで決定。
		if(Random.Range(0,2) == 0) {
            mw.InstantiateTaxArea(1);
        } else {
            mw.InstantiateTaxArea(-1);
        }
		mw.InstantiateBar(0, scale_y);
	}
}
