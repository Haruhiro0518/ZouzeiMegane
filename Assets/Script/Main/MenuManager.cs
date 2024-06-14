using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using unityroom.Api;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    [SerializeField, Header("ポーズボタン")]
    private GameObject PauseButton;
    
    [SerializeField, Header("メニューUI")]
    private GameObject MenuUI;

    [SerializeField, Header("リザルトUI")]
    private GameObject ResultUI;

    [SerializeField, Header("ウェーブ生成オブジェクト")] 
    private GameObject WaveGenerator;

    private WaveGenerate waveGenerate;

    [SerializeField, Header("スコアオブジェクト")] 
    private GameObject ScoreGUI;

    [SerializeField, Header("上部UI")] 
    private GameObject TopUI;

    [SerializeField, Header("下部UI")] 
    private GameObject ButtomUI;

    [SerializeField, Header("リザルトコメント")]
    private TMPro.TMP_Text Tips;

    [SerializeField, Header("最終状態テキスト")]
    private TMPro.TMP_Text FinalState;

    private Score scoreScript;

    UnityEvent GameOverEvent = new UnityEvent();

    void Start()
    {
        waveGenerate = WaveGenerator.GetComponent<WaveGenerate>();
        scoreScript = ScoreGUI.GetComponent<Score>();

        // event追加
        GameOverEvent.AddListener(OnGameOver);
    }
    
    void Update()
    {
        
        if(waveGenerate.IsGameOver==true || waveGenerate.IsGameClear==true)
        {
            GameOverEvent.Invoke();
            
            #if Unity_Room
            UnityroomApiClient.Instance.SendScore(1, scoreScript.score, ScoreboardWriteMode.HighScoreDesc);
            #endif

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
        SettingManager.instance.mainSource.Clear(); // Addする前にClearを読んでおく
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
        
        StartCoroutine(WaitForResult());
        GameOverEvent.RemoveListener(OnGameOver);
    }
    IEnumerator WaitForResult()
    {
        yield return new WaitForSeconds(2.0f);
        Time.timeScale = 0;
        PauseButton.SetActive(false);
        TopUI.SetActive(false);
        ButtomUI.SetActive(false);
        ResultUI.SetActive(true);

        if(waveGenerate.IsGameOver==true) {
            FinalState.SetText("<size=55>解散</size>");
        } else if(waveGenerate.IsGameClear==true) {
            FinalState.SetText("<size=55>任期満了！</size>");
        }
    }
}
