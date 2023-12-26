using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField, Header("オプションUI")]
    private GameObject OptionUI;

    [SerializeField, Header("スライダー")]
    private GameObject Slider;

    [SerializeField, Header("ヘルプUI")]
    private GameObject HelpUI;
    
    private AudioSource titleBGM;

    public static float volumeValue = 0.5f;

    private void Start()
    {
        titleBGM = GetComponent<AudioSource>();
        if(volumeValue != 0.5f)
        {
            titleBGM.volume = volumeValue;
        }
        titleBGM.Play();
    }

    private void Update()
    {
        if(titleBGM.time > 45.7f)
        {
            titleBGM.Stop();
            titleBGM.time = 0.5f;
            titleBGM.Play();
        }
    }

    public void SelectStart()
    {
        SceneManager.LoadScene("Intro");
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
        volumeValue = value;
        titleBGM.volume = value;
    }
}
