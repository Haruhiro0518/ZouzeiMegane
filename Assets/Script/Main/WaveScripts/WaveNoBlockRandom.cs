using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveNoBlockRandom : MonoBehaviour
{
    void Awake()
    {
        gameObject.GetComponent<WaveRandom>().IsNoBlock = true;
    }
}
