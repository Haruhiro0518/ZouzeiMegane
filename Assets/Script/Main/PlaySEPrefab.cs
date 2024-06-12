using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Prefab/SEオブジェクトにアタッチ。SE再生後、Destoryする

public class PlaySEPrefab : MonoBehaviour
{
    private AudioSource myAudioSource;
    [SerializeField] private float volume_offset;

    IEnumerator Start()
    {
        
        myAudioSource = gameObject.GetComponent<AudioSource>();
        
        float length = myAudioSource.clip.length;

        myAudioSource.volume = SettingManager.instance.volume_se;
        
        if(myAudioSource.volume > 0.0f)
        {
            myAudioSource.volume += volume_offset;
        }

        myAudioSource.Play();
        // SE再生終了を待つ 
        yield return new WaitForSeconds(length);
        
        Destroy(gameObject);
    }

}
