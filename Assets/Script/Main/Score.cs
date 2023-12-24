using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // text
    public GameObject ScoreUI;
    private TMPro.TMP_Text TextScore;

    // score格納
    public float score = 0f;

    void Start()
    {
        // gameObjectのTMPro取得
        TextScore = ScoreUI.GetComponent<TMPro.TMP_Text>();
    }

    void Update()
    {
        ChangeText((int)score);
    }

    public void ChangeText(int score)
    {
        Debug.Log(score);
        string scoreText = score.ToString();
        TextScore.SetText(scoreText);
    }
    
    public void AddScore(int power, float taxRate)
    {
        
        score += (float)power * taxRate;
    }
    
}
