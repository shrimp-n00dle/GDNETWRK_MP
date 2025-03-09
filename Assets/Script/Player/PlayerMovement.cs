using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 5;
    [SerializeField] private InputAction playerControls;

    private Vector2 moveDirection = Vector2.zero;
    private string direction;


    public override void OnNetworkSpawn()
    {
        //if (!IsOwner) Destroy(this);
        if(!IsOwner) enabled = false;

        //this.enabled = false;

    }

    private void OnEnable ()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        moveDirection = playerControls.ReadValue<Vector2>();

        //  rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed); 
        setDirection();
        movePlayer();
    }

    private void setDirection()
    {
        if (moveDirection == new Vector2(0, 1)) //going up
        {
            direction = "UP";
        }
        else if (moveDirection == new Vector2(0, -1))
        {
            direction = "DOWN";
        }
        else if (moveDirection == new Vector2(1, 0))
        {
            direction = "RIGHT";
        }
        else if (moveDirection == new Vector2(-1, 0))
        {
            direction = "LEFT";
        }
    }

    private void movePlayer()
    {
        switch (direction)
        {
            case "UP":
                rb.velocity = new Vector2(0, 1 * speed);
                break;
            case "DOWN":
                rb.velocity = new Vector2(0, -1 * speed);
                break;
            case "RIGHT":
                rb.velocity = new Vector2(1 * speed, 0);
                break;
            case "LEFT":
                rb.velocity = new Vector2(-1 * speed, 0);
                break;

        }
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //   // if (PlayerHasEatenFruit) //not yet implemented..ideally, player will be only defeat the other player while
    //                                        //mystical fruit is in effect
    //   // {
    //        if (other.gameObject.CompareTag("Player")) // Adjust tag as needed
    //        {
    //            NetworkObject netPlayer = other.gameObject.GetComponent<NetworkObject>();
    //        // Debug.Log($"Player {player.OwnerClientId} Despawned");
    //        //other.gameObject.
    //            RequestDestroyServerRpc(netPlayer);
    //            this.enabled = false; //game over when player has successfully collided with the other player
    //        }
    //    //}
    //}


    //[ServerRpc(RequireOwnership = false)] 
    //private void RequestDestroyServerRpc(NetworkObjectReference playerObjectRef)
    //{
    //    if (playerObjectRef.TryGet(out NetworkObject playerObject))
    //    {
    //        playerObject.Despawn(true);
    //    }
    //}

}
