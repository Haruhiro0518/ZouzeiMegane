using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [SerializeField, Header("オプションUI")]
    private GameObject OptionUI;

    [SerializeField, Header("スライダー")]
    private GameObject Slider;

    [SerializeField, Header("ヘルプUI")]
    private GameObject HelpUI;
    
    private AudioSource bgm;

    private void Start()
    {
        bgm = GetComponent<AudioSource>();
        bgm.Play();
    }

    private void Update()
    {
        if(bgm.time > 45.7f)
        {
            bgm.Stop();
            bgm.time = 0.5f;
            bgm.Play();
        }
    }

    public void SelectStart()
    {
        SceneManager.LoadScene("Main");
    }
    
    public void SelectOption()
    {
        OptionUI.SetActive(true);
        HelpUI.SetActive(false);
    }

    public void SelectHelp()
    {
        OptionUI.SetActive(false);
        HelpUI.SetActive(true);
    }

    public void SelectClose()
    {
        OptionUI.SetActive(false);
        HelpUI.SetActive(false);
    }

    public void MoveSlider(float value)
    {
        bgm.volume = value;
    }
}
