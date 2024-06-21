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
    private TMPro.TMP_Text Tips;

    [SerializeField] ClearParticle clearParticle;
    [SerializeField] AudioManager audioManager;
    [SerializeField] Result Result;
    
    private WaveGenerate waveGenerate;
    private Score Score;
    private Player player;
    

    UnityEvent GameOverEvent = new UnityEvent();

    void Start()
    {
        waveGenerate = WaveGenerator.GetComponent<WaveGenerate>();
        Score = ScoreGUI.GetComponent<Score>();
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

        int send_score;
        Time.timeScale = 0;

        send_score = Score.SetFinalScore();
        Result.SetFinalState();
        
        PauseButton.SetActive(false);
        TopUI.SetActive(false);
        BottomUI.SetActive(false);
        ResultUI.SetActive(true);
        
        // スコア送信
        #if Unity_Room
        UnityroomApiClient.Instance.SendScore(1, send_score, ScoreboardWriteMode.HighScoreDesc);
        #endif
    }

}
