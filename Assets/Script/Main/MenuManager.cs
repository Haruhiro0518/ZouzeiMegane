using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using unityroom.Api;
using UnityEngine.Events;

// Menu UIの操作、ゲームオーバー・ゲームクリア時の処理をするクラス

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseButton, MenuUI, ResultUI, WaveGenerator, 
                        ScoreGUI, TopUI, BottomUI;
    [SerializeField]
    private TMPro.TMP_Text Tips, FinalState;

    [SerializeField] ClearParticle clearParticle;
    [SerializeField] AudioManager audioManager;
    
    private WaveGenerate waveGenerate;
    private Score scoreScript;
    private Player player;
    

    UnityEvent GameOverEvent = new UnityEvent();

    void Start()
    {
        waveGenerate = WaveGenerator.GetComponent<WaveGenerate>();
        scoreScript = ScoreGUI.GetComponent<Score>();
        player = GameObject.Find("Player").GetComponent<Player>();

        // event追加
        GameOverEvent.AddListener(OnGameOver);
    }
    
    void Update()
    {
        if(waveGenerate.IsGameOver==true || waveGenerate.IsGameClear==true)
        {
            GameOverEvent.Invoke();
        }
    }
    
    public void SelectPause()
    {
        if(MenuUI.activeSelf == false) {
            Time.timeScale = 0;
            MenuUI.SetActive(true);
        } else {
            this.SelectClose();
        }
    }

    public void SelectRetry()
    {
        Time.timeScale = 1;
        waveGenerate.IsGameOver = false;
        waveGenerate.IsGameClear = false;
        // Mainシーンを読み込む前にmainSourceリストをClearする必要がある. 読み込むたびにAddするため
        SettingManager.instance.mainSource.Clear(); 
        SceneManager.LoadScene("Main");
    }

    public void SelectRetire()
    {
        Time.timeScale = 1;
        waveGenerate.IsGameOver = false;
        SettingManager.instance.mainSource.Clear();
        SceneManager.LoadScene("Title");
    }

    public void SelectClose()
    {
        Time.timeScale = 1;
        MenuUI.SetActive(false);
        SettingManager.instance.SelectClose();
    }

    // GameOver時にCSVを読み込み、Tipsオブジェクトのテキストを変更する
    // このメソッドをUpdate()内で一度だけ呼び出すために、GameOverEventにこれを登録しておき
    // 最後にGameOverEventから削除する
    void OnGameOver()
    {
        CSVreader csvreader = gameObject.GetComponent<CSVreader>();
        csvreader.ReadCSV();
        csvreader.SetupText(Tips);
        
        GameOverEvent.RemoveListener(OnGameOver);

        if(waveGenerate.IsGameClear == true) {
            audioManager.Play_Popper();
            clearParticle.Play();
        }
        StartCoroutine(WaitForResult());
    }

    
    IEnumerator WaitForResult()
    {
        yield return new WaitForSeconds(3.0f);

        float send_score;
        Time.timeScale = 0;

        if(waveGenerate.IsGameOver == true) {
            send_score = scoreScript.score;
            SetFinalScore(scoreScript.score, 0f, scoreScript.score);
            FinalState.SetText("<size=55>＜解散＞</size>");
        } 
        else {
            // クリア時の最終スコアは   集めた税 + 支持者の数*5[億](寄付金)  とする
            send_score = scoreScript.score + (player.HP * 5);
            SetFinalScore(scoreScript.score, (player.HP * 5), send_score);
            FinalState.SetText("<size=55>＜任期満了！＞</size>");
        }
        
        PauseButton.SetActive(false);
        TopUI.SetActive(false);
        BottomUI.SetActive(false);
        ResultUI.SetActive(true);
        
        // スコア送信
        #if Unity_Room
        UnityroomApiClient.Instance.SendScore(1, send_score, ScoreboardWriteMode.HighScoreDesc);
        #endif
    }
    
    [SerializeField] TMPro.TMP_Text Tax_score, Donation_score, FinalScore;
    void SetFinalScore(float tax, float donation, float final)
    {
        Tax_score.SetText("<size=36>"+ tax.ToString() +"億</size>");
        Donation_score.SetText("<size=36>"+ donation.ToString() +"億</size>");
        FinalScore.SetText("<size=90>"+ final.ToString() +"億</size>");
    }
}
