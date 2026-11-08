using UnityEngine;

public class ColorDropTarget : MonoBehaviour
{
    [Header("Box Color Info")]
    public ColorData boxColorData; 
    
    [SerializeField] private UnityEngine.UI.Image boxImage; 

    public void InitializeBox(ColorData colorData)
    {
        boxColorData = colorData;

        if (boxImage != null && boxColorData != null)
        {
            Color targetColor = boxColorData.boxColor;
            targetColor.a = 1f; 
        
            boxImage.color = targetColor;
        }
    }

    public bool CheckMatch(ColorData droppedItemColor, Transform droppedItemTransform)
    {
        if (droppedItemColor == null || boxColorData == null) return false;

        bool isMatch = (droppedItemColor == boxColorData);

        if (isMatch)
            SnapItemToBox(droppedItemTransform);
        return isMatch;
    }
    private void SnapItemToBox(Transform itemTransform)
    {
        if (itemTransform != null)
        {
            itemTransform.position = transform.position;
            itemTransform.localScale = Vector3.one * 0.8f;

            if (itemTransform.TryGetComponent<DraggableItem>(out var draggable))
            {
                draggable.enabled = false; 
            }
        }
    }
}