using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField, Header("ポーズUI")]
    private GameObject PauseUI;
    
    [SerializeField, Header("オプションUI")]
    private GameObject OptionUI;

    [SerializeField, Header("スライダー")]
    private GameObject Slider;

    [SerializeField, Header("ヘルプUI")]
    private GameObject HelpUI;
    
    private AudioSource bgm;

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
        SceneManager.LoadScene("Main");
    }

    public void SelectRetire()
    {
        Time.timeScale = 1;
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
        //bgm.volume = value;
    }
}
