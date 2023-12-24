using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private int state;
    
    private bool talking;

    [SerializeField, Header("総理UI")]
    private GameObject PlayerUI;

    [SerializeField, Header("秘書UI")]
    private GameObject NonPlayerUI;
    
    [SerializeField, Header("オプションUI")]
    private GameObject OptionUI;

    [SerializeField, Header("スライダー")]
    private GameObject Slider;

    [SerializeField, Header("総理アニメーター")]
    Animator PlayerAnimator;

    [SerializeField, Header("秘書アニメーター")]
    Animator NonPlayerAnimator;

    [SerializeField, Header("テキスト1")]
    private GameObject Text1;

    [SerializeField, Header("テキスト2")]
    private GameObject Text2;

    [SerializeField, Header("テキスト3")]
    private GameObject Text3;

    [SerializeField, Header("テキスト4")]
    private GameObject Text4;

    [SerializeField, Header("テキスト5")]
    private GameObject Text5;

    [SerializeField, Header("テキスト6")]
    private GameObject Text6;

    [SerializeField, Header("テキスト7")]
    private GameObject Text7;

    [SerializeField, Header("テキスト8")]
    private GameObject Text8;

    private AudioSource introBGM;
    
    void Start()
    {
        state = 0;
        talking = false;
        PlayerUI.SetActive(talking);
        NonPlayerUI.SetActive(!talking);
        PlayerAnimator.SetBool("talking", talking);
        NonPlayerAnimator.SetBool("talking", !talking);
        Text1.SetActive(true);
        introBGM = GetComponent<AudioSource>();
        introBGM.volume = TitleManager.volumeValue;
        introBGM.Play();
    }

    void Update()
    {
        if(introBGM.time > 156.9f)
        {
            introBGM.Stop();
            introBGM.time = 0.5f;
            introBGM.Play();
        }
    }

    public void SelectNext()
    {
        state += 1;
        talking = !talking;

        if(state > 7)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            PlayerUI.SetActive(talking);
            NonPlayerUI.SetActive(!talking);
            PlayerAnimator.SetBool("talking", talking);
            NonPlayerAnimator.SetBool("talking", !talking);

            switch(state)
            {
                case 1:
                {
                    Text1.SetActive(false);
                    Text2.SetActive(true);
                    break;
                }
                case 2:
                {
                    Text2.SetActive(false);
                    Text3.SetActive(true);
                    break;
                }
                case 3:
                {
                    Text3.SetActive(false);
                    Text4.SetActive(true);
                    break;
                }
                case 4:
                {
                    Text4.SetActive(false);
                    Text5.SetActive(true);
                    break;
                }
                case 5:
                {
                    Text5.SetActive(false);
                    Text6.SetActive(true);
                    break;
                }
                case 6:
                {
                    Text6.SetActive(false);
                    Text7.SetActive(true);
                    break;
                }
                case 7:
                {
                    Text7.SetActive(false);
                    Text8.SetActive(true);
                    break;
                }
            }
        }
    }

    public void SelectOption()
    {
        OptionUI.SetActive(true);
    }

    public void SelectSkip()
    {
        SceneManager.LoadScene("Main");
    }

    public void SelectClose()
    {
        OptionUI.SetActive(false);
    }

    public void MoveSlider(float value)
    {
        TitleManager.volumeValue = value;
        introBGM.volume = value;
    }
}
