using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySE : MonoBehaviour
{
    [SerializeField] AudioSource source;

    void Start()
    {
        
    }

    public void Play()
    {
        source.volume = SettingManager.instance.volume_se;
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}
