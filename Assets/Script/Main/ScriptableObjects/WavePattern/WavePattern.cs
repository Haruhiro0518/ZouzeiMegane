using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class WavePattern : ScriptableObject 
{
	public WaveType type;
    // これを各パターンで override する
    public abstract void Generate(Transform parent, ManageWave manageWave); //, ItemLibrary library); // , ValueData data);
}