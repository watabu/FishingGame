using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//https://qiita.com/toRisouP/items/3619ad3d2b7d785968f1

public class AutoScrollList : MonoBehaviour
{
    [Header("Scroll")]
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _contentTransform;
    [SerializeField] private RectTransform _viewportRectransform;
    [Header("Debug")]
    [SerializeField, ReadOnly] private GameObject currentGameObject;
    [SerializeField, ReadOnly] private float currentTop;
    [SerializeField, ReadOnly] private float currentBottom;
    [SerializeField, ReadOnly] private float centerPosition;
    [SerializeField, ReadOnly] private float topPosition;
    [SerializeField, ReadOnly] private float bottomPosition;
    [SerializeField, ReadOnly] private float viewportSize;
    [SerializeField, ReadOnly] private float contentSize;
    [SerializeField, ReadOnly] private float parentY;
    /// <summary>
    /// 自動スクロール
    /// </summary>
    public void Scroll()
    {
        currentGameObject = EventSystem.current.currentSelectedGameObject;
        parentY = currentGameObject.transform.parent.position.y;
        RectTransform current = (currentGameObject.transform) as RectTransform;
        currentTop = current.offsetMin.y;
        currentBottom = current.offsetMax.y;

        //現在のスクロール範囲の数値を計算しやすい様に上下反転
        var p = 1.0f - _scrollRect.verticalNormalizedPosition;

        //描画範囲のサイズ
        viewportSize = _viewportRectransform.rect.height;
        //描画範囲のサイズの半分
        var harlViewport = viewportSize * 0.5f;

        contentSize = _contentTransform.sizeDelta.y;

        //現在の描画範囲の中心座標
        centerPosition = -(contentSize - viewportSize) * p - harlViewport;
        //現在の描画範囲の上端座標
        topPosition = centerPosition - harlViewport;
        //現在の現在描画の下端座標
        bottomPosition = centerPosition + harlViewport;

        //選択した要素が上側にはみ出ている
        if (currentTop < topPosition)
        {
            //選択要素が描画範囲に収まるようにスクロール
            _scrollRect.verticalNormalizedPosition -= 0.01f;
            return;
        }

        //選択した要素が下側にはみ出ている
        if (currentBottom > bottomPosition)
        {
            _scrollRect.verticalNormalizedPosition += 0.01f;
        }
    }
}
