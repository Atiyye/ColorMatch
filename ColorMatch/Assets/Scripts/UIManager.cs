using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject winPopupPanel;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowWinPopup()
    {
        winPopupPanel?.SetActive(true);
    }

    public void HideWinPopup()
    {
        winPopupPanel?.SetActive(false);
    }
}