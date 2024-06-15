using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ScoreGUIオブジェクトで使用するクラス
// ScoreTextの表示更新を行う
public class Score : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text TextScore;

    [System.NonSerialized] public float score = 0f;

    void Start()
    {

    }

    void Update()
    {
        ChangeText((int)score);
    }

    public void ChangeText(int score)
    {
        string scoreText = score.ToString()+"<size=50>億</size>";
        TextScore.SetText(scoreText);
    }
    
    public void AddScore(int valueScored, float taxRate)
    {
        score += (float)valueScored * taxRate;
    }

}  
