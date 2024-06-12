using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : MonoBehaviour
{
    SettingManager settingManager;

    void Start()
    {
        settingManager = GameObject.Find("Setting").GetComponent<SettingManager>();
    }

    public void OnPressed()
    {
        settingManager.SelectOpen();
    }
    
}
