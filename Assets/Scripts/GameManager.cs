using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int curDay;
    public int money;

    [SerializeField]
    private Dictionary<CropData, int> cropInventory = new Dictionary<CropData, int>();
    public CropData selectedCrop;

    public CropData wheat;
    public CropData potato;

    public TextMeshProUGUI statsText;

    public event UnityAction onNewDay;

    public GameObject wheatButton;
    public GameObject PotatoButton;

    public GameObject buyWheatButton;
    public GameObject buyPotatoButton;
    public GameObject openShopButton;
    public GameObject closeShopButton;
    public GameObject shopUi;

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
        cropInventory[wheat] = 2;
        cropInventory[potato] = 2;
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
        cropInventory[cropData]--;
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
        if(cropInventory.ContainsKey(crop)) // checks name of the crop
        {
            cropInventory[crop]++;
            UpdateStatsText();
        }
        else
        {
            cropInventory[crop] = 1;
            UpdateStatsText();
        }
    }

    public int GetCropCount(CropData crop)
    {
        if(cropInventory.ContainsKey(crop))
        {
            return cropInventory[crop];
        }
        return 0;
    }

    public bool CanPlantCrop()
    {
       if(selectedCrop != null && GetCropCount(selectedCrop) > 0)
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

    public void OnSelectWheat()
    {
        selectedCrop = wheat;
    }
    public void OnSelectPotato()
    {
        selectedCrop = potato;
    }

    public void OnOpenShop()
    {
        buyPotatoButton.SetActive(true);
        buyWheatButton.SetActive(true);
        shopUi.SetActive(true);
        closeShopButton.SetActive(true);
        openShopButton.SetActive(false);
    }
    public void OnCloseShop()
    {
        buyPotatoButton.SetActive(false);
        buyWheatButton.SetActive(false);
        shopUi.SetActive(false);
        closeShopButton.SetActive(false);
        openShopButton.SetActive(true);

    }

    public void OnBagPickedUpShowUi(PlayerController player)
    {
        if(player.IsHolding("Bag"))
        {
           wheatButton.SetActive(true);
           PotatoButton.SetActive(true);
        }
        else
        {
              wheatButton.SetActive(false);
              PotatoButton.SetActive(false);
        }
    }

        void UpdateStatsText()
    {
        int wheatCount = GetCropCount(wheat);
        int potatoCount = GetCropCount(potato);

        statsText.text = $"Day: {curDay}\nMoney: ${money}\nWheat: {wheatCount}\nPotato: {potatoCount}";
    }
}
