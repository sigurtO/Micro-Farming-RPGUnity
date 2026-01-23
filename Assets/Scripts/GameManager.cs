using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int curDay;
    public int money;
    public int cropInventory;

    public CropData selectedCrop;

    public TextMeshProUGUI statsText;

    public event UnityAction onNewDay;

    //Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject); //make sure there's only one instance
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateStatsText(); // update ui on game start

    }
    private void OnEnable()
    {
        Crops.onPlantCrop += OnPlantCrop;
        Crops.onHarvestCrop += OnHarvestCrop;
    }
    private void OnDisable()
    {
        Crops.onPlantCrop -= OnPlantCrop;
        Crops.onHarvestCrop -= OnHarvestCrop;
    }

    public void SetNextDay()
    {
        curDay++;
        onNewDay?.Invoke();
        UpdateStatsText();
    }

    public void OnPlantCrop(CropData cropData)
    {
        cropInventory--;
        UpdateStatsText();
    }

    public void OnHarvestCrop(CropData crop)
    {
        money += crop.sellPrice;
        UpdateStatsText();
    }

    public void PurchaseCrop(CropData crop)
    {
        money -= crop.purchasePrice;
        cropInventory++;
        UpdateStatsText();
    }

    public bool CanPlantCrop()
    {
        if(cropInventory > 0)
        {
            return true;
        }
        return false;
    }

    public void OnBuyCropButton(CropData crop)
    {
        if(money >= crop.purchasePrice)
        {
            PurchaseCrop(crop);
            Debug.Log("Crop purchased!");
        }
           Debug.Log("Not enough money to buy crop!");
    }

    void UpdateStatsText()
    {
        statsText.text = $"Day: {curDay}\nMoney: ${money}\nCrops: {cropInventory} ";
    }
}
