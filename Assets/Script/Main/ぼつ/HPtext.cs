using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPtext : MonoBehaviour
{
    private float width;
    public float offsetY;
    public GUIStyle style;
    public Transform target;
    private RectTransform targetUI;

    public Camera cam;

    void start()
    {
        cam = GetComponent<Camera>();
        width = gameObject.transform.localScale.x / 2;
    }
    void OnGUI()
    {
        Vector3 ScreenPos = cam.WorldToScreenPoint(target.position);
        // Vector3 ScreenPos = cam.main.ViewportToScreenPoint(target.position);
        Debug.Log(ScreenPos);
        GUI.Label(new Rect(ScreenPos.x, ScreenPos.y, 200, 200), "50", style);
    }


    


}
