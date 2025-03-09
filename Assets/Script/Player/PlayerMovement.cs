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
       // this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        moveDirection = playerControls.ReadValue<Vector2>();

        //  rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed); 
        setDirection();
        movePlayer();

        //var behaviours = GetComponents<NetworkBehaviour>();
        //for (int i = 0; i < behaviours.Length; i++)
        //{
        //    Debug.Log($"[{i}] {behaviours[i].GetType().Name}");
        //}
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Adjust tag as needed
        {

            if (IsServer)
            {
                //Destroy the other player immediately if on the server
                other.gameObject.GetComponent<NetworkObject>().Despawn(true);
            }
            else
            {
                //If not the server, request the server to despawn the object
                RequestDestroyServerRpc(other.gameObject.GetComponent<NetworkObject>());
            }
        }
    }


    [ServerRpc(RequireOwnership = false)] 
    private void RequestDestroyServerRpc(NetworkObjectReference playerObjectRef)
    {
        if (playerObjectRef.TryGet(out NetworkObject playerObject))
        {
            playerObject.Despawn(true);
        }
    }

}
