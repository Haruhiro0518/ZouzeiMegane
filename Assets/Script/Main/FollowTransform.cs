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
    // [SerializeField] private Vector3 _worldOffset;
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

    private void OnUpdateUIPosition()
    {
        var objectWorldPos = _objectTransform.position + _worldOffset;
    
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

/* 
        <2Dゲームのため、「オブジェクトがカメラの後ろに位置するか」の判定は行わない>
        var cameraTransform = _objectCamera.transform;
        
        var cameraDir = cameraTransform.forward;

        // カメラから対象オブジェクトへのベクトル
        var objectDir = objectWorldPos - cameraTransform.position;
        // 内積を使ってカメラ前方かどうかを判定
        var isFront = Vector3.Dot(cameraDir, objectDir) > 0;
        // カメラ前方ならUI表示、後方なら非表示
        _uiObjectTransform.gameObject.SetActive(isFront);
        if (!isFront) return;

        */