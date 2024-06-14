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

    [SerializeField] GameObject slider_bgm, slider_se, slider_sens, close_button, bg_canvas;
    [SerializeField] TextSensitivity textSensitivity;
    [System.NonSerialized] public float volume_bgm = 0.5f;
    [System.NonSerialized] public float volume_se = 0.5f;
    [System.NonSerialized] public float dragSensitivity = 0f;

    public void SelectClose()
    {
        // gameObject.SetActive(false);
        gameObject.GetComponent<Image>().enabled = false;
        slider_bgm.SetActive(false);
        slider_se.SetActive(false);
        slider_sens.SetActive(false);
        close_button.SetActive(false);
        bg_canvas.SetActive(false);
    }

    public void SelectOpen()
    {
        gameObject.GetComponent<Image>().enabled = true;
        slider_bgm.SetActive(true);
        slider_se.SetActive(true);
        slider_sens.SetActive(true);
        close_button.SetActive(true);
        bg_canvas.SetActive(true);
    }

    [System.NonSerialized] public AudioSource titleSource, introSource; 
    [System.NonSerialized] public List<AudioSource> mainSource = new List<AudioSource>(); 

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

    // 5段階の値をとるドラッグ感度のスライダー
    public void MoveSlider_sens(float value)
    {
        float v;
        if(value < 0.25f) {
            v = 0f;
        } else if(value >= 0.25f && value < 0.5f) {
            v = 0.25f;
        } else if(value >= 0.5f && value < 0.75f) {
            v = 0.5f;
        } else if(value >= 0.75f && value < 1.0f) {
            v = 0.75f;
        } else {
            v = 1f;
        }
        textSensitivity.SetText(v);
        slider_sens.GetComponent<Slider>().value = v;
        dragSensitivity = v;

        string scenename = SceneManager.GetActiveScene().name;
        if(scenename == "Main") {
            GameObject.Find("Player").GetComponent<DragPlayer>().sensitivity 
                                                        = dragSensitivity;
        }
    }
}
