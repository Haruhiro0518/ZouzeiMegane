using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ScoreGUIオブジェクトで使用するクラス
// ScoreTextの表示更新を行う
public class Score : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text TextScore;
    [SerializeField] WaveGenerate waveGenerate;
    private Player player;

    [System.NonSerialized] public float score = 0f;
    [System.NonSerialized] public int scorerate = 10;

    void Start()
    {   
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        ChangeText((int)score);
    }

    public void ChangeText(int score)
    {
        string scoreText = score.ToString()+"<size=35>億</size>";
        TextScore.SetText(scoreText);
    }
    
    public void AddScore(float valueScored)
    {
        score += valueScored;
    }

    [SerializeField] TMPro.TMP_Text Tax_score, Donation_score, FinalScore;
    public int SetFinalScore()
    {
        // クリア時の最終スコアは   集めた税 + 支持者の数*5[億](寄付金)  とする
        int donation = 0;
        if(waveGenerate.IsGameClear == true) {
            donation = player.HP * 5 * scorerate;
        }
        int send_score = (int)(score + donation);

        
        
        Tax_score.SetText("<size=35>"+ score.ToString() +"億</size>");
        Donation_score.SetText("<size=35>"+ donation.ToString() +"億</size>");
        FinalScore.SetText("<size=90>"+ send_score.ToString() +"億</size>");

        return send_score;
    }    
}  
