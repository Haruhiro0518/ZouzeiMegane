using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ScoreGUIオブジェクトで使用するクラス
// ScoreTextの表示更新を行う
public class Score : MonoBehaviour
{
    public GameObject ScoreUI;
    private TMPro.TMP_Text TextScore;

    public GameObject ResultUI;
    private TMPro.TMP_Text TextResult;

    public float score = 0f;

    void Start()
    {
        TextScore = ScoreUI.GetComponent<TMPro.TMP_Text>();
        TextResult = ResultUI.GetComponent<TMPro.TMP_Text>();
    }

    void Update()
    {
        ChangeText((int)score);
    }

    public void ChangeText(int score)
    {
        string scoreText = score.ToString()+"<size=50>億</size>";
        TextScore.SetText(scoreText);
        scoreText = score.ToString()+"<size=90>億</size>";
        TextResult.SetText(scoreText);
    }
    
    public void AddScore(int valueScored, float taxRate)
    {
        
        score += (float)valueScored * taxRate;
    }
    
}
