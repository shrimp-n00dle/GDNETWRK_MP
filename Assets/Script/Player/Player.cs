using ParrelSync.NonCore;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.VisualScripting;
using UnityEngine;

public class Player : NetworkBehaviour
{
	[Header("PlayerInfo")]
	//[SerializeField] private int points = 0;

    [SerializeField]
    public NetworkVariable<int> points = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    );

    //private readonly NetworkVariable<bool> hasMysticFruit = new(writePerm: NetworkVariableWritePermission.Owner);
    [SerializeField]
    private NetworkVariable<bool> hasMysticFruit = new NetworkVariable<bool>(
		false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
	);

    [SerializeField]
    private NetworkVariable<int> buffDuration = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    );

    [SerializeField] private int mysticPowerDuration = 15;

    private PlayerMovement playerMovement;
	private float elapsedTime = 0f;

	//public bool showUI = false;
	//public string UIText;

	private void Start()
	{
		playerMovement = GetComponent<PlayerMovement>();
	}

	private void Update()
	{
		//consists timer for the hasmysticfruit if it is in effect

		//if (IsServer)
		//{
		//	Debug.Log(this.hasMysticFruit.Value);
		//}   }

		if (this.hasMysticFruit.Value && IsOwner)
		{
			this.elapsedTime += Time.deltaTime;
            int currentDuration = (int)this.elapsedTime - mysticPowerDuration;

            RequestModifyBuffDurationServerRpc(Mathf.Abs(currentDuration));

            //Debug.Log("BUFF: " + (int)elapsedTime);
            if (this.elapsedTime >= this.mysticPowerDuration)
			{
                RequestDisableFruitBuffServerRpc();
            }
        }
	}

	public bool HasMysticFruit  
	{
		get { return hasMysticFruit.Value; }  
		set { hasMysticFruit.Value = value; } 
	}

	public int Points
	{
		get { return points.Value; }
		set { points.Value = value; }
	}

    public override void OnNetworkSpawn()
    {
        if (IsServer || IsClient)
        {
            points.OnValueChanged += OnPointsChanged;
            buffDuration.OnValueChanged += OnBuffDurationChanged; 
          //  UIManager.Instance?.UpdatePlayerPointsUI(GetPlayerIndex(), points.Value); // Initial update
        }
    }

    

    public override void OnNetworkDespawn()
    {
        points.OnValueChanged -= OnPointsChanged;
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

					RequestDestroyObjectServerRpc(netPlayer);
					playerMovement.enabled = false; //game over when player has successfully collided with the other player

					//string text = "Player " + OwnerClientId + (int)1 + " won!";
                   // RequestShoGUIServerRpc(text);

                }
			}

		}
		else
		{
            //if (other.gameObject.CompareTag("Ghost"))

        }

        if (other.gameObject.CompareTag("Fruit"))
        {
            // Debug.Log("touch fruit");
            if (IsOwner)
            {
                RequestEnableFruitBuffServerRpc();

                NetworkObject netFruit = other.gameObject.GetComponent<NetworkObject>();
                RequestDestroyObjectServerRpc(netFruit);
            }
        }

        if (other.gameObject.CompareTag("Orb"))
        {
            if (IsOwner)
            {
                RequestAddPointToPlayerServerRpc();
                NetworkObject netOrb = other.gameObject.GetComponent<NetworkObject>();
                RequestDestroyObjectServerRpc(netOrb);

                //if(IsOw) 
                    FruitSpawner.Instance.CheckAllPlayerPoints();
             //   RequestUpdatePlayerPointsUIClientRpc();
            }
        }


    }

    private void OnPointsChanged(int oldValue, int newValue)
    {
        int playerIndex = (int)OwnerClientId + 1;
        UIManager.Instance?.UpdatePlayerPointsUI(playerIndex, newValue);
    }

    private void OnBuffDurationChanged(int oldValue, int newValue)
    {
        int playerIndex = (int)OwnerClientId + 1;
        UIManager.Instance?.UpdatePlayerBuffDurationUI(playerIndex, newValue);

    }


    [ServerRpc(RequireOwnership = false)]
	private void RequestDestroyObjectServerRpc(NetworkObjectReference playerObjectRef)
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
        this.buffDuration.Value = 15;
       // testDebugClientRpc();
        Debug.Log($"Player {OwnerClientId+1} picked up a Mystical Fruit!");
	}

    [ServerRpc(RequireOwnership = false)]
    private void RequestDisableFruitBuffServerRpc()
    {
        this.hasMysticFruit.Value = false; // Server updates the value
		this.elapsedTime = 0f;
        Debug.Log($"Player {OwnerClientId+1}'s mystic fruit power timed out!");
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestAddPointToPlayerServerRpc()
    {
        this.points.Value += 1;
        Debug.Log($"Player {OwnerClientId + 1}' nommed an orb." + $"Total: {this.points.Value}");
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestModifyBuffDurationServerRpc(int duration)
    {
        buffDuration.Value = duration;
    }

    [ClientRpc(RequireOwnership = false)]
    private void RequestUpdatePlayerPointsUIClientRpc()
    {
        UIManager.Instance.UpdatePlayerPointsUI((int)OwnerClientId + 1, this.points.Value);
    }

    [ClientRpc(RequireOwnership = false)]
    private void testDebugClientRpc()
    {
        Debug.Log("Ena4 hjuhuhuhdskh " + this.points.Value);
    }



	//ignore functions below

    //   [ClientRpc] 
    //private void DestroyAllPlayersClientRpc()
    //{
    //	NetworkObject localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject;
    //	localPlayer.Despawn(true);

    //}
    //[ServerRpc(RequireOwnership = false)]
    //private void RequestShoGUIServerRpc(string text)
    //{
    //	ShowGUIClientRpc(text);

    //   }


    //  private void OnGUI()
    //  {
    //if (this.showUI)
    //{
    //	GUIStyle textStyle = new GUIStyle();
    //	textStyle.fontSize = 20;
    //	textStyle.normal.textColor = Color.white;
    //	textStyle.alignment = TextAnchor.MiddleCenter;

    //	Rect textRect = new Rect(10, 10, 300, 50);
    //          //string text = "Player " + OwnerClientId + 1 + " won!";
    //          //GUI.Label(textRect, this.UIText, textStyle);
    //        //  GUI.Label(textRect, "AAAAA", textStyle);
    //      }
    //  }


    //[ClientRpc(RequireOwnership = false)]
    //private void ShowGUIClientRpc(string text)
    //{
    //       NetworkObject localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject;
    //	Debug.Log("hahahaha");
    //	localPlayer.GetComponent<Player>().showUI = true;
    //       localPlayer.GetComponent<Player>().UIText = text;
    //   }

}

