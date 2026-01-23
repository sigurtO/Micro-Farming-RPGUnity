using UnityEngine;
using UnityEngine.Events;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Transform followPoint;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.SetPickUp(this); // call to the method in player controller
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerController.ClearPickUp(this); // call to the method in player controller
        }
    }
    public void PickedUp()
    {
        transform.SetParent(followPoint);
        transform.localPosition = Vector3.zero; // snap to follow point
        transform.localRotation = Quaternion.identity;
    }

    public void Dropped()
    {
      transform.SetParent(null);
    }


}
