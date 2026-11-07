using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewColorData", menuName = "Game/Color Data")]
public class ColorData : ScriptableObject
{
    public string colorName;
    public Color boxColor;
    public List<Sprite> objectSprites;
    
}