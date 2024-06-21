using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBgColor : MonoBehaviour
{
    private Camera _camera;

    Color SpringBgColor = new Color32(221, 131, 135, 0); // 桜色
    Color SummerBgColor = new Color32(0, 76, 113, 0);    // 藍色
    Color FallBgColor = new Color32(175, 92, 46, 0);    // 秋らしい色
    Color WinterBgColor = new Color32(161, 163, 166, 0); // 銀灰色

    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        _camera.backgroundColor = SpringBgColor;
    }

    public void ChangeColor(int seasonnum)
    {
        switch (seasonnum) {
            case 0:
                _camera.backgroundColor = SpringBgColor;
                break;
            case 1:
                _camera.backgroundColor = SummerBgColor;
                break;
            case 2:
                _camera.backgroundColor = FallBgColor;
                break;
            case 3:
                _camera.backgroundColor = WinterBgColor;
                break;
        }
    }
}
