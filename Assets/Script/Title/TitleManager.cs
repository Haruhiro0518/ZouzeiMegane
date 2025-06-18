using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private AudioSource titleBGM;
    [SerializeField] private ValueData valueData;

    private void Start()
    {
        titleBGM = GetComponent<AudioSource>();
        titleBGM.volume = SettingManager.instance.volume_bgm;
        titleBGM.Play();
        // SettingManager側からtitleBGMのAudioSource.volumeを変更するため、参照を渡す
        SettingManager.instance.titleSource = titleBGM;
    }

    private void Update()
    {
        if (titleBGM.time > 45.7f)
        {
            titleBGM.Stop();
            titleBGM.time = 0.5f;
            titleBGM.Play();
        }
    }

    public void SelectStart(ValueData.GameMode gameMode)
    {
        valueData.currentGameMode = gameMode;
        SettingManager.instance.SelectClose();
        SceneManager.LoadScene("Intro");
    }
    public void OnNormalStartButtonClick()
    {
        SelectStart(ValueData.GameMode.Normal);
    }

    public void OnEndlessStartButtonClick()
    {
        SelectStart(ValueData.GameMode.Endless);
    }

}