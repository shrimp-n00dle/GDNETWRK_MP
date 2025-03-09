using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.VisualScripting;
using UnityEngine;

public class Player : NetworkBehaviour
{
	[Header("PlayerInfo")]
	//[SerializeField] private bool hasMysticFruit = false;
	[SerializeField] private int points = 0;

	//private readonly NetworkVariable<bool> hasMysticFruit = new(writePerm: NetworkVariableWritePermission.Owner);
	private NetworkVariable<bool> hasMysticFruit = new NetworkVariable<bool>(
		false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
	);

	private PlayerMovement playerMovement;

	private void Start()
	{
		playerMovement = GetComponent<PlayerMovement>();
	}

	private void Update()
	{
		//consists timer for the hasmysticfruit if it is in effect

		if (IsServer)
		{
			Debug.Log(this.hasMysticFruit.Value);
		}
		
	}

	public bool HasMysticFruit  
	{
		get { return hasMysticFruit.Value; }  
		set { hasMysticFruit.Value = value; } 
	}

	public int Points
	{
		get { return points; }
		set { points = value; }
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		 if (this.hasMysticFruit.Value) //not yet implemented..ideally, player will be only defeat the other player while

		 {
			if (other.gameObject.CompareTag("Player")) // Adjust tag as needed
			{
				bool otherPlayerHasFruit = other.gameObject.GetComponent<Player>().HasMysticFruit;

				if(!otherPlayerHasFruit) {
					Debug.Log("collided other player with fruit");
					NetworkObject netPlayer = other.gameObject.GetComponent<NetworkObject>();

					RequestDestroyServerRpc(netPlayer);
					playerMovement.enabled = false; //game over when player has successfully collided with the other player


				}
			}

		}


		if (other.gameObject.CompareTag("Fruit"))
		{
		   // Debug.Log("touch fruit");
			if(IsOwner)
			{
				RequestEnableFruitBuffServerRpc();
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

	[ServerRpc(RequireOwnership = false)]
	private void RequestEnableFruitBuffServerRpc()
	{
		this.hasMysticFruit.Value = true; // Server updates the value
		Debug.Log($"Player {OwnerClientId} picked up a Mystical Fruit!");
	}

	[ClientRpc] 
	private void DestroyAllPlayersClientRpc()
	{
		NetworkObject localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject;
		localPlayer.Despawn(true);
		
	}

}

