using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ドラッグ感度の数値を表示するクラス
public class TextSensitivity : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text sensText;

    public void SetText(float value) 
    {
        // 表示例　「×1.5」, 「×1.25」. valueは0~1だから, 1 + valueとする
        sensText.SetText("<size=35>"+(1 + value).ToString()+"</size>");
    }
}
