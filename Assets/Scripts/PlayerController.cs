using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Vector2 moveInput;
    private bool interactInput;
    private bool pickUpInput;
    private bool dropInput;

    private PickUp inRangePickUp; // item we can pick up
    private PickUp heldPickUp; // item we are holding

    private Vector2 facingDir;

    [SerializeField]
    private LayerMask interactLayerMask;
    [SerializeField]
    private Rigidbody2D rig;
    [SerializeField]
    private SpriteRenderer sr;


    public bool IsHoldingItem => heldPickUp != null;    // Public property to check if holding an item

    public bool IsHolding(string itemName) // Public method to check if holding a specific item
    {
        return heldPickUp != null && heldPickUp.name == itemName;
    }

    void Update()
    {
        if(moveInput.magnitude != 0.0) // if we are pressing down a button
        {
            facingDir = moveInput.normalized;
            if(facingDir.x != 0) // ensure we only flip when moving left or right
            {
                sr.flipX = facingDir.x > 0; // flip the sprite if we are facing left
            }
            
        }

        if(interactInput)
        {
            TryInteractTile();
            interactInput = false;
        }

        if(pickUpInput)
        {
            if(inRangePickUp != null && heldPickUp == null) // if we are in range of an item and not holding one
            {
                heldPickUp = inRangePickUp;
                heldPickUp.PickedUp();
                inRangePickUp = null; // clear the in range reference
                Debug.Log("Picked up item");
            }
            else if (heldPickUp != null)
            {
                Debug.Log("Already holding an item");
            }
            else
            {
                Debug.Log("No item in range to pick up");
            }
            pickUpInput = false;
        }

        if(dropInput)
        {
            if(heldPickUp != null)
            {
                heldPickUp.Dropped();
                heldPickUp = null;
                Debug.Log("Dropped item");
            }
            else
            {
                Debug.Log("No item to drop");
            }
            dropInput = false; // so we dont drop constantly (only once)
        }
    }

    private void FixedUpdate()
    {
        rig.linearVelocity = moveInput.normalized * moveSpeed; //normalized to prevent faster diagonal movement
    } // here move input actually moves us/

    void TryInteractTile()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, facingDir, 0.1f, interactLayerMask);

        if(hit.collider != null)
        {
            FieldTile tile = hit.collider.GetComponent<FieldTile>();
            tile.Interact(this);
        }
    }

    public void SetPickUp(PickUp pickUp) // so we can call from PickUp script to playerController
    {
        inRangePickUp = pickUp;
    }
    public void ClearPickUp(PickUp pickUp)
    {
        if(inRangePickUp == pickUp)
        {
            inRangePickUp = null;
        }
    }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // we bind the keys
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed) 
        {
            interactInput = true;
        }
    }
    public void OnPickUp(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            pickUpInput = true;
        }
    }
    public void OnDrop(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            dropInput = true;
        }
    }
}
