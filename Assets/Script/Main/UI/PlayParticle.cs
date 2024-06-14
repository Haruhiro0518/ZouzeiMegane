using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] 
    ParticleSystem sakuraParticle, leafParticle, mapleParticle, snowParticle;
    
    public void Play(int season, int seasonLength)
    {
        float duration;
        switch(season) {
            case 0: // 春
                duration = sakuraParticle.main.duration;
                duration = (float)seasonLength;
                sakuraParticle.Play();
                break;
            case 1: // 夏
                duration = leafParticle.main.duration;
                duration = (float)seasonLength;
                leafParticle.Play();
                break;
            case 2: // 秋
                duration = mapleParticle.main.duration;
                duration = (float)seasonLength;
                mapleParticle.Play();
                break;
            case 3: // 冬
                duration = snowParticle.main.duration;
                duration = (float)seasonLength;
                snowParticle.Play();
                break;
        }
    }
}
