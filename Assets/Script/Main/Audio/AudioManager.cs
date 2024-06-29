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
    AudioClip TaxRateUp, TaxRateDown, ignoreTaxArea, Cracker, Smoke;

    void Start()
    {
        main_bgm.volume = SettingManager.instance.volume_bgm;
        TaxArea.volume = SettingManager.instance.volume_bgm;
        main_bgm.Play();
        // SettingManager側からmain_bgmのAudioSource.volumeを変更するため、参照を渡す
        SettingManager.instance.mainSource.Add(main_bgm);
    }

    void Updata()
    {
        if(main_bgm.time > 143.0f)
        {
            main_bgm.Stop();
            main_bgm.time = 0.48f;
            PlayBGM(main_bgm);
        }
    }
    
    void PlayBGM(AudioSource source)
    {
        source.volume = SettingManager.instance.volume_bgm;
        source.Play();
    }
    void PlaySE(AudioSource source, AudioClip clip)
    {
        source.volume = SettingManager.instance.volume_se;
        source.PlayOneShot(clip);
    }


    public void Play_ignoreTaxArea()
    {
        PlaySE(TaxArea, ignoreTaxArea);
    }
    // TaxRateUpDonwの再生はTaxRateTextオブジェクト(UIキャンバス内)から呼び出される
    public void Play_TaxRateUp()
    {
        PlaySE(TaxArea, TaxRateUp);
    }

    public void Play_TaxRateDown()
    {
        PlaySE(TaxArea, TaxRateDown);
    }

    public void Play_Cracker()
    {
        // TaxArea関係の音源と同時に鳴らさないため、同じAudioSourceを使用
        PlaySE(TaxArea, Cracker);
    }

    public void Play_Smoke()
    {
        PlaySE(TaxArea, Smoke);
    }
}