using UnityEngine;
using UnityEngine.Events;

public class Crops : MonoBehaviour
{
    private CropData curCrop;
    [SerializeField] 
    private int plantDay; // only serialized for debugging purposes
    [SerializeField]
    private int daysSinceLastWatered; // this is only serialized for debugging purposes


    public SpriteRenderer sr;

    public static event UnityAction<CropData> onPlantCrop;
    public static event UnityAction<CropData> onHarvestCrop;


    public void Plant(CropData crop)
    {
        curCrop = crop;
        plantDay = GameManager.Instance.curDay;
        daysSinceLastWatered = 1;
        UpdateCropSprite();

        onPlantCrop?.Invoke(crop);
    }
    public void NewDayCheck()
    {
        daysSinceLastWatered++;
        if (daysSinceLastWatered > curCrop.waterPerDayBeforeDeath) // num of days till crop dies without being watered this is defined in cropdata
        {
            Destroy(gameObject);
        }
       UpdateCropSprite();
    }

    void UpdateCropSprite()
    {
        int cropProgress = CropProgress();
        if (cropProgress < curCrop.daysToGrow)
        {
            sr.sprite = curCrop.growProgressSprite[cropProgress];
        }
        else
        {
            sr.sprite = curCrop.readyToHarvestSprite;
        }

    }

    public void Water()
    {
        daysSinceLastWatered = 0;

    }
    public void Harvest()
    {
        if(CanHarvest())
        {
            onHarvestCrop?.Invoke(curCrop);
            Destroy(gameObject);
        }
    }
    int CropProgress()
    {
        if(daysSinceLastWatered == 0)
        {
            return GameManager.Instance.curDay - plantDay;
        }
        // Return last known progress instead of 0
        return Mathf.Max(0, GameManager.Instance.curDay - plantDay - daysSinceLastWatered);

    }

    public bool CanHarvest()
    {
        return CropProgress() >= curCrop.daysToGrow;
    }
}
