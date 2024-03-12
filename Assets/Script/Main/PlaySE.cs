using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySE : MonoBehaviour
{
    private AudioSource myAudioSource;

    IEnumerator Start()
    {
        
        myAudioSource = gameObject.GetComponent<AudioSource>();
        
        float length = myAudioSource.clip.length;

        myAudioSource.volume = TitleManager.volumeValue;
        if(myAudioSource.volume > 0.0f)
        {
            myAudioSource.volume += 0.2f;
        }

        // SE再生終了を待つ ただしTimeScaleが0の場合はここで処理が止まる
        yield return new WaitForSeconds(length);
        // 再生終了後、オブジェクト削除
        Destroy(gameObject);
    }

}
