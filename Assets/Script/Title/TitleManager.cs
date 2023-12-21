using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    
    [SerializeField, Header("スタートボタン")]
    private GameObject StartButton;

    [SerializeField, Header("オプションボタン")]
    private GameObject OptionButton;

    [SerializeField, Header("オプションUI")]
    private GameObject OptionUI;

    [SerializeField, Header("スライダー")]
    private GameObject Slider;
    
    private AudioSource bgm;

    private void Start()
    {
        bgm = GetComponent<AudioSource>();
        bgm.Play();
    }

    void Update()
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
    }

    public void SelectBack()
    {
        OptionUI.SetActive(false);
    }

    public void MoveSlider(float value)
    {
        bgm.volume = value;
    }
}
