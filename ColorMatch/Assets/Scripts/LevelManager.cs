using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Game Configuration")]
    [SerializeField] private int minBoxCount = 2;
    [SerializeField] private int maxBoxCount = 3;

    [Header("References")]
    [SerializeField] private Transform boxesParent;
    [SerializeField] private Transform itemsParent;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject itemPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() => PrepareNewLevel();

    public void PrepareNewLevel()
    {
        var colorPool = GameManager.Instance?.allColorDatas;
        if (colorPool == null || colorPool.Count == 0)
        {
            Debug.LogError("Color pool is empty!");
            return;
        }

        ClearExistingObjects();

        int targetCount = Mathf.Min(maxBoxCount + 1, colorPool.Count + 1);
        int boxCount = Random.Range(minBoxCount, targetCount);
        var selectedColors = colorPool.OrderBy(_ => Random.value).Take(boxCount).ToList();

        SpawnBoxes(selectedColors);
        SpawnDraggableItems(selectedColors);
    }

    private void SpawnBoxes(List<ColorData> colors)
    {
        colors.ForEach(color => 
        {
            Instantiate(boxPrefab, boxesParent)
                .GetComponent<ColorDropTarget>()?.InitializeBox(color);
        });
    }

  private void SpawnDraggableItems(List<ColorData> colors)
  {
      int totalItemCount = 0;
  
      List<(ColorData colorData, Sprite sprite)> allSpawnableItems = new List<(ColorData, Sprite)>();
  
      foreach (var colorData in colors)
      {
          int maxAvailable = colorData.objectSprites?.Count ?? 0;
          if (maxAvailable == 0) continue;
  
          int spawnCount = Random.Range(1, Mathf.Min(3, maxAvailable + 1));
  
          var selectedSprites = colorData.objectSprites
              .OrderBy(_ => Random.value)
              .Take(spawnCount);
  
          foreach (var sprite in selectedSprites)
          {
              allSpawnableItems.Add((colorData, sprite));
          }
      }
  
      var shuffledItems = allSpawnableItems.OrderBy(_ => Random.value).ToList();
  
      foreach (var itemInfo in shuffledItems)
      {
          GameObject item = Instantiate(itemPrefab, itemsParent);
          if (item.TryGetComponent<DraggableItem>(out var draggable))
          {
              draggable.InitializeItem(itemInfo.colorData, itemInfo.sprite); 
              draggable.OnItemDropped += GameManager.Instance.ProcessItemDrop;
              totalItemCount++;
          }
      }
  
      GameManager.Instance.InitializeLevelProgress(totalItemCount);
  }

    private void ClearExistingObjects()
    {
        foreach (Transform child in boxesParent) Destroy(child.gameObject);
        foreach (Transform child in itemsParent) Destroy(child.gameObject);
    }
    

}