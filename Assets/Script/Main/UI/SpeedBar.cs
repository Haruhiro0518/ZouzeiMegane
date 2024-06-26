using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    private Player player;
    private Slider slider;
    private Animator animator;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }

    public void SpeedUpTextFX()
    {
        animator = player.SpeedUpTextAnimator;
        animator.SetTrigger("speedup");
    }
}
