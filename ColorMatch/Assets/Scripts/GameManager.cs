using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Data Pool")]
    public List<ColorData> allColorDatas;
    
    [Header("Box Item Settings")]
    [SerializeField] private float itemScale = 0.8f;
    [SerializeField] private float itemOffsetX = 25f;

    private int targetBoxCount;
    private int matchedItemCount;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void InitializeLevelProgress(int boxCount)
    {
        targetBoxCount = boxCount;
        matchedItemCount = 0;

        if (UIManager.Instance != null)
            UIManager.Instance.HideWinPopup();
    }

    public void ProcessItemDrop(DraggableItem droppedItem)
    {
        if (droppedItem == null) return;

        ColorDropTarget targetBox = FindDropTargetUnderMouse();

        if (targetBox == null || !targetBox.CheckMatch(droppedItem.ItemColorData))
        {
            droppedItem.ReturnToStart();
            return;
        }

        PlaceItemIntoBox(droppedItem, targetBox);
        HandleSuccessfulMatch(droppedItem);
    }

    private void PlaceItemIntoBox(DraggableItem droppedItem, ColorDropTarget targetBox)
    {
        Transform itemTransform = droppedItem.transform;
        
        // Nesneyi kutunun içine taşıyoruz
        itemTransform.SetParent(targetBox.ItemDropPoint, false);
        itemTransform.localScale = Vector3.one * itemScale;

        if (itemTransform is RectTransform itemRect)
        {
            // Kutunun içerisindeki mevcut sıra numarasına göre sağa veya sola konumlandırıyoruz
            int itemIndex = targetBox.ItemDropPoint.childCount - 1;
            float targetX = (itemIndex == 0) ? -itemOffsetX : itemOffsetX;

            itemRect.anchoredPosition = new Vector2(targetX, 0f);
            itemRect.localRotation = Quaternion.identity;
        }

        // Sürükleme etkileşimini kapatıyoruz
        if (droppedItem.TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            canvasGroup.blocksRaycasts = false;
        }
        
        droppedItem.enabled = false;
    }
    private void HandleSuccessfulMatch(DraggableItem matchedItem)
    {
        matchedItem.OnItemDropped -= ProcessItemDrop;
        matchedItemCount++;

        if (matchedItemCount >= targetBoxCount)
            TriggerWinState();
    }

    private void TriggerWinState()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.ShowWinPopup();
    }

    public void RestartGame()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.HideWinPopup();

        if (LevelManager.Instance != null)
            LevelManager.Instance.PrepareNewLevel();
    }

    private ColorDropTarget FindDropTargetUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
            if (result.gameObject.TryGetComponent<ColorDropTarget>(out var dropTarget))
            {
                return dropTarget;
            }
        return null;
    }
}