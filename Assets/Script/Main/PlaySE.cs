using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySE : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] private float volume_offset;

    void Start()
    {
        
    }

    public void Play()
    {
        source.volume = SettingManager.instance.volume_se + volume_offset;
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}
