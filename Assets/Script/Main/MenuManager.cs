using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [SerializeField, Header("ステージオブジェクト")] 
    private GameObject Stage;

    private StageScript stageScript;
    
    private AudioSource MainBGM;

    void Start()
    {
        stageScript = Stage.GetComponent<StageScript>();
        MainBGM = GetComponent<AudioSource>();
        MainBGM.volume = TitleManager.volumeValue;
        MainBGM.Play();
    }
    
    void Update()
    {
        if(MainBGM.time > 143.0f)
        {
            MainBGM.Stop();
            MainBGM.time = 0.48f;
            MainBGM.Play();
        }

        if(stageScript.IsGameover == true)
        {
            Time.timeScale = 0;
            PauseButton.SetActive(false);
            ResultUI.SetActive(true);
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
        stageScript.IsGameover = false;
        SceneManager.LoadScene("Main");
    }

    public void SelectRetire()
    {
        Time.timeScale = 1;
        stageScript.IsGameover = false;
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


}
