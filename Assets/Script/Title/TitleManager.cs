using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField, Header("スタートボタン")]
    private GameObject StartButton;

    [SerializeField, Header("オプションボタン")]
    private GameObject OptionButton;

    [SerializeField, Header("オプションUI")]
    private GameObject OptionUI;
    
    //private AudioSource bgm;

    /*private void Start()
    {
        bgm = GetComponent<AudioSource>();
        bgm.Play();
    }

    void Update()
    {
        if(bgm.time > 86.3f)
        {
            bgm.Stop();
            bgm.time = 0.7f;
            bgm.Play();
        }
    }*/

    
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
}
