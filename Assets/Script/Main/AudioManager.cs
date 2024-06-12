using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブジェクトがDestroyされても効果音を鳴らすことが出来る
// breakBlockは複数のAudioSourceが必要なため、SE(money)プレハブをインスタンス化する

public class AudioManager : MonoBehaviour
{
    public AudioSource main_bgm;
    [SerializeField]
    AudioSource TaxArea;
    [SerializeField]
    AudioClip TaxRateUp, TaxRateDown, ignoreTaxArea;
    [SerializeField]
    float TaxArea_offset, main_bgm_offset;

    void Start()
    {
        PlayBGM(main_bgm, main_bgm_offset);
        // SettingManager側からmain_bgmのAudioSource.volumeを変更するため、参照を渡す
        SettingManager.instance.mainSource.Add(main_bgm);
    }

    void Updata()
    {
        if(main_bgm.time > 143.0f)
        {
            main_bgm.Stop();
            main_bgm.time = 0.48f;
            PlayBGM(main_bgm, main_bgm_offset);
        }
    }
    
    void PlayBGM(AudioSource source, float offset)
    {
        if(SettingManager.instance.volume_bgm > 0f) {
            source.volume = SettingManager.instance.volume_bgm + offset;
        } else {
            source.volume = 0f;
        }
        source.Play();
    }
    void PlaySE(AudioSource source, AudioClip clip, float offset)
    {
        if(SettingManager.instance.volume_se > 0f) {
            source.volume = SettingManager.instance.volume_se + offset;
        } else {
            source.volume = 0f;
        }
        source.PlayOneShot(clip);
    }


    public void Play_ignoreTaxArea()
    {
        PlaySE(TaxArea, ignoreTaxArea, TaxArea_offset);
    }
    // TaxRateUpDonwの再生はTaxRateTextオブジェクト(UIキャンバス内)から呼び出される
    public void Play_TaxRateUp()
    {
        PlaySE(TaxArea, TaxRateUp, TaxArea_offset);
    }

    public void Play_TaxRateDown()
    {
        PlaySE(TaxArea, TaxRateDown, TaxArea_offset);
    }


}