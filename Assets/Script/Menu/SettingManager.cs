using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Singleton and DontDestroyOnLoad
// Titleシーンから遷移しなくても、シーン毎にSettingCanvasプレハブを配置すれば、
// シーン単体のテストが可能。ビルド時はTitleシーンだけに配置する

public class SettingManager : MonoBehaviour
{

    public static SettingManager instance; 
    public AudioSource titleSource, introSource; 
    public List<AudioSource> mainSource = new List<AudioSource>(); 
    // シングルトン
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject.transform.parent);
    }

    public float volume_bgm = 1f;
    public float volume_se = 1f;
    [SerializeField] GameObject slider_bgm, slider_se, close_button;

    public void SelectClose()
    {
        // gameObject.SetActive(false);
        gameObject.GetComponent<Image>().enabled = false;
        slider_bgm.SetActive(false);
        slider_se.SetActive(false);
        close_button.SetActive(false);
    }

    public void SelectOpen()
    {
        gameObject.GetComponent<Image>().enabled = true;
        slider_bgm.SetActive(true);
        slider_se.SetActive(true);
        close_button.SetActive(true);
    }

    public void MoveSlider_bgm(float value)
    {
        volume_bgm = value;
        // sliderがActiveでないときは変更しない
        if(slider_bgm != null) {
            slider_bgm.GetComponent<Slider>().value = volume_bgm;
        }

        // scene毎にBGMを再生するAudioSourceへの参照を持ち
        // volume_bgmが変更されたら、各AudioSourceのvolumeを変更する
        string scenename = SceneManager.GetActiveScene().name;
        switch(scenename) {
            case "Title":
                titleSource.volume = volume_bgm;
                break;
            case "Intro":
                introSource.volume = volume_bgm;
                break;
            case "Main":
                foreach(AudioSource a in mainSource) {
                    a.volume = volume_bgm;
                }
                break;
        }
    }

    public void MoveSlider_se(float value)
    {
        volume_se = value;
        if(slider_bgm != null) {
            slider_se.GetComponent<Slider>().value = volume_se;
        }
    }
}
