using UnityEngine;

public class ColorDropTarget : MonoBehaviour
{
    [Header("Box Color Info")]
    [SerializeField] private ColorData boxColorData;
    public ColorData BoxColorData => boxColorData;

    [SerializeField] private UnityEngine.UI.Image boxImage;
    [SerializeField] private Transform itemDropPoint;

    public Transform ItemDropPoint => itemDropPoint != null ? itemDropPoint : transform;

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

    public bool CheckMatch(ColorData droppedItemColor)
    {
        if (droppedItemColor == null || boxColorData == null) return false;
        return droppedItemColor == boxColorData;
    }
}