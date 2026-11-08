using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Data Pool")]
    public List<ColorData> allColorDatas; 

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
        {
            Destroy(gameObject); 
        }
    }

    public void InitializeLevelProgress(int boxCount)
    {
        targetBoxCount = boxCount;
        matchedItemCount = 0;
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideWinPopup();
        }
    }

    public void ProcessItemDrop(DraggableItem droppedItem)
    {
        if (droppedItem == null) return;

        ColorDropTarget targetBox = FindDropTargetUnderMouse();

        if (targetBox == null || !targetBox.CheckMatch(droppedItem.ItemColorData, droppedItem.transform))
        {
            droppedItem.ReturnToStart();
            return;
        }

        HandleSuccessfulMatch();
    }

    private void HandleSuccessfulMatch()
    {
        matchedItemCount++;

        if (matchedItemCount >= targetBoxCount)
        {
            TriggerWinState();
        }
    }

    private void TriggerWinState()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowWinPopup();
        }
    }

    public void RestartGame()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideWinPopup();
        }
        
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.PrepareNewLevel();
        }
    }

    private ColorDropTarget FindDropTargetUnderMouse()
    {
        Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

        return hit.collider != null && hit.collider.TryGetComponent<ColorDropTarget>(out var dropTarget) 
            ? dropTarget 
            : null;
    }
}