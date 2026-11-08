using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DraggableItem : MonoBehaviour
{
    [Header("Item Color Info")]
    [SerializeField] private ColorData itemColorData;

    public event Action<DraggableItem> OnItemDropped;

    private Vector3 startPosition;
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 mouseOffset;
    public ColorData ItemColorData => itemColorData;
    void Start()
    {
        mainCamera = Camera.main;
        startPosition = transform.position; 
    }
    
    void OnMouseDown()
    {
        isDragging = true;
        mouseOffset = transform.position - GetMouseWorldPosition();
        transform.localScale = Vector3.one * 1.15f;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        transform.position = GetMouseWorldPosition() + mouseOffset;
    }

    void OnMouseUp()
    {
        isDragging = false;
        transform.localScale = Vector3.one;

        OnItemDropped?.Invoke(this);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }

    public void ReturnToStart()
    {
        transform.position = startPosition;
    }
    public void InitializeItem(ColorData colorData, Sprite targetSprite)
    {
        itemColorData = colorData;

        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.sprite = targetSprite;
            spriteRenderer.color = Color.white; 
        }
        
        if (TryGetComponent<UnityEngine.UI.Image>(out var uiImage))
        {
            uiImage.sprite = targetSprite;
            
            Color imgColor = uiImage.color;
            imgColor.a = 1f;
            uiImage.color = imgColor;
        }
    }
    private void OnDestroy()
    {
        OnItemDropped = null; 
    }
}