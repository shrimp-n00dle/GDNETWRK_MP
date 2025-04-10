using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    //[SerializeField]
    public NetworkVariable<int> currentTimeLeft = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    );

    [SerializeField]
    private int timerDuration = 90;


    private bool hasTimerStarted;
    private float elapsedTime = 0;
  //  private int timerDuration = 90;

    public bool HasTimerStarted
    {
        get { return hasTimerStarted; }
        set { hasTimerStarted = value; }
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        currentTimeLeft.OnValueChanged += OnTimerDurationChanged;
    }

    private void OnTimerDurationChanged(int oldValue, int newValue)
    {
        string formattedTime = FormatTimeToString(newValue);
        UIManager.Instance?.UpdateGameTimer(formattedTime);
    }

    private void Update()
    {
        if (hasTimerStarted && IsOwner)
        {
            elapsedTime += Time.deltaTime;
            int currentDuration = (int)this.elapsedTime - timerDuration;
            //  int current = currentTimeLeft.Value - (int)elapsedTime;

            // Debug.Log("current: " + current);

            RequestModifyCurrentTimeServerRpc(Mathf.Abs(currentDuration));

            if (Mathf.Abs(currentDuration) <= 0)
            {
                DetermineWinnerByPointsServerRpc();

            }
        }
    }

    private string FormatTimeToString(int currentTime)
    {
        string formattedTime = string.Format("{0}:{1:00}", currentTime / 60, currentTime % 60);
        return formattedTime;
    }


    public int CheckPlayerPoints(int playerID)
    {
      //  ulong test = NetworkManager.Singleton.ConnectedClients[(ulong)playerID].ClientId;
        NetworkClient client = NetworkManager.Singleton.ConnectedClients[(ulong)playerID-1];
        GameObject playerObject = client.PlayerObject?.gameObject;

        if(playerObject != null)
        {
            Player playerScript = playerObject.GetComponent<Player>();
            int playerPoints = playerScript.points.Value;

            return playerPoints;
        }

        return 0;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DetermineWinnerByPointsServerRpc()
    {
        string outcome = "";

        int P1Points = CheckPlayerPoints(1);
        int P2Points = CheckPlayerPoints(2);

        if(P1Points > P2Points)
        {
            outcome = "Player 1 wins!";
        }
        else if (P2Points > P1Points)
        {
            outcome = "Player 2 wins!";
        }
        else
        {
            outcome = "Draw!";
        }

        hasTimerStarted = false;
        PlayerSpawner.Instance.DisableAllPlayerMovementsClientRpc();
        ShowGameOutcomeUIClientRpc(outcome);

    }

    [ServerRpc(RequireOwnership = false)]
    public void DetermineWinnerByCollisionServerRpc(int playerID)
    {
        string outcome = "";

        switch(playerID)
        {
            case 1:
                outcome = "Player 1 wins!";
                break;
            case 2:
                outcome = "Player 2 wins!";
                break;
        }

        hasTimerStarted = false;
        PlayerSpawner.Instance.DisableAllPlayerMovementsClientRpc();
        ShowGameOutcomeUIClientRpc(outcome);
    }



    [ServerRpc(RequireOwnership = false)]
    public void CheckIfAllOrbsAreEatenServerRpc()
    {
        int P1Points = CheckPlayerPoints(1);
        int P2Points = CheckPlayerPoints(2);

        if (P1Points + P2Points >= OrbSpawner.Instance.OrbsTotal)
        {
            DetermineWinnerByPointsServerRpc();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void RequestModifyCurrentTimeServerRpc(int currentTime)
    {
        currentTimeLeft.Value = currentTime;
        //Debug.Log("current: " + current);
      //  Debug.Log("Time Left: " + currentTimeLeft.Value);
    }

    [ClientRpc(RequireOwnership = false)]
    private void ShowGameOutcomeUIClientRpc(string outcome)
    {
        UIManager.Instance.ShowWinner(outcome);
    }




    //public void CheckAllPlayerPoints()
    //{
    //    if (!IsServer) return;

    //    int P1points = 0;
    //    int P2points = 0;

    //    foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
    //    {
    //        ulong clientId = client.ClientId;
    //        GameObject playerObject = client.PlayerObject?.gameObject;

    //        if (playerObject != null)
    //        {
    //            Player playerScript = playerObject.GetComponent<Player>();
    //            int playerPoints = playerScript.points.Value;

    //            if (client.ClientId == 0)
    //            {
    //                P1points = playerPoints;
    //            }
    //            else
    //            {
    //                P2points = playerPoints;
    //            }

    //            //  Debug.Log($"Player {clientId} has {playerPoints} points.");
    //        }
    //    }

    //    // Debug.Log("Total points: " +  (P1points + P2points));

    //    //if ((P1points + P2points) % 15 == 0)
    //    //{
    //    //    this.SpawnMysticFruit();
    //    //}
    //}







}
