using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Game Configuration")]
    [Tooltip("Min Box")]
    [SerializeField] private int minBoxCount = 2;

    [Tooltip("Max Box")]
    [SerializeField] private int maxBoxCount = 3;

    [Header("References")]
    [SerializeField] private Transform boxesParent;
    [SerializeField] private GameObject boxPrefab;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        PrepareNewLevel();
    }

    public void PrepareNewLevel()
    {
        List<ColorData> colorPool = GameManager.Instance != null ? GameManager.Instance.allColorDatas : null;
        
        int totalAvailableColors = colorPool != null ? colorPool.Count : 0;

        if (totalAvailableColors == 0)
        {
            Debug.LogError("List is empty");
            return;
        }

        int upperLimit = Mathf.Min(maxBoxCount + 1, totalAvailableColors + 1);
        int targetCount = Random.Range(minBoxCount, upperLimit);

        Debug.Log("Box count: " + targetCount);
        SpawnBoxes(targetCount, colorPool);
    }

    void SpawnBoxes(int count, List<ColorData> colorPool)
    {
        foreach (Transform child in boxesParent)
            Destroy(child.gameObject);

        for (int i = 0; i < count; i++)
        {
            GameObject newBox = Instantiate(boxPrefab, boxesParent);
            
            // ColorData assignedColor = colorPool[i]; 
            // newBox.GetComponent<BoxSetup>().Initialize(assignedColor);
        }
    }
}