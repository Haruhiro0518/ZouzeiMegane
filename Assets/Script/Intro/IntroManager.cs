using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private int state;
    
    private bool talking;

    [SerializeField, Header("オプションUI")]
    private GameObject OptionUI;

    [SerializeField, Header("スライダー")]
    private GameObject Slider;

    [SerializeField, Header("総理アニメーター")]
    Animator PlayerAnimator;

    [SerializeField, Header("秘書アニメーター")]
    Animator NonPlayerAnimator;

    private AudioSource introBGM;
    
    void Start()
    {
        state = 0;
        talking = true;
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

        if(state > 3)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            PlayerAnimator.SetBool("talking", talking);
            NonPlayerAnimator.SetBool("talking", !talking);
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
