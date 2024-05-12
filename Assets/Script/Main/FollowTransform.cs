using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] private Camera _objectCamera;

    [System.NonSerialized] public Transform _objectTransform;

    [SerializeField] private Transform _objectTrnasform_UI;

    // オブジェクト位置のオフセット
    public Vector3 _worldOffset;

    private RectTransform _parentUIRectTransform;

    
    

    // 初期化メソッド（Prefabから生成する時に使う）
    public void Initialize(Transform objectTransform, Camera objectCamera = null)
    {
        _objectTransform = objectTransform;
        _objectCamera = objectCamera == null ? Camera.main : objectCamera;
        
        OnUpdateUIPosition();
    }

    private void Awake()
    {
        if (_objectCamera == null) {
            _objectCamera = Camera.main;
        }
        _parentUIRectTransform = _objectTrnasform_UI.parent.GetComponent<RectTransform>();

    }

    private void FixedUpdate()
    {
        OnUpdateUIPosition();
    }

    private Vector3 objectWorldPos;
    private void OnUpdateUIPosition()
    {
        if(_objectTransform != null) {
            objectWorldPos = _objectTransform.position + _worldOffset;
        }
    
        // オブジェクトのワールド座標→スクリーン座標へ変換
        var objectScreenPos = _objectCamera.WorldToScreenPoint(objectWorldPos);

        // スクリーン座標→UIローカル座標へ変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUIRectTransform,
            objectScreenPos,
            null,
            out var UILocalPos
        );

        _objectTrnasform_UI.localPosition = UILocalPos;
        
    }
}