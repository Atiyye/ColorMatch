using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Image))]
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ColorData itemColorData;
    public ColorData ItemColorData => itemColorData;

    public event Action<DraggableItem> OnItemDropped;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform startParent;
    private int startSiblingIndex;

    public void InitializeItem(ColorData colorData, Sprite targetSprite)
    {
        itemColorData = colorData;

        if (TryGetComponent<Image>(out var uiImage))
        {
            uiImage.sprite = targetSprite;
            Color imgColor = uiImage.color;
            imgColor.a = 1f;
            uiImage.color = imgColor;
        }
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        startSiblingIndex = transform.GetSiblingIndex(); 
        
        if (startParent.TryGetComponent<LayoutGroup>(out var layoutGroup))
            layoutGroup.enabled = false;

        transform.SetParent(canvas.transform, true);
        
        canvasGroup.blocksRaycasts = false;
        transform.localScale = Vector3.one * 1.15f; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.localScale = Vector3.one;

        if (transform.parent == canvas.transform && startParent != null)
        {
            if (startParent.TryGetComponent<LayoutGroup>(out var layoutGroup))
            {
                layoutGroup.enabled = true;
            }
        }
        OnItemDropped?.Invoke(this);
    }

    public void ReturnToStart()
    {
        transform.SetParent(startParent, true);
        transform.SetSiblingIndex(startSiblingIndex);
        
        if (startParent.TryGetComponent<LayoutGroup>(out var layoutGroup))
        {
            layoutGroup.enabled = true;
            if (startParent is RectTransform parentRect)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
            }
        }
    }
}