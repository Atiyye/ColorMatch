using UnityEngine;

[RequireComponent(typeof(Collider2D))] 
public class DraggableItem : MonoBehaviour
{
    private Vector3 startPosition;
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 mouseOffset;

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
        if (isDragging)
            transform.position = GetMouseWorldPosition() + mouseOffset;
    }

    void OnMouseUp()
    {
        isDragging = false;
        transform.localScale = Vector3.one;

       // CheckDropTarget();
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
}