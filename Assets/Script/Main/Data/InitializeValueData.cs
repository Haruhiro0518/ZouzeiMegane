using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObjectインスタンスであるvalueDataの値を、ゲーム開始時に初期化する
// このスクリプトはWaveGeneratorオブジェクトにアタッチする

public class InitializeValueData : MonoBehaviour
{
    [SerializeField] ValueData data;
    private float OriginalTaxRate = 1.0f;

    void Awake()
    {
        data.ItemHPCoefficient = 1;
        data.ChangeItemHPminmax(OriginalTaxRate);
        data.ChangeBlockHPDistribution(OriginalTaxRate);
    }

}
