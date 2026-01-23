using UnityEngine;

[CreateAssetMenu(fileName = "Crop Data", menuName = "ScriptableObjects/Crop Data")]
public class CropData : ScriptableObject
{
   public int daysToGrow;
    public Sprite[] growProgressSprite;
    public Sprite readyToHarvestSprite;

    public int waterPerDayBeforeDeath = 2;

    public int purchasePrice;
    public int sellPrice;
}
 