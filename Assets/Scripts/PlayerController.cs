using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Vector2 moveInput;
    private bool interactInput;

    private Vector2 facingDir;

    [SerializeField]
    private LayerMask interactLayerMask;
    [SerializeField]
    private Rigidbody2D rig;
    [SerializeField]
    private SpriteRenderer sr;

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
    }

    private void FixedUpdate()
    {
        rig.linearVelocity = moveInput.normalized * moveSpeed; //normalized to prevent faster diagonal movement
    } // here move input actually moves us/

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
}
