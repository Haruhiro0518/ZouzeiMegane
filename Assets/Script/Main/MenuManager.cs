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
    
    [SerializeField, Header("ポーズUI")]
    private GameObject PauseUI;
    
    [SerializeField, Header("オプションUI")]
    private GameObject OptionUI;

    [SerializeField, Header("スライダー")]
    private GameObject Slider;

    [SerializeField, Header("ヘルプUI")]
    private GameObject HelpUI;

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
    
    private AudioSource MainBGM;

    UnityEvent GameOverEvent = new UnityEvent();

    void Start()
    {
        waveGenerate = WaveGenerator.GetComponent<WaveGenerate>();
        scoreScript = ScoreGUI.GetComponent<Score>();
        MainBGM = GetComponent<AudioSource>();
        MainBGM.volume = TitleManager.volumeValue;
        MainBGM.Play();

        // event追加
        GameOverEvent.AddListener(OnGameOver);
    }
    
    void Update()
    {
        if(MainBGM.time > 143.0f)
        {
            MainBGM.Stop();
            MainBGM.time = 0.48f;
            MainBGM.Play();
        }

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
        Time.timeScale = 0;
        PauseUI.SetActive(true);
        OptionUI.SetActive(false);
        HelpUI.SetActive(false);
    }

    public void SelectOption()
    {
        PauseUI.SetActive(false);
        OptionUI.SetActive(true);
        HelpUI.SetActive(false);
    }

    public void SelectHelp()
    {
        PauseUI.SetActive(false);
        OptionUI.SetActive(false);
        HelpUI.SetActive(true);
    }

    public void SelectRetry()
    {
        Time.timeScale = 1;
        waveGenerate.IsGameOver = false;
        waveGenerate.IsGameClear = false;
        SceneManager.LoadScene("Main");
    }

    public void SelectRetire()
    {
        Time.timeScale = 1;
        waveGenerate.IsGameOver = false;
        SceneManager.LoadScene("Title");
    }

    public void SelectClose()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(false);
        OptionUI.SetActive(false);
        HelpUI.SetActive(false);
    }

    public void MoveSlider(float value)
    {
        TitleManager.volumeValue = value;
        MainBGM.volume = value;
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
