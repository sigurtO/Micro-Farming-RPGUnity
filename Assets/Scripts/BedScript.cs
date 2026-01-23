using UnityEngine;
using TMPro;

public class BedScript : MonoBehaviour
{
    [SerializeField]
    public GameObject Sleepbutton;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Sleepbutton.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Sleepbutton.SetActive(false);
        }
    }
}
