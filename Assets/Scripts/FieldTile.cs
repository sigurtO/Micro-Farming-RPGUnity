using UnityEngine;

public class FieldTile : MonoBehaviour
{

    [SerializeField]
    private GameObject cropPrefab;
    [SerializeField]
    private SpriteRenderer sr;

    private Crops curCrop;

    private bool tilled;

    [Header("Sprites")]
    [SerializeField]
    private Sprite grassSprite;
    [SerializeField]
    private Sprite tilledSprite;
    [SerializeField]
    private Sprite wateredTilledSprite;



    private void Start()
    {
        //set default to grass sprite
        sr.sprite = grassSprite;
    }

    public void Interact(PlayerController player)
    {
        if (!tilled && player.IsHolding("Rake"))
        {
            Till();
        }
        else if(!HasCrop() && GameManager.Instance.CanPlantCrop() && player.IsHolding("Bag"))
        {
            PlantNewCrop(GameManager.Instance.selectedCrop);
        }
        else if (HasCrop() && curCrop.CanHarvest() && player.IsHolding("Collector"))
        {
            curCrop.Harvest();
        }
        else if(player.IsHolding("WaterCan"))
        {
            Water();
        }

    }

    void PlantNewCrop(CropData crop)
    {
        if (!tilled)
            return;

        curCrop = Instantiate(cropPrefab, transform).GetComponent<Crops>();
        curCrop.Plant(crop);

        GameManager.Instance.onNewDay += OnNewDay;
    }

    void Till()
    {
        tilled = true;
        sr.sprite = tilledSprite;
    }

    void Water()
    {

        if (!tilled) // Don't water untilled soil
            return;

        sr.sprite = wateredTilledSprite;

        if(HasCrop())
        {
            curCrop.Water();
        }
    }

    void OnNewDay()
    {
        if(curCrop == null) //reset tile if crop is harvested or has died
        {
            tilled = false;
            sr.sprite = grassSprite;

            GameManager.Instance.onNewDay -= OnNewDay;
        }
        else if (curCrop != null) // if we have crop remove the watered effect on new day
        {
            sr.sprite = tilledSprite;
            curCrop.NewDayCheck();
        }
    }
    bool HasCrop()
    {
        return curCrop != null;
    }

}
