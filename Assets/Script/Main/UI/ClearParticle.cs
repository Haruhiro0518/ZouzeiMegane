using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearParticle : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> seasonParticle = new List<ParticleSystem>(); 
    public void Play()
    {
        foreach(ParticleSystem p in seasonParticle) {
            p.Play();
        }
    }
    
}
